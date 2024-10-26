using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using _2048.Commands;
using _2048.Data;
using _2048.Models;
using _2048.ViewModels.Base;

namespace _2048.ViewModels
{
    public class GameViewModel : ViewModel
    {
        private GameBoard gameBoard;
        private Random random;

        public int[,] Board
        {
            get => gameBoard.board;
            private set
            {
                SetField(ref gameBoard.board, value);
                OnPropertyChanged(nameof(Board));
            }
        }

        public int Score
        {
            get => gameBoard.score;
            private set => SetField(ref gameBoard.score, value);
        }

        public GameViewModel()
        {
            this.gameBoard = new GameBoard();
            this.random = new Random();

            Reset();
        }

        #region Commands

        public NavigationCommand NavigateToMenuPage { get => new(NavigateToPage, new Uri("Views/Pages/MenuPage.xaml", UriKind.RelativeOrAbsolute)); }
        public RelayCommand ShiftLeftCommand => new(ShiftLeft);     
        public RelayCommand ShiftRightCommand => new(ShiftRight);
        public RelayCommand ShiftDownCommand => new(ShiftDown);
        public RelayCommand ShiftUpCommand => new(ShiftUp);
        public RelayCommand ResetCommand => new(Reset);

        #endregion

        #region GameState

        private void CheckGameState()
        {

            Update();
            if (IsGameOver())
            {
                MessageBoxResult result = MessageBox.Show("Game over :( Do you want to note this?", "Finish",
                    MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes)
                {
                    AddToStatistics();
                }
                Reset();
            }

            else if (IsGameWin())
            {
                MessageBoxResult result = MessageBox.Show("You are winner!!! Do you want to note this?", "Finish",
                    MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes)
                {
                    AddToStatistics();
                }
                Reset();
            }
        }

        public bool IsGameWin()
        {
            for (int row = 0; row < gameBoard.boardSize; row++)
            {
                for (int column = 0; column < gameBoard.boardSize; column++)
                {
                    if (gameBoard.board[row, column] == gameBoard.WinValue)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool IsGameOver()
        {
            for (int row = 0; row < gameBoard.boardSize; row++)
            {
                for (int column = 0; column < gameBoard.boardSize; column++)
                {
                    if (gameBoard.board[row, column] == 0)
                    {
                        return false;
                    }
                }
            }

            for (int row = 0; row < gameBoard.boardSize; row++)
            {
                for (int column = 0; column < gameBoard.boardSize; column++)
                {
                    int value = gameBoard.board[row, column];
                    
                    if (row > 0 && gameBoard.board[row - 1, column] == value)
                    {
                        return false;
                    }
                    if (row < gameBoard.boardSize - 1 && gameBoard.board[row + 1, column] == value)
                    {
                        return false;
                    }
                    if (column > 0 && gameBoard.board[row, column - 1] == value)
                    {
                        return false;
                    }
                    if (column < gameBoard.boardSize - 1 && gameBoard.board[row, column + 1] == value)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        #endregion

        #region Statistics

        private void AddToStatistics()
        {
            string name;
            do
            {
                name = Microsoft.VisualBasic.Interaction.InputBox("Name: ", "Name input", "");
                if (string.IsNullOrEmpty(name))
                {
                    var result = MessageBox.Show("Invalid arg or Cancel pressed. Do you want to try again?", "Error", MessageBoxButton.YesNo, MessageBoxImage.Error);
                    if (result == MessageBoxResult.No)
                    {
                        return;
                    }
                }

            } while (string.IsNullOrEmpty(name));

            Statistics.Add(name, Score.ToString());
        }

        #endregion

        #region Operations

        private void GenerateRandomNumber()
        {
            int row, col;
            do
            {
                row = random.Next(gameBoard.boardSize);
                col = random.Next(gameBoard.boardSize);
            } while (gameBoard.board[row, col] != 0);

            gameBoard.board[row, col] = random.Next(100) < 90 ? 2 : 4;
        }

        private void Update()
        {
            Board = gameBoard.Board;
            Score = gameBoard.Score;
        }

        private void Reset()
        {
            Board = new int[gameBoard.boardSize, gameBoard.boardSize];
            Score = 0;
            GenerateRandomNumber();
            GenerateRandomNumber();
            Update();
        }

        #endregion

        #region Shifts

        public void ShiftLeft()
        {
            bool shifted = false;
            for (int i = 0; i < gameBoard.board.GetLength(0); i++)
            {
                int index = 0;
                for (int j = 0; j < gameBoard.board.GetLength(1); j++)
                {
                    if (gameBoard.board[i, j] != 0)
                    {
                        if (index > 0 && gameBoard.board[i, index - 1] == gameBoard.board[i, j])
                        {
                            gameBoard.board[i, index - 1] *= 2;
                            gameBoard.board[i, j] = 0;
                            shifted = true;
                            gameBoard.score += gameBoard.board[i, index - 1];
                        }
                        else
                        {
                            if (j != index)
                            {
                                gameBoard.board[i, index] = gameBoard.board[i, j];
                                gameBoard.board[i, j] = 0;
                                shifted = true;
                            }

                            index++;
                        }
                    }
                }
            }

            if (shifted)
            {
                GenerateRandomNumber();
                CheckGameState();
                OnPropertyChanged(nameof(Board));
                OnPropertyChanged(nameof(Score));

            }

        }

        public void ShiftRight()
        {
            bool shifted = false;
            for (int i = 0; i < gameBoard.board.GetLength(0); i++)
            {
                int index = gameBoard.board.GetLength(1) - 1;
                for (int j = gameBoard.board.GetLength(1) - 1; j >= 0; j--)
                {
                    if (gameBoard.board[i, j] != 0)
                    {
                        if (index < gameBoard.board.GetLength(1) - 1 && gameBoard.board[i, index + 1] == gameBoard.board[i, j])
                        {
                            gameBoard.board[i, index + 1] *= 2;
                            gameBoard.board[i, j] = 0;
                            shifted = true;
                            gameBoard.score += gameBoard.board[i, index + 1];
                        }
                        else
                        {
                            if (j != index)
                            {
                                gameBoard.board[i, index] = gameBoard.board[i, j];
                                gameBoard.board[i, j] = 0;
                                shifted = true;
                            }

                            index--;
                        }
                    }
                }
            }

            if (shifted)
            {
                GenerateRandomNumber();
                CheckGameState();
                OnPropertyChanged(nameof(Board));
                OnPropertyChanged(nameof(Score));
            }
        }

        public void ShiftDown()
        {
            bool shifted = false;
            for (int j = 0; j < gameBoard.board.GetLength(1); j++)
            {
                int index = gameBoard.board.GetLength(0) - 1;
                for (int i = gameBoard.board.GetLength(0) - 1; i >= 0; i--)
                {
                    if (gameBoard.board[i, j] != 0)
                    {
                        if (index < gameBoard.board.GetLength(0) - 1 && gameBoard.board[index + 1, j] == gameBoard.board[i, j])
                        {
                            gameBoard.board[index + 1, j] *= 2;
                            gameBoard.board[i, j] = 0;
                            shifted = true;
                            gameBoard.score += gameBoard.board[index + 1, j];
                        }
                        else
                        {
                            if (i != index)
                            {
                                gameBoard.board[index, j] = gameBoard.board[i, j];
                                gameBoard.board[i, j] = 0;
                                shifted = true;
                            }

                            index--;
                        }
                    }
                }
            }

            if (shifted)
            {
                GenerateRandomNumber();
                CheckGameState();
                OnPropertyChanged(nameof(Board));
                OnPropertyChanged(nameof(Score));
            }
        }

        public void ShiftUp()
        {
            bool shifted = false;
            for (int j = 0; j < gameBoard.board.GetLength(1); j++)
            {
                int index = 0;
                for (int i = 0; i < gameBoard.board.GetLength(0); i++)
                {
                    if (gameBoard.board[i, j] != 0)
                    {
                        if (index > 0 && gameBoard.board[index - 1, j] == gameBoard.board[i, j])
                        {
                            gameBoard.board[index - 1, j] *= 2;
                            gameBoard.board[i, j] = 0;
                            shifted = true;
                            gameBoard.score += gameBoard.board[index - 1, j];
                        }
                        else
                        {
                            if (i != index)
                            {
                                gameBoard.board[index, j] = gameBoard.board[i, j];
                                gameBoard.board[i, j] = 0;
                                shifted = true;
                            }

                            index++;
                        }
                    }
                }
            }

            if (shifted)
            {
                GenerateRandomNumber();
                CheckGameState();
                OnPropertyChanged(nameof(Board));
                OnPropertyChanged(nameof(Score));
            }
        }

        #endregion

    }
}
