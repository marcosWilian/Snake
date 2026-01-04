using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ConsoleApplication2;

namespace ConsoleApplication2.Networking
{
    /// <summary>
    /// Encapsula a conexão TCP de um cliente com o servidor de jogo.
    /// </summary>
    public class ClientConnection : IDisposable
    {
        private readonly TcpClient _tcpClient;
        private readonly GameServer _server;
        private readonly object _sendLock = new object();
        private readonly StreamReader _reader;
        private readonly StreamWriter _writer;
        private bool _running;

        public int PlayerId { get; }

        public ClientConnection(int playerId, TcpClient tcpClient, GameServer server)
        {
            PlayerId = playerId;
            _tcpClient = tcpClient;
            _server = server;
            var stream = tcpClient.GetStream();
            _reader = new StreamReader(stream, Encoding.UTF8);
            _writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
        }

        public void Start()
        {
            _running = true;
            var thread = new Thread(Listen) { IsBackground = true };
            thread.Start();
        }

        public void Send(string message)
        {
            lock (_sendLock)
            {
                if (_running)
                {
                    _writer.WriteLine(message);
                }
            }
        }

        private void Listen()
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

                    if (line.StartsWith("DIR|", StringComparison.OrdinalIgnoreCase))
                    {
                        var parts = line.Split('|');
                        if (parts.Length > 1 && Enum.TryParse(parts[1], out Direction direction))
                        {
                            _server.HandleDirectionChange(PlayerId, direction);
                        }
                    }
                }
            }
            finally
            {
                Dispose();
                _server.Disconnect(PlayerId);
            }
        }

        public void Dispose()
        {
            _running = false;
            try
            {
                _tcpClient.Close();
            }
            catch
            {
                // ignorar erros ao fechar.
            }
        }
    }
}
