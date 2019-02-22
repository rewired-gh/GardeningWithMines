using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using static GardeningWithMines.Managers.ControlsManager;
using static GardeningWithMines.Properties.Settings;

namespace GardeningWithMines.Managers
{
    public struct Position
    {
        public int Row, Column;

        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }

    public static class MapManager
    {
        private readonly static int[,] Around = { { -1, -1 }, { -1, 0 }, { -1,  1 },
                                                  {  0, -1 },            {  0,  1 },
                                                  {  1, -1 }, {  1, 0 }, {  1,  1 } };

        static MapManager()
        {
            Random random = new Random();
            int mapRow = Default.MapRow, mapColumn = Default.MapColumn;
            MineMap = new int[mapRow, mapColumn];
            List<Position> positions = new List<Position>();
            for (int i = 0; i < mapRow; i++)
            {
                for (int j = 0; j < mapColumn; j++)
                {
                    positions.Add(new Position(i, j));
                }
            }
            for (int i = 0; i < Default.MinesCount; i++)
            {
                int currentIndex = random.Next(positions.Count - 1);
                SetMine(positions[currentIndex].Row, positions[currentIndex].Column);
                positions.RemoveAt(currentIndex);
            }
        }

        public static int[,] MineMap { get; set; }

        public static bool Click(int row, int column)
        {
            if (BlockButtons[row, column].Content == null)
            {
                BlockButtons[row, column].IsEnabled = false;
                if (MineMap[row, column] == -1)
                {
                    BlockButtons[row, column].FontFamily = new FontFamily(Default.IconFontFamily);
                    BlockButtons[row, column].Content = Default.MineCharacter;
                    BlockButtons[row, column].SetBinding(Button.FontSizeProperty, IconFontSizeBinding);
                    return true;
                }
                else if (MineMap[row, column] == 0)
                {
                    BFS_Algorithm(row, column);
                }
                else
                {
                    BlockButtons[row, column].Content = MineMap[row, column].ToString();
                }
            }
            return false;
        }

        private static void BFS_Algorithm(int row, int column)
        {
            Queue<Position> buttons = new Queue<Position>();
            HashSet<Position> isSet = new HashSet<Position>();
            Position currentPosition = new Position(row, column);
            buttons.Enqueue(currentPosition);
            isSet.Add(currentPosition);
            Position tempPosition;
            while (buttons.Count > 0)
            {
                tempPosition = buttons.Dequeue();

                for (int i = 0; i < 8; ++i)
                {
                    Position nextPosition =
                        new Position(tempPosition.Row + Around[i, 0],
                        tempPosition.Column + Around[i, 1]);
                    if (IsInRange(nextPosition.Row, nextPosition.Column) &&
                        (!isSet.Contains(nextPosition)) &&
                        BlockButtons[nextPosition.Row, nextPosition.Column].Content == null)
                    {
                        isSet.Add(nextPosition);
                        if (MineMap[nextPosition.Row, nextPosition.Column] > 0)
                        {
                            BlockButtons[nextPosition.Row, nextPosition.Column].Content =
                                MineMap[nextPosition.Row, nextPosition.Column].ToString();
                            BlockButtons[nextPosition.Row, nextPosition.Column].IsEnabled = false;
                        }
                        if (MineMap[nextPosition.Row, nextPosition.Column] == 0)
                        {
                            BlockButtons[nextPosition.Row, nextPosition.Column].IsEnabled = false;
                            buttons.Enqueue(nextPosition);
                        }
                    }
                }
            }
        }

        private static bool IsInRange(int row, int column)
        {
            return row >= 0 && column >= 0 && row < Default.MapRow && column < Default.MapColumn;
        }

        private static void SetMine(int row, int column)
        {
            MineMap[row, column] = -1;
            int tempRow, tempColumn;

            for (int i = 0; i < 8; ++i)
            {
                tempRow = row + Around[i, 0];
                tempColumn = column + Around[i, 1];
                if (IsInRange(tempRow, tempColumn) && MineMap[tempRow, tempColumn] != -1)
                {
                    MineMap[tempRow, tempColumn]++;
                }
            }
        }
    }
}