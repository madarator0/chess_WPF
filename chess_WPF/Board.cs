using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace chess_WPF
{
    class Board
    {
        private Player player1;
        private Player player2;
        private Player attacking;
        private Player protecting;

        public pisece[][] board { get; private set; }
        public Button_pi[][] button_s { get; private set; }

        public Board(Grid grid)
        {
            player1 = new Player(this, pisece.W);
            player2 = new Player(this, pisece.B);
            board = new pisece[8][];
            for (int i = 0; i < 8; i++)
            {
                board[i] = new pisece[8];
            }
            attacking = player1;
            protecting = player2;
            button_s = new Button_pi[8][];
            for (int i = 0; i < 8; i++)
            {
                button_s[i] = new Button_pi[8];
            }
            int i1 = 0, j1 = 0;
            foreach(var ei in grid.Children)
            {
                if (j1 == 8)
                {
                    i1++;
                    j1 = 0;
                }
                if (ei is Button_pi button)
                {
                    button_s[j1][i1] = button;
                }
                if ((i1 == 7) && (j1 == 7))
                {
                    break;
                }
                j1++;
            }
            initializePiseces();
        }

        private void initializePiseces()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    board[i][j] = null;
                }
            }

            foreach (pisece p in player1.piseces)
            {
                board[p.xy.Y][p.xy.X] = p;
            }

            foreach (pisece p in player2.piseces)
            {
                board[p.xy.Y][p.xy.X] = p;
            }
        }

        public void init()
        {
            initializePiseces();
            for (int i = 0;i < 8;i++)
            {
                for(int j = 0;j < 8;j++)
                {
                    button_s[i][j].pisece = board[i][j]; 
                }
            }
        }

        public void printAllP(List<XY> allSteps)
        {
            for (int i = 0; i < allSteps.Count; i++)
            {
                if ((board[allSteps[i].Y][allSteps[i].X]?.symbol ?? " ") != "K")
                {
                    button_s[allSteps[i].Y][allSteps[i].X].Background = Brushes.Green;
                }                   
            }
        }

        public void clean(List<XY> allSteps)
        {
            for (int i = 0; i < allSteps.Count; i++)
            {
                button_s[allSteps[i].Y][allSteps[i].X].Background = ((allSteps[i].Y+ allSteps[i].X)%2==0) ? Brushes.LightGreen : Brushes.DimGray;
            }
        }

        public bool canMove(List<XY> allSteps, XY xy)
        {
            pisece tmp = board[xy.Y][xy.Y];
            string res = (tmp == null) ? " " : tmp.symbol; 
            if (res != "K")
            {
                return allSteps.Contains(xy);
            }
            return false;
        }

        private bool isCheck(Player player)
        {
            var king = player.piseces.Find(p => p is King);
            foreach (var p in attacking.piseces)
            {
                if (p.allSteps().Contains(king.xy))
                {
                    return true;
                }
            }
            return false;
        }

        private bool isCheckmate(Player player)
        {
            foreach (var p in player.piseces)
            {
                if (p.validMoves().Count > 0)
                {
                    return false;
                }
            }
            return true;
        }

        public bool isValidMove(pisece piece, XY move)
        {
            var originalPosition = piece.xy;
            var targetPiece = board[move.Y][move.X];

            board[move.Y][move.X] = piece;
            board[originalPosition.Y][originalPosition.X] = null;
            piece.move(move.X, move.Y);

            bool valid = !isCheck(protecting);

            board[move.Y][move.X] = targetPiece;
            board[originalPosition.Y][originalPosition.X] = piece;
            piece.move(originalPosition.X, originalPosition.Y);

            return valid;
        }


        //private void round()
        //{
        //    string team = (attacking.team == true) ? "Белых" : "Чёрных";
        //    Console.WriteLine($"Ход {team}");
        //    pisece Pisece = attacking.choose();
        //    List<XY> allSteps = Pisece.allSteps();
        //    printAllP(allSteps);
        //    XY xy;
        //    do
        //    {
        //        xy = Program.getValidCoordinates();
        //    } while (!canMove(allSteps, xy));

        //    if (board[xy.Y][xy.X] == null)
        //    {
        //        Pisece.move(xy.X, xy.Y);
        //    }
        //    else
        //    {
        //        protecting.piseces.Remove(board[xy.Y][xy.X]);
        //        Pisece.move(xy.X, xy.Y);
        //    }
        //    initializePiseces();

        //}
        public void kill(pisece pisece)
        {
            protecting.piseces.Remove(pisece);
        }

        public bool teamA()
        {
            return attacking.team;
        }

        public void swap()
        {
            Player tmp = attacking;
            attacking = protecting;
            protecting = tmp;
        }

        public void print()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board[i][j] != null)
                    {
                        button_s[i][j].FontSize = 24;
                        button_s[i][j].Foreground = (board[i][j].team) ? Brushes.White : Brushes.Black; 
                        button_s[i][j].Content = board[i][j].symbol;
                    }
                    else
                    {
                        button_s[i][j].Content = "";
                    }
                }
            }
        }

        //public void Play()
        //{
        //    print();
        //    while (true)
        //    {
        //        round();
        //        print();
        //    }
        //}
    }
}
