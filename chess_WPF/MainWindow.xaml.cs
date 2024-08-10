using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace chess_WPF
{
    public partial class MainWindow : Window
    {
        private bool isWhiteTurn = true;
        private Grid grid;
        private Board board;
        private Stack<Button_pi> stack = new Stack<Button_pi>();

        public MainWindow()
        {
            InitializeComponent();
            grid = mainGrid;
            board = new Board(mainGrid);
            board.init();
            board.print();
            UpdateTurnInfo();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button_pi button = sender as Button_pi;
            if (button == null) return;

            string[] coordinates = button.Tag.ToString().Split(',');
            int row = int.Parse(coordinates[0]);
            int column = int.Parse(coordinates[1]);

            if (board.board[column][row] == null)
            {
                HandleEmptyButtonClick(row, column);
            }
            else
            {
                HandleOccupiedButtonClick(button, row, column);
            }

            board.init();
            board.print();
            UpdateTurnInfo();
        }

        private void HandleEmptyButtonClick(int row, int column)
        {
            if (stack.Count > 0)
            {
                List<XY> steps = stack.Peek().pisece.allSteps();
                if (board.canMove(steps, new XY(row, column)))
                {
                    steps.Add(stack.Peek().pisece.xy); 
                    board.clean(steps); 
                    stack.Peek().pisece.move(row, column); 
                    stack.Pop();
                    CheckForCheck();
                    board.swap();
                    isWhiteTurn = !isWhiteTurn;
                }
                else
                {
                    steps.Add(stack.Peek().pisece.xy); 
                    board.clean(steps);  
                    stack.Pop();
                }
            }
        }

        private void HandleOccupiedButtonClick(Button_pi button, int row, int column)
        {
            List<XY> allSteps = board.board[column][row].allSteps();

            if (stack.Count == 0)
            {
                if (belonging(button.pisece))
                {
                    stack.Push(button);
                    board.printAllP(allSteps, column, row);  
                }
            }
            else if (stack.Peek() == button)
            {
                stack.Pop();
                allSteps.Add(new XY(row, column));
                board.clean(allSteps);  
            }
            else
            {
                List<XY> steps = stack.Peek().pisece.allSteps();
                if (board.canMove(steps, new XY(row, column)))
                {
                    steps.Add(stack.Peek().pisece.xy);  
                    board.clean(steps);  
                    board.kill(board.board[column][row]); 
                    stack.Peek().pisece.move(row, column); 
                    stack.Pop();
                    CheckForCheck();
                    board.swap();
                    isWhiteTurn = !isWhiteTurn;
                }
            }
        }

        private bool belonging(pisece pisece)
        {
            return pisece.team == board.teamA();
        }

        private void CheckForCheck()
        {
            if (board.isCheck(board.protecting))
            {
                MessageBox.Show($"{(board.protecting.team ? "Белым" : "Чёрным")} шах");
            }
        }

        private void UpdateTurnInfo()
        {
            TurnInfo.Text = isWhiteTurn ? "Ход белых" : "Ход черных";
        }
    }
}
