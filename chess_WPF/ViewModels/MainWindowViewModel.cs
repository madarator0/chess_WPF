using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using chess_WPF.Models;
using chess_WPF.Models.Pisece;

namespace chess_WPF.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private bool isWhiteTurn = true;
        private Board board;
        private Stack<CellViewModel> stack = new Stack<CellViewModel>();
        private string turnInfo;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<ObservableCollection<CellViewModel>> BoardCells { get; }

        public ICommand CellClickCommand { get; }

        public string TurnInfo
        {
            get => turnInfo;
            set
            {
                if (turnInfo != value)
                {
                    turnInfo = value;
                    OnPropertyChanged(nameof(TurnInfo));
                }
            }
        }

        public MainWindowViewModel()
        {
            board = new Board();
            BoardCells = new ObservableCollection<ObservableCollection<CellViewModel>>();
            CellClickCommand = new RelayCommand(param => OnCellClick(param as CellViewModel));
            for (int y = 0; y < 8; y++)
            {
                var row = new ObservableCollection<CellViewModel>();
                for (int x = 0; x < 8; x++)
                {
                    row.Add(new CellViewModel(x, y, CellClickCommand) { Piece = board.board[y][x] });
                }
                BoardCells.Add(row);
            }
            UpdateTurnInfo();
        }

        private void OnCellClick(CellViewModel cell)
        {
            if (cell == null) return;

            int row = cell.X;
            int column = cell.Y;

            if (board.board[column][row] == null)
            {
                HandleEmptyButtonClick(row, column);
            }
            else
            {
                HandleOccupiedButtonClick(cell, row, column);
            }

            UpdateBoardCells();
            UpdateTurnInfo();
        }

        private void HandleEmptyButtonClick(int row, int column)
        {
            if (stack.Count > 0)
            {
                var selectedCell = stack.Peek();
                if (board.IsValidStep(selectedCell.Piece, new XY(row, column)))
                {
                    // Подсветку сбрасываем
                    ClearHighlights();
                    // Вместо selectedCell.Piece.move(row, column);
                    board.SwapPieces(selectedCell.Piece.xy.X, selectedCell.Piece.xy.Y, row, column);
                    stack.Pop();
                    board.SwitchPlayers();
                    isWhiteTurn = !isWhiteTurn;
                }
                else
                {
                    ClearHighlights();
                    stack.Pop();
                }
            }
        }

        private void HandleOccupiedButtonClick(CellViewModel cell, int row, int column)
        {
            List<XY> allSteps = board.board[column][row].GetAllMoves();
            var king = board.protecting.piseces.Find(p1 => p1 is King);
            allSteps.Remove(king.xy);
            if (stack.Count == 0)
            {
                if (IsAttackingTeamPiece(cell.Piece))
                {
                    stack.Push(cell);
                    allSteps.Add(cell.Piece.xy);
                    HighlightPossibleMoves(allSteps);
                }
            }
            else if (stack.Peek() == cell)
            {
                stack.Pop();
                ClearHighlights();
            }
            else
            {
                var selectedCell = stack.Peek();
                if (board.IsValidStep(selectedCell.Piece, new XY(row, column)))
                {
                    ClearHighlights();
                    board.kill(board.board[column][row]);
                    // Используем SwapPieces для перемещения и взятия
                    board.SwapPieces(selectedCell.Piece.xy.X, selectedCell.Piece.xy.Y, row, column);
                    stack.Pop();
                    CheckForCheck();
                    board.SwitchPlayers();
                    isWhiteTurn = !isWhiteTurn;
                }
            }
        }

        private bool IsAttackingTeamPiece(ChessPiece pisece)
        {
            return pisece.Team == board.GetAttackingTeam();
        }

        private void CheckForCheck()
        {
            if (board.isCheck(board.protecting))
            {
                // Здесь можно реализовать уведомление через событие или свойство
                // Например, Message = $"{(_board.protecting.team ? "Белым" : "Чёрным")} шах";
            }
        }

        private void HighlightPossibleMoves(List<XY> moves)
        {
            ClearHighlights();
            foreach (var move in moves)
            {
                BoardCells[move.Y][move.X].Background = "Green";
            }
        }

        private void ClearHighlights()
        {
            for (int y = 0; y < 8; y++)
                for (int x = 0; x < 8; x++)
                {
                    var cell = BoardCells[y][x];
                    cell.Background = ((cell.X + cell.Y) % 2 == 0) ? "LightGreen" : "DimGray";
                }
        }

        private void UpdateBoardCells()
        {
            for (int y = 0; y < 8; y++)
                for (int x = 0; x < 8; x++)
                    BoardCells[y][x].Piece = board.board[y][x];
        }

        private void UpdateTurnInfo()
        {
            TurnInfo = isWhiteTurn ? "Ход белых" : "Ход черных";
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}