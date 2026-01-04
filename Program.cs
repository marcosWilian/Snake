using ConsoleApplication2.Game;
using ConsoleApplication2.Rendering;

namespace ConsoleApplication2
{
    internal class Program
    {
        private static void Main()
        {
            const int consoleWidth = 120;
            const int consoleHeight = 30;
            System.Console.SetWindowSize(consoleWidth, consoleHeight);
            System.Console.SetBufferSize(consoleWidth, consoleHeight);

            var settings = new GameSettings(playAreaWidth: 100, playAreaHeight: 20, playAreaOffsetX: 10, playAreaOffsetY: 5, refreshIntervalMs: 120);
            var renderer = new ConsoleRenderer(consoleWidth, consoleHeight);
            var snake = new Snake(initialPosition: new Position(settings.PlayAreaOffsetX + settings.PlayAreaWidth / 2, settings.PlayAreaOffsetY + settings.PlayAreaHeight / 2), initialLength: 3, initialDirection: Direction.Right);
            var foodGenerator = new FoodGenerator(settings);
            var game = new GameLoop(settings, snake, foodGenerator, renderer);

            game.Run();
        }
    }
}
