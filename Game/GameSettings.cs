namespace ConsoleApplication2.Game
{
    public class GameSettings
    {
        public int PlayAreaWidth { get; }
        public int PlayAreaHeight { get; }
        public int PlayAreaOffsetX { get; }
        public int PlayAreaOffsetY { get; }
        public int RefreshIntervalMs { get; }

        public GameSettings(int playAreaWidth, int playAreaHeight, int playAreaOffsetX, int playAreaOffsetY, int refreshIntervalMs)
        {
            PlayAreaWidth = playAreaWidth;
            PlayAreaHeight = playAreaHeight;
            PlayAreaOffsetX = playAreaOffsetX;
            PlayAreaOffsetY = playAreaOffsetY;
            RefreshIntervalMs = refreshIntervalMs;
        }
    }
}
