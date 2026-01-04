using System;

namespace ConsoleApplication2.Game
{
    public class FoodGenerator
    {
        private readonly Random _random = new Random();
        private readonly GameSettings _settings;

        public FoodGenerator(GameSettings settings)
        {
            _settings = settings;
        }

        public Position NextFoodPosition()
        {
            var x = _random.Next(_settings.PlayAreaOffsetX + 1, _settings.PlayAreaOffsetX + _settings.PlayAreaWidth - 1);
            var y = _random.Next(_settings.PlayAreaOffsetY + 1, _settings.PlayAreaOffsetY + _settings.PlayAreaHeight - 1);

            return new Position(x, y);
        }
    }
}
