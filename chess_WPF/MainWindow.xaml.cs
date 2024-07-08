using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Converters;
using System.Windows.Media.Imaging;

namespace chess_WPF
{
    public partial class MainWindow : Window
    {
        private bool isWhiteTurn = true;
        private Grid grid = new Grid(); 
        private Board board = null;
        private Stack<Button_pi> stack = new Stack<Button_pi>();

        public MainWindow()
        {
            InitializeComponent();
            grid = mainGrid;
            board = new Board(mainGrid);
            board.init();
            board.print();
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button_pi button = sender as Button_pi;
            if (button != null)
            {
                string[] coordinates = button.Tag.ToString().Split(',');
                int row = int.Parse(coordinates[0]);
                int column = int.Parse(coordinates[1]);

                
                List<XY> allSteps;
                List<XY> steps;
                if (board.board[column][row] == null)
                {
                    allSteps = new List<XY> { };
                    if (stack.Count > 0)
                    {
                        steps = stack.Peek().pisece.allSteps();
                        if (button.pisece == null)
                        {
                            if (board.canMove(steps, new XY(row, column)))
                            {
                                stack.Peek().pisece.move(row, column);
                                board.clean(steps);
                                stack.Peek().Background = ((column + row) % 2 == 0) ? Brushes.LightGreen : Brushes.DimGray; 
                                stack.Pop();
                                if (board.isCheck(board.protecting))
                                {
                                    MessageBox.Show($"{((board.protecting.team == true) ? "Белым" : "Чёрным")} шах");
                                }
                                board.swap();
                                isWhiteTurn = !isWhiteTurn;
                            }
                            else
                            {
                                board.clean(steps);
                                stack.Peek().Background = ((column + row) % 2 == 0) ? Brushes.LightGreen : Brushes.DimGray;
                                stack.Pop();
                                //MessageBox.Show("Этот ход невозможен");
                                return;
                            }
                        }
                    }
                    else
                    {
                        //MessageBox.Show("выберите фигуру");
                        return;
                    }
                }
                else
                {
                    allSteps = board.board[column][row].allSteps();

                    if (stack.Count == 0)
                    {
                        if (!belonging(button.pisece))
                        {
                            return;
                        }
                        stack.Push(button);
                        board.printAllP(allSteps);
                        board.button_s[column][row].Background = Brushes.Blue;
                    }
                    else if (stack.Peek() == button)
                    {
                        board.clean(allSteps);
                        stack.Peek().Background = ((column + row) % 2 == 0) ? Brushes.LightGreen : Brushes.DimGray; 
                        stack.Pop();
                        return;
                    }
                    else
                    {
                        steps = stack.Peek().pisece.allSteps();
                        if (board.canMove(steps, new XY(row, column)))
                        {
                            board.kill(board.board[column][row]);
                            stack.Peek().pisece.move(row, column);
                            board.clean(steps);
                            stack.Peek().Background = ((column + row) % 2 == 0) ? Brushes.LightGreen : Brushes.DimGray; 
                            stack.Pop();
                            if (board.isCheck(board.protecting))
                            {
                                MessageBox.Show($"{((board.protecting.team == true) ? "Белым" : "Чёрным")} шах");
                            }
                            board.swap();
                            isWhiteTurn = !isWhiteTurn;
                        }
                    }
                }
                //board.swap();
                board.init();
                board.print();
                //isWhiteTurn = !isWhiteTurn;
                UpdateTurnInfo();
                //if (board.isCheck(board.protecting)) {
                //    MessageBox.Show($"{((board.protecting.team == true) ? "Белым" : "Чёрным")} шах");
                //}
            }
        }

        bool belonging(pisece pisece)
        {
            if (pisece.team == board.teamA()) { return true; }
            else {
                //MessageBox.Show("Это не ваша фигура");
                return false;
            }
        }

        private void UpdateTurnInfo()
        {
            if (isWhiteTurn)
            {
                TurnInfo.Text = "Ход белых";
            }
            else
            {
                TurnInfo.Text = "Ход черных";
            }
        }

        
    }
}
