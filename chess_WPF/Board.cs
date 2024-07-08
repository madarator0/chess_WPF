using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace chess_WPF
{
    class Board
    {
        private Player player1;
        private Player player2;
        public Player attacking       { get; private set; }
        public Player protecting      { get; private set; }

        public pisece[][] board       { get; private set; }
        public Button_pi[][] button_s { get; private set; }

        private Dictionary<string, string> Bpice = new Dictionary<string, string>();
        private Dictionary<string, string> Wpice = new Dictionary<string, string>();

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
            Bpice.Add("K", "C:\\Users\\user\\source\\repos\\chess_WPF\\chess_WPF\\resours\\BpiceK.png");
            Bpice.Add("Q", "C:\\Users\\user\\source\\repos\\chess_WPF\\chess_WPF\\resours\\BpiceQ.png");
            Bpice.Add("B", "C:\\Users\\user\\source\\repos\\chess_WPF\\chess_WPF\\resours\\BpiceB.png");
            Bpice.Add("R", "C:\\Users\\user\\source\\repos\\chess_WPF\\chess_WPF\\resours\\BpiceR.png");
            Bpice.Add("N", "C:\\Users\\user\\source\\repos\\chess_WPF\\chess_WPF\\resours\\BpiseN.png");
            Bpice.Add("P", "C:\\Users\\user\\source\\repos\\chess_WPF\\chess_WPF\\resours\\BpiceP.png");

            Wpice.Add("K", "C:\\Users\\user\\source\\repos\\chess_WPF\\chess_WPF\\resours\\WpiceK.png");
            Wpice.Add("Q", "C:\\Users\\user\\source\\repos\\chess_WPF\\chess_WPF\\resours\\WpiceQ.png");
            Wpice.Add("B", "C:\\Users\\user\\source\\repos\\chess_WPF\\chess_WPF\\resours\\WpiceB.png");
            Wpice.Add("R", "C:\\Users\\user\\source\\repos\\chess_WPF\\chess_WPF\\resours\\WpiceR.png");
            Wpice.Add("N", "C:\\Users\\user\\source\\repos\\chess_WPF\\chess_WPF\\resours\\WpiseN.png");
            Wpice.Add("P", "C:\\Users\\user\\source\\repos\\chess_WPF\\chess_WPF\\resours\\WpiceP.png");
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

        public bool isCheck(Player player)
        {
            var king = player.piseces.Find(p1 => p1 is King);
            foreach (var p in attacking.piseces)
            {
                if (p.allSteps().Contains(king.xy))
                {
                    return true;
                }
            }
            return false;
        }

        public bool isCheckmate(Player player)
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

                        // Создаем новое изображение для каждой кнопки
                        Image myImage = new Image
                        {
                            Source = new BitmapImage(new Uri((board[i][j].team) ? Wpice[board[i][j].symbol] : Bpice[board[i][j].symbol])),
                            Stretch = Stretch.Uniform
                        };

                        button_s[i][j].Content = myImage;
                    }
                    else
                    {
                        button_s[i][j].Content = "";
                    }
                }
            }
        }
    }
}
