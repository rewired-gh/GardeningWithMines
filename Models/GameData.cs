namespace GardeningWithMines.Models
{
    internal class GameData
    {
        public bool HaveShownNotification { get; set; }
        public int MapColumn { get; set; }
        public int MapRow { get; set; }
        public int MinesCount { get; set; }
        public int[,] MinesMap { get; set; }
        public int SteppedCount { get; set; }
        public int UnclickedSafeBlockCount { get; set; }
    }
}