using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess_WPF
{
    internal class Pawn : pisece
    {
        public Pawn(bool team, int x, int y, Board board) : base(team, "P", x, y, board) { }

        public override List<XY> allSteps()
        {
            List<XY> steps = new List<XY>();
            int direction = team ? 1 : -1;

            // Move forward
            if (isValidMove(xy.X, xy.Y + direction))
            {
                steps.Add(new XY(xy.X, xy.Y + direction));

                // Move two squares forward from starting position
                if ((team && xy.Y == 1) || (!team && xy.Y == 6))
                {
                    if (isValidMove(xy.X, xy.Y + 2 * direction))
                    {
                        steps.Add(new XY(xy.X, xy.Y + 2 * direction));
                    }
                }
            }

            // Capture diagonally to the left
            if (xy.X > 0 && !Program.isOut(xy.X - 1, xy.Y + direction) && board.board[xy.Y + direction][xy.X - 1] != null)
            {
                if (board.board[xy.Y + direction][xy.X - 1].team != team)
                {
                    steps.Add(new XY(xy.X - 1, xy.Y + direction));
                }
            }

            // Capture diagonally to the right
            if (xy.X < 7 && !Program.isOut(xy.X + 1, xy.Y + direction) && board.board[xy.Y + direction][xy.X + 1] != null)
            {
                if (board.board[xy.Y + direction][xy.X + 1].team != team)
                {
                    steps.Add(new XY(xy.X + 1, xy.Y + direction));
                }
            }

            return steps;
        }

        private bool isValidMove(int x, int y)
        {
            // Ensure move is within board bounds and target square is empty
            return x >= 0 && x < 8 && y >= 0 && y < 8 && board.board[y][x] == null;
        }
    }
}
