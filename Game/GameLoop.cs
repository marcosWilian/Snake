using System;
using System.Threading;
using ConsoleApplication2.Rendering;

namespace ConsoleApplication2.Game
{
    public class GameLoop
    {
        private readonly GameSettings _settings;
        private readonly Snake _snake;
        private readonly FoodGenerator _foodGenerator;
        private readonly ConsoleRenderer _renderer;
        private readonly object _directionLock = new object();
        private Position _food;
        private bool _gameOver;

        public GameLoop(GameSettings settings, Snake snake, FoodGenerator foodGenerator, ConsoleRenderer renderer)
        {
            _settings = settings;
            _snake = snake;
            _foodGenerator = foodGenerator;
            _renderer = renderer;
        }

        public void Run()
        {
            Console.CursorVisible = false;
            InitializeFood();
            var inputThread = new Thread(ReadInput) { IsBackground = true };
            inputThread.Start();

            while (!_gameOver)
            {
                Update();
                Render();
                Thread.Sleep(_settings.RefreshIntervalMs);
            }

            Console.SetCursorPosition(_settings.PlayAreaOffsetX + _settings.PlayAreaWidth / 2 - 4, _settings.PlayAreaOffsetY + _settings.PlayAreaHeight / 2);
            Console.Write("GAME OVER");
            Console.ReadKey(true);
        }

        private void Update()
        {
            bool shouldGrow = _snake.Head.Equals(_food);
            _snake.Move(shouldGrow);

            if (shouldGrow)
            {
                InitializeFood();
            }

            if (IsOutOfBounds(_snake.Head) || _snake.HasSelfCollision())
            {
                _gameOver = true;
            }
        }

        private void Render()
        {
            _renderer.Clear();
            _renderer.DrawBorder(_settings.PlayAreaOffsetX, _settings.PlayAreaOffsetY, _settings.PlayAreaOffsetX + _settings.PlayAreaWidth, _settings.PlayAreaOffsetY + _settings.PlayAreaHeight);

            foreach (var segment in _snake.Body)
            {
                _renderer.DrawPoint(segment, '■');
            }

            _renderer.DrawPoint(_food, '*');
            _renderer.Flush();
        }

        private void InitializeFood()
        {
            _food = _foodGenerator.NextFoodPosition();
        }

        private bool IsOutOfBounds(Position position)
        {
            return position.X <= _settings.PlayAreaOffsetX || position.X >= _settings.PlayAreaOffsetX + _settings.PlayAreaWidth ||
                   position.Y <= _settings.PlayAreaOffsetY || position.Y >= _settings.PlayAreaOffsetY + _settings.PlayAreaHeight;
        }

        private void ReadInput()
        {
            while (!_gameOver)
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
                    lock (_directionLock)
                    {
                        _snake.ChangeDirection(direction.Value);
                    }
                }
            }
        }
    }
}
