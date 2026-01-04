using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApplication2.Game
{
    public class Snake
    {
        private readonly LinkedList<Position> _bodyPositions = new LinkedList<Position>();
        public Direction Direction { get; private set; }
        public Position Head => _bodyPositions.First.Value;
        public IEnumerable<Position> Body => _bodyPositions;

        public Snake(Position initialPosition, int initialLength, Direction initialDirection)
        {
            Direction = initialDirection;
            for (int i = 0; i < initialLength; i++)
            {
                var position = new Position(initialPosition.X - i, initialPosition.Y);
                _bodyPositions.AddLast(position);
            }
        }

        public void ChangeDirection(Direction newDirection)
        {
            if (IsOpposite(Direction, newDirection))
            {
                return;
            }

            Direction = newDirection;
        }

        public Position Move(bool grow)
        {
            var newHead = Head.Translate(Direction);
            _bodyPositions.AddFirst(newHead);

            if (!grow)
            {
                _bodyPositions.RemoveLast();
            }

            return newHead;
        }

        public bool HasSelfCollision()
        {
            return _bodyPositions.Skip(1).Any(segment => segment.Equals(Head));
        }

        private static bool IsOpposite(Direction current, Direction next)
        {
            return (current == Direction.Left && next == Direction.Right) ||
                   (current == Direction.Right && next == Direction.Left) ||
                   (current == Direction.Up && next == Direction.Down) ||
                   (current == Direction.Down && next == Direction.Up);
        }
    }
}
