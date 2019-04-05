using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using static GardeningWithMines.Managers.ControlsManager;
using static GardeningWithMines.Managers.GameDataManager;
using static GardeningWithMines.Properties.Settings;

namespace GardeningWithMines.Managers
{
    //Define position
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
        //Handle 'click event'
        public static void Click(int row, int column)
        {
            if (BlockButtons[row, column].Content == null)
            {
                BlockButtons[row, column].IsEnabled = false;

                //For mine
                if (CurrentGameData.MinesMap[row, column] == -1)
                {
                    BlockButtons[row, column].FontFamily = new FontFamily(Default.IconFontFamily);
                    BlockButtons[row, column].Content = Default.MineCharacter;
                    BlockButtons[row, column].SetBinding(Button.FontSizeProperty, IconFontSizeBinding);
                    ++CurrentGameData.SteppedCount;
                }

                //For 'plain'
                else if (CurrentGameData.MinesMap[row, column] == 0)
                {
                    BFS_Algorithm(row, column);
                }

                //For 'hill'
                else
                {
                    BlockButtons[row, column].Content = CurrentGameData.MinesMap[row, column].ToString();
                    --CurrentGameData.UnclickedSafeBlockCount;
                }
            }
        }

        //Magic. Don't touch
        private static void BFS_Algorithm(int row, int column)
        {
            //Init
            Queue<Position> buttons = new Queue<Position>();
            HashSet<Position> isSet = new HashSet<Position>();
            Position currentPosition = new Position(row, column);
            Position tempPosition;

            //Enqueue the 'fuse'
            buttons.Enqueue(currentPosition);
            isSet.Add(currentPosition);
            --CurrentGameData.UnclickedSafeBlockCount;

            //Chain reaction ( flood-fill algorithm )
            while (buttons.Count > 0)
            {
                tempPosition = buttons.Dequeue();

                //Search around
                for (int i = 0; i < 8; ++i)
                {
                    Position nextPosition =
                        new Position(tempPosition.Row + Around[i, 0],
                        tempPosition.Column + Around[i, 1]);

                    //Catch the next 'unlucky'
                    if (IsInRange(nextPosition.Row, nextPosition.Column) &&
                        (!isSet.Contains(nextPosition)) &&
                        BlockButtons[nextPosition.Row, nextPosition.Column].Content == null)
                    {
                        isSet.Add(nextPosition);

                        //Found 'hill'
                        if (CurrentGameData.MinesMap[nextPosition.Row, nextPosition.Column] > 0)
                        {
                            BlockButtons[nextPosition.Row, nextPosition.Column].Content =
                                CurrentGameData.MinesMap[nextPosition.Row, nextPosition.Column].ToString();
                            BlockButtons[nextPosition.Row, nextPosition.Column].IsEnabled = false;
                            --CurrentGameData.UnclickedSafeBlockCount;
                        }

                        //Found 'plain'
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