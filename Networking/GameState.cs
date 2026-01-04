using System.Collections.Generic;
using ConsoleApplication2;

namespace ConsoleApplication2.Networking
{
    /// <summary>
    /// Representa um snapshot do estado do jogo recebido pelo cliente.
    /// </summary>
    public class GameState
    {
        public Position Food { get; }
        public Dictionary<int, List<Position>> Snakes { get; }

        public GameState(Position food, Dictionary<int, List<Position>> snakes)
        {
            Food = food;
            Snakes = snakes;
        }
    }
}
