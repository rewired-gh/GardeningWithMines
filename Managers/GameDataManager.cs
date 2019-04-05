using GardeningWithMines.Models;
using System;
using System.Collections.Generic;
using static GardeningWithMines.Properties.Settings;

namespace GardeningWithMines.Managers
{
    internal static class GameDataManager
    {
        //Define 2D around
        internal readonly static int[,] Around = { { -1, -1 }, { -1, 0 }, { -1,  1 },
                                                   {  0, -1 },            {  0,  1 },
                                                   {  1, -1 }, {  1, 0 }, {  1,  1 } };

        //Create new data
        static GameDataManager()
        {
            DataRefresh();
        }

        public static GameData CurrentGameData { get; set; }

        public static void GameRefresh()
        {
            DataRefresh();

            //Refresh UI
            ((MainWindow)App.Current.MainWindow).Clear();
            ((MainWindow)App.Current.MainWindow).Init();
        }

        public static bool IsInRange(int row, int column)
        {
            //Range: [0, row), [0, column)
            return row >= 0 && column >= 0 && row < Default.MapRow && column < Default.MapColumn;
        }

        private static void DataRefresh()
        {
            CurrentGameData = new GameData()
            {
                HaveShownNotification = false,
                MinesCount = Default.MinesCount,
                MapColumn = Default.MapColumn,
                MapRow = Default.MapRow,
                UnclickedSafeBlockCount = Default.MapRow * Default.MapColumn - Default.MinesCount,
                SteppedCount = 0,
                MinesMap = new int[Default.MapRow, Default.MapColumn]
            };

            GenerateMinesMap();
        }

        private static void GenerateMinesMap()
        {
            Random random = new Random();
            List<Position> positions = new List<Position>();

            //Add all positions into list
            for (int i = 0; i < CurrentGameData.MapRow; i++)
            {
                for (int j = 0; j < CurrentGameData.MapColumn; j++)
                {
                    positions.Add(new Position(i, j));
                }
            }

            //Pick one out from list and set it as mine
            for (int i = 0; i < CurrentGameData.MinesCount; i++)
            {
                int currentIndex = random.Next(positions.Count);
                SetMine(positions[currentIndex].Row, positions[currentIndex].Column);
                positions.RemoveAt(currentIndex);
            }
        }

        private static void SetMine(int row, int column)
        {
            //Set mine
            CurrentGameData.MinesMap[row, column] = -1;

            //Tell neighbours
            int tempRow, tempColumn;
            for (int i = 0; i < 8; ++i)
            {
                tempRow = row + Around[i, 0];
                tempColumn = column + Around[i, 1];
                if (IsInRange(tempRow, tempColumn) && CurrentGameData.MinesMap[tempRow, tempColumn] != -1)
                {
                    CurrentGameData.MinesMap[tempRow, tempColumn]++;
                }
            }
        }
    }
}