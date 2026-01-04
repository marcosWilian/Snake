using System;

namespace ConsoleApplication2.Rendering
{
    /// <summary>
    /// Responsável por desenhar elementos do jogo utilizando um buffer de console.
    /// </summary>
    public class ConsoleRenderer
    {
        private readonly ConsoleBuffer _buffer;
        private readonly int _width;
        private readonly int _height;

        public ConsoleRenderer(int width, int height)
        {
            _width = width;
            _height = height;
            _buffer = new ConsoleBuffer(width, height);
        }

        public void Clear()
        {
            _buffer.Clear(' ');
        }

        public void DrawBorder(int x1, int y1, int x2, int y2, char borderChar = '#')
        {
            for (int x = x1; x <= x2; x++)
            {
                _buffer.Write(x, y1, borderChar);
                _buffer.Write(x, y2, borderChar);
            }

            for (int y = y1; y <= y2; y++)
            {
                _buffer.Write(x1, y, borderChar);
                _buffer.Write(x2, y, borderChar);
            }
        }

        public void DrawText(int x, int y, string text)
        {
            _buffer.Write(x, y, text);
        }

        public void DrawPoint(Position position, char character)
        {
            _buffer.Write(position.X, position.Y, character);
        }

        public void Flush()
        {
            _buffer.Flush();
        }
    }
}
