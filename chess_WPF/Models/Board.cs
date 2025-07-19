using chess_WPF.Models.Pisece;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace chess_WPF.Models
{
    public class Board
    {
        private Player player1;
        private Player player2;
        public Player attacking { get; private set; }
        public Player protecting { get; private set; }

        public ChessPiece[][] board { get; private set; }

        public Board()
        {
            player1 = new Player(this, TeamSide.White);
            player2 = new Player(this, TeamSide.Black);
            board = new ChessPiece[8][];
            for (int i = 0; i < 8; i++)
            {
                board[i] = new ChessPiece[8];
            }
            attacking = player1;
            protecting = player2;
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

            foreach (ChessPiece p in player1.piseces)
            {
                board[p.xy.Y][p.xy.X] = p;
            }

            foreach (ChessPiece p in player2.piseces)
            {
                board[p.xy.Y][p.xy.X] = p;
            }
        }

        public bool canMove(List<XY> allSteps, XY xy)
        {
            return allSteps.Contains(xy);
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

        public bool isValidMove(ChessPiece piece, XY move)
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

        public void kill(ChessPiece pisece)
        {
            if (pisece != null)
            {
                var pos = pisece.xy;
                if (pos.Y >= 0 && pos.Y < 8 && pos.X >= 0 && pos.X < 8)
                {
                    board[pos.Y][pos.X] = null;
                    protecting.piseces.Remove(pisece);
                }
            }
        }

        public TeamSide teamA()
        {
            return attacking.Team;
        }

        public void swap()
        {
            Player tmp = attacking;
            attacking = protecting;
            protecting = tmp;
        }

        public void SwapPieces(int x1, int y1, int x2, int y2)
        {
            var piece1 = board[y1][x1];
            var piece2 = board[y2][x2];

            board[y1][x1] = piece2;
            board[y2][x2] = piece1;

            // Обновляем координаты фигур, если они не null
            if (piece1 != null)
                piece1.move(x2, y2);
            if (piece2 != null)
                piece2.move(x1, y1);
        }
    }
}
