using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ConsoleApplication2;
using ConsoleApplication2.Game;
using ConsoleApplication2.Rendering;

namespace ConsoleApplication2.Networking
{
    /// <summary>
    /// Cliente TCP responsável por enviar comandos e renderizar o estado recebido do servidor.
    /// </summary>
    public class GameClient : IDisposable
    {
        private readonly string _host;
        private readonly int _port;
        private readonly int _consoleWidth;
        private readonly int _consoleHeight;
        private readonly object _stateLock = new object();
        private TcpClient _tcpClient;
        private StreamReader _reader;
        private StreamWriter _writer;
        private GameSettings _settings;
        private ConsoleRenderer _renderer;
        private bool _running;
        private int _playerId;
        private GameState _latestState;

        public GameClient(string host, int port, int consoleWidth, int consoleHeight)
        {
            _host = host;
            _port = port;
            _consoleWidth = consoleWidth;
            _consoleHeight = consoleHeight;
        }

        public void Start()
        {
            Console.WriteLine($"Conectando a {_host}:{_port}...");
            _tcpClient = new TcpClient();
            _tcpClient.Connect(_host, _port);
            var stream = _tcpClient.GetStream();
            _reader = new StreamReader(stream, Encoding.UTF8);
            _writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
            _running = true;

            var listenThread = new Thread(ListenToServer) { IsBackground = true };
            listenThread.Start();

            var inputThread = new Thread(ReadInput) { IsBackground = true };
            inputThread.Start();

            RunRenderLoop();
        }

        public void Dispose()
        {
            _running = false;
            try
            {
                _tcpClient?.Close();
            }
            catch
            {
                // ignorar erros ao fechar.
            }
        }

        private void ListenToServer()
        {
            try
            {
                while (_running)
                {
                    var line = _reader.ReadLine();
                    if (line == null)
                    {
                        break;
                    }

                    if (line.StartsWith("ASSIGN|", StringComparison.OrdinalIgnoreCase))
                    {
                        ParseAssignment(line);
                    }
                    else if (line.StartsWith("STATE|", StringComparison.OrdinalIgnoreCase))
                    {
                        var state = ParseState(line);
                        lock (_stateLock)
                        {
                            _latestState = state;
                        }
                    }
                }
            }
            finally
            {
                _running = false;
            }
        }

        private void ParseAssignment(string message)
        {
            // ASSIGN|<playerId>|playAreaWidth|playAreaHeight|offsetX|offsetY|refreshMs
            var parts = message.Split('|');
            if (parts.Length < 7)
            {
                return;
            }

            _playerId = int.Parse(parts[1]);
            var settings = new GameSettings(
                playAreaWidth: int.Parse(parts[2]),
                playAreaHeight: int.Parse(parts[3]),
                playAreaOffsetX: int.Parse(parts[4]),
                playAreaOffsetY: int.Parse(parts[5]),
                refreshIntervalMs: int.Parse(parts[6]));

            _settings = settings;
            Console.WriteLine($"Conectado como jogador {_playerId}");

            Console.CursorVisible = false;
            Console.SetWindowSize(_consoleWidth, _consoleHeight);
            Console.SetBufferSize(_consoleWidth, _consoleHeight);
            _renderer = new ConsoleRenderer(_consoleWidth, _consoleHeight);
        }

        private GameState ParseState(string message)
        {
            // STATE|foodX,foodY|playerId:x1,y1;x2,y2|playerId2:...
            var parts = message.Split('|');
            if (parts.Length < 2 || _settings == null)
            {
                return null;
            }

            var foodCoordinates = parts[1].Split(',');
            var food = new Position(int.Parse(foodCoordinates[0]), int.Parse(foodCoordinates[1]));
            var snakes = new Dictionary<int, List<Position>>();

            for (int i = 2; i < parts.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(parts[i]))
                {
                    continue;
                }

                var playerSection = parts[i].Split(':');
                if (playerSection.Length != 2)
                {
                    continue;
                }

                int playerId = int.Parse(playerSection[0]);
                var segments = playerSection[1].Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                var positions = new List<Position>();
                foreach (var segment in segments)
                {
                    var coordinates = segment.Split(',');
                    positions.Add(new Position(int.Parse(coordinates[0]), int.Parse(coordinates[1])));
                }

                snakes[playerId] = positions;
            }

            return new GameState(food, snakes);
        }

        private void RunRenderLoop()
        {
            while (_running)
            {
                GameState state;
                lock (_stateLock)
                {
                    state = _latestState;
                }

                if (state != null && _renderer != null)
                {
                    RenderState(state);
                }

                Thread.Sleep(_settings?.RefreshIntervalMs ?? 50);
            }
        }

        private void RenderState(GameState state)
        {
            _renderer.Clear();
            _renderer.DrawBorder(_settings.PlayAreaOffsetX, _settings.PlayAreaOffsetY, _settings.PlayAreaOffsetX + _settings.PlayAreaWidth, _settings.PlayAreaOffsetY + _settings.PlayAreaHeight);

            foreach (var snake in state.Snakes)
            {
                var character = snake.Key == _playerId ? '■' : '□';
                foreach (var segment in snake.Value)
                {
                    _renderer.DrawPoint(segment, character);
                }
            }

            _renderer.DrawPoint(state.Food, '*');
            _renderer.DrawText(_settings.PlayAreaOffsetX, _settings.PlayAreaOffsetY - 1, $"Jogador: {_playerId}  Cobras: {state.Snakes.Count}");
            _renderer.Flush();
        }

        private void ReadInput()
        {
            while (_running)
            {
                if (!Console.KeyAvailable)
                {
                    Thread.Sleep(10);
                    continue;
                }

                var keyInfo = Console.ReadKey(true);
                Direction? direction = null;

                switch (keyInfo.Key)
                {
                    case ConsoleKey.LeftArrow:
                        direction = Direction.Left;
                        break;
                    case ConsoleKey.RightArrow:
                        direction = Direction.Right;
                        break;
                    case ConsoleKey.UpArrow:
                        direction = Direction.Up;
                        break;
                    case ConsoleKey.DownArrow:
                        direction = Direction.Down;
                        break;
                }

                if (direction.HasValue)
                {
                    SendDirection(direction.Value);
                }
            }
        }

        private void SendDirection(Direction direction)
        {
            if (_writer != null)
            {
                _writer.WriteLine($"DIR|{direction}");
            }
        }
    }
}
