using System;
using System.Text;

namespace ConsoleApplication2.Rendering
{
    /// <summary>
    /// Bufferiza as operações no console para diminuir flicker.
    /// </summary>
    public class ConsoleBuffer
    {
        private readonly int _width;
        private readonly int _height;
        private readonly char[,] _buffer;

        public ConsoleBuffer(int width, int height)
        {
            _width = width;
            _height = height;
            _buffer = new char[height, width];
            Clear(' ');
        }

        public void Clear(char fillChar = ' ')
        {
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    _buffer[y, x] = fillChar;
                }
            }
        }

        public void Write(int x, int y, char character)
        {
            if (x < 0 || y < 0 || x >= _width || y >= _height)
            {
                return;
            }

            _buffer[y, x] = character;
        }

        public void Write(int x, int y, string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                Write(x + i, y, text[i]);
            }
        }

        public void Flush()
        {
            Console.SetCursorPosition(0, 0);
            var builder = new StringBuilder();
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    builder.Append(_buffer[y, x]);
                }

                builder.Append(Environment.NewLine);
            }

            Console.Write(builder.ToString());
        }
    }
}
