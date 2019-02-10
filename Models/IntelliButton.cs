using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace GardeningWithMines.Models
{
    class IntelliButton : Button
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public IntelliButton(int i, int j) : base()
        {
            Row = i;
            Column = j;
        }
    }
}
