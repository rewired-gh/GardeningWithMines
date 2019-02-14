﻿using System.Windows.Controls;
using MaterialDesignThemes.Wpf;

namespace GardeningWithMines.Models
{
    internal class IntelliButton : Button
    {
        public IntelliButton(int i, int j) : base()
        {
            Row = i;
            Column = j;
        }

        public int Column { get; set; }
        public int Row { get; set; }
    }
}