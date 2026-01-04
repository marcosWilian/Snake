using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ConsoleApplication2;
using ConsoleApplication2.Game;

namespace ConsoleApplication2.Networking
{
    /// <summary>
    /// Servidor TCP responsável por gerenciar o estado do jogo e sincronizar os clientes.
    /// </summary>
    public class GameServer : IDisposable
    {
        private readonly Dictionary<int, ClientConnection> _clients = new Dictionary<int, ClientConnection>();
        private readonly Dictionary<int, Snake> _snakes = new Dictionary<int, Snake>();
        private readonly TcpListener _listener;
        private readonly GameSettings _settings;
        private readonly FoodGenerator _foodGenerator;
        private readonly object _stateLock = new object();
        private readonly Random _random = new Random();
        private bool _running;
        private Thread _acceptThread;
        private Position _food;
        private int _nextPlayerId = 1;

        public GameServer(GameSettings settings, int port)
        {
            _settings = settings;
            _foodGenerator = new FoodGenerator(settings);
            _listener = new TcpListener(IPAddress.Any, port);
        }

        public void Start()
        {
            Console.WriteLine("Iniciando servidor...");
            _running = true;
            _food = _foodGenerator.NextFoodPosition();

            _listener.Start();
            _acceptThread = new Thread(AcceptClients) { IsBackground = true };
            _acceptThread.Start();

            RunGameLoop();
        }

        public void Dispose()
        {
            _running = false;
            lock (_stateLock)
            {
                foreach (var client in _clients.Values)
                {
                    client.Dispose();
                }

                _clients.Clear();
                _snakes.Clear();
            }

            try
            {
                _listener.Stop();
            }
            catch
            {
                // ignorar erros de parada.
            }
        }

        private void AcceptClients()
        {
            while (_running)
            {
                try
                {
                    var tcpClient = _listener.AcceptTcpClient();
                    var playerId = _nextPlayerId++;
                    var connection = new ClientConnection(playerId, tcpClient, this);

                    lock (_stateLock)
                    {
                        _clients[playerId] = connection;
                        _snakes[playerId] = CreateNewSnake();
                    }

                    connection.Start();
                    SendAssignment(connection);
                    Console.WriteLine($"Jogador {playerId} conectado.");
                }
                catch (SocketException)
                {
                    if (!_running)
                    {
                        return;
                    }

                    Console.WriteLine("Erro ao aceitar conexão.");
                }
            }
        }

        private void RunGameLoop()
        {
            while (_running)
            {
                UpdateSnakes();
                BroadcastState();
                Thread.Sleep(_settings.RefreshIntervalMs);
            }
        }

        private void UpdateSnakes()
        {
            var playersToRemove = new List<int>();

            lock (_stateLock)
            {
                foreach (var snakeEntry in _snakes)
                {
                    var snake = snakeEntry.Value;
                    bool shouldGrow = snake.Head.Equals(_food);
                    snake.Move(shouldGrow);

                    if (shouldGrow)
                    {
                        _food = _foodGenerator.NextFoodPosition();
                    }
                }

                foreach (var snakeEntry in _snakes)
                {
                    var head = snakeEntry.Value.Head;
                    if (IsOutOfBounds(head) || snakeEntry.Value.HasSelfCollision() || CollidesWithOtherSnake(snakeEntry.Key, head))
                    {
                        playersToRemove.Add(snakeEntry.Key);
                    }
                }

                foreach (var playerId in playersToRemove)
                {
                    Console.WriteLine($"Jogador {playerId} foi removido por colisão.");
                    _snakes.Remove(playerId);
                    if (_clients.TryGetValue(playerId, out var connection))
                    {
                        connection.Dispose();
                        _clients.Remove(playerId);
                    }
                }
            }
        }

        private void BroadcastState()
        {
            string message;
            lock (_stateLock)
            {
                message = BuildStateMessage();
            }

            foreach (var client in _clients.Values.ToList())
            {
                client.Send(message);
            }
        }

        private bool IsOutOfBounds(Position position)
        {
            return position.X <= _settings.PlayAreaOffsetX || position.X >= _settings.PlayAreaOffsetX + _settings.PlayAreaWidth ||
                   position.Y <= _settings.PlayAreaOffsetY || position.Y >= _settings.PlayAreaOffsetY + _settings.PlayAreaHeight;
        }

        private bool CollidesWithOtherSnake(int playerId, Position head)
        {
            foreach (var entry in _snakes)
            {
                if (entry.Key == playerId)
                {
                    continue;
                }

                if (entry.Value.Body.Any(segment => segment.Equals(head)))
                {
                    return true;
                }
            }

            return false;
        }

        internal void HandleDirectionChange(int playerId, Direction direction)
        {
            lock (_stateLock)
            {
                if (_snakes.TryGetValue(playerId, out var snake))
                {
                    snake.ChangeDirection(direction);
                }
            }
        }

        internal void Disconnect(int playerId)
        {
            lock (_stateLock)
            {
                _snakes.Remove(playerId);
                _clients.Remove(playerId);
            }

            Console.WriteLine($"Jogador {playerId} desconectado.");
        }

        private Snake CreateNewSnake()
        {
            var x = _random.Next(_settings.PlayAreaOffsetX + 2, _settings.PlayAreaOffsetX + _settings.PlayAreaWidth - 2);
            var y = _random.Next(_settings.PlayAreaOffsetY + 2, _settings.PlayAreaOffsetY + _settings.PlayAreaHeight - 2);
            return new Snake(new Position(x, y), initialLength: 3, initialDirection: Direction.Right);
        }

        private void SendAssignment(ClientConnection connection)
        {
            var message = $"ASSIGN|{connection.PlayerId}|{_settings.PlayAreaWidth}|{_settings.PlayAreaHeight}|{_settings.PlayAreaOffsetX}|{_settings.PlayAreaOffsetY}|{_settings.RefreshIntervalMs}";
            connection.Send(message);
        }

        private string BuildStateMessage()
        {
            var builder = new StringBuilder();
            builder.Append("STATE|");
            builder.Append(_food.X).Append(',').Append(_food.Y);

            foreach (var snakeEntry in _snakes)
            {
                builder.Append('|').Append(snakeEntry.Key).Append(':');
                bool firstSegment = true;
                foreach (var segment in snakeEntry.Value.Body)
                {
                    if (!firstSegment)
                    {
                        builder.Append(';');
                    }

                    builder.Append(segment.X).Append(',').Append(segment.Y);
                    firstSegment = false;
                }
            }

            return builder.ToString();
        }
    }
}
