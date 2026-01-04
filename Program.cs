using ConsoleApplication2.Game;
using ConsoleApplication2.Rendering;

namespace ConsoleApplication2
{
    internal class Program
    {
        private const int ConsoleWidth = 120;
        private const int ConsoleHeight = 30;
        private const int DefaultPort = 9000;

        private static void Main(string[] args)
        {
            if (args.Length > 0 && args[0].Equals("server", System.StringComparison.OrdinalIgnoreCase))
            {
                StartServer();
                return;
            }

            if (args.Length > 0 && args[0].Equals("client", System.StringComparison.OrdinalIgnoreCase))
            {
                var host = args.Length > 1 ? args[1] : "localhost";
                var port = args.Length > 2 && int.TryParse(args[2], out var parsedPort) ? parsedPort : DefaultPort;
                StartClient(host, port);
                return;
            }

            StartLocal();
        }

        private static void StartServer()
        {
            var settings = new GameSettings(playAreaWidth: 100, playAreaHeight: 20, playAreaOffsetX: 10, playAreaOffsetY: 5, refreshIntervalMs: 120);
            using (var server = new Networking.GameServer(settings, DefaultPort))
            {
                server.Start();
            }
        }

        private static void StartClient(string host, int port)
        {
            using (var client = new Networking.GameClient(host, port, ConsoleWidth, ConsoleHeight))
            {
                client.Start();
            }
        }

        private static void StartLocal()
        {
            System.Console.CursorVisible = false;
            System.Console.SetWindowSize(ConsoleWidth, ConsoleHeight);
            System.Console.SetBufferSize(ConsoleWidth, ConsoleHeight);

            var settings = new GameSettings(playAreaWidth: 100, playAreaHeight: 20, playAreaOffsetX: 10, playAreaOffsetY: 5, refreshIntervalMs: 120);
            var renderer = new ConsoleRenderer(ConsoleWidth, ConsoleHeight);
            var snake = new Snake(initialPosition: new Position(settings.PlayAreaOffsetX + settings.PlayAreaWidth / 2, settings.PlayAreaOffsetY + settings.PlayAreaHeight / 2), initialLength: 3, initialDirection: Direction.Right);
            var foodGenerator = new FoodGenerator(settings);
            var game = new GameLoop(settings, snake, foodGenerator, renderer);

            game.Run();
        }
    }
}
