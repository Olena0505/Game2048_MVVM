using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2048.ViewModels.Base;

namespace _2048.Models
{
    public class GameBoard : ViewModel
    {
        public readonly int boardSize = 4;
        public readonly int WinValue = 2048;

        public int[,] board;
        public int score;

        public int[,] Board
        {
            get => board; 
            set => SetField(ref board, value);
        }

        public int Score
        {
            get => score;
            set => SetField(ref score, value);
        }
    }
}
