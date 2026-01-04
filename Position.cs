using System;

namespace ConsoleApplication2
{
    /// <summary>
    /// Representa uma posição do tabuleiro.
    /// </summary>
    public struct Position : IEquatable<Position>
    {
        public int X { get; }
        public int Y { get; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Position Translate(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return new Position(X, Y - 1);
                case Direction.Down:
                    return new Position(X, Y + 1);
                case Direction.Left:
                    return new Position(X - 1, Y);
                case Direction.Right:
                    return new Position(X + 1, Y);
                default:
                    return this;
            }
        }

        public bool Equals(Position other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (obj is Position position)
            {
                return Equals(position);
            }

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }
    }
}
