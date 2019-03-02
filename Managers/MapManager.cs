using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using static GardeningWithMines.Managers.ControlsManager;
using static GardeningWithMines.Managers.GameDataManager;
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
        static MapManager()
        {
        }

        public static void Click(int row, int column)
        {
            if (BlockButtons[row, column].Content == null)
            {
                BlockButtons[row, column].IsEnabled = false;
                if (CurrentGameData.MinesMap[row, column] == -1)
                {
                    BlockButtons[row, column].FontFamily = new FontFamily(Default.IconFontFamily);
                    BlockButtons[row, column].Content = Default.MineCharacter;
                    BlockButtons[row, column].SetBinding(Button.FontSizeProperty, IconFontSizeBinding);
                    ++CurrentGameData.SteppedCount;
                }
                else if (CurrentGameData.MinesMap[row, column] == 0)
                {
                    BFS_Algorithm(row, column);
                }
                else
                {
                    BlockButtons[row, column].Content = CurrentGameData.MinesMap[row, column].ToString();
                    --CurrentGameData.UnclickedSafeBlockCount;
                }
            }
        }

        private static void BFS_Algorithm(int row, int column)
        {
            Queue<Position> buttons = new Queue<Position>();
            HashSet<Position> isSet = new HashSet<Position>();
            Position currentPosition = new Position(row, column);
            buttons.Enqueue(currentPosition);
            isSet.Add(currentPosition);
            --CurrentGameData.UnclickedSafeBlockCount;
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
                        if (CurrentGameData.MinesMap[nextPosition.Row, nextPosition.Column] > 0)
                        {
                            BlockButtons[nextPosition.Row, nextPosition.Column].Content =
                                CurrentGameData.MinesMap[nextPosition.Row, nextPosition.Column].ToString();
                            BlockButtons[nextPosition.Row, nextPosition.Column].IsEnabled = false;
                            --CurrentGameData.UnclickedSafeBlockCount;
                        }
                        else if (CurrentGameData.MinesMap[nextPosition.Row, nextPosition.Column] == 0)
                        {
                            BlockButtons[nextPosition.Row, nextPosition.Column].IsEnabled = false;
                            buttons.Enqueue(nextPosition);
                            --CurrentGameData.UnclickedSafeBlockCount;
                        }
                    }
                }
            }
        }
    }
}