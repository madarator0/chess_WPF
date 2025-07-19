using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess_WPF.Models.Pisece
{
    internal class Pawn : ChessPiece
    {
        public Pawn(TeamSide team, int x, int y, Board board) : base(team, x, y, board, team is TeamSide.White ? "../../Resources/WpiceP.png" : "../../Resources/BpiceP.png") { }

        public override List<XY> allSteps()
        {
            List<XY> steps = new List<XY>();
            int direction = Team is TeamSide.White ? 1 : -1;

            // Move forward
            if (isValidMove(xy.X, xy.Y + direction))
            {
                steps.Add(new XY(xy.X, xy.Y + direction));

                // Move two squares forward from starting position
                if ((Team is TeamSide.White && xy.Y == 1) || (Team is TeamSide.Black && xy.Y == 6))
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
                if (board.board[xy.Y + direction][xy.X - 1].Team != Team)
                {
                    steps.Add(new XY(xy.X - 1, xy.Y + direction));
                }
            }

            // Capture diagonally to the right
            if (xy.X < 7 && !Program.isOut(xy.X + 1, xy.Y + direction) && board.board[xy.Y + direction][xy.X + 1] != null)
            {
                if (board.board[xy.Y + direction][xy.X + 1].Team != Team)
                {
                    steps.Add(new XY(xy.X + 1, xy.Y + direction));
                }
            }

            return steps;
        }

        private bool isValidMove(int x, int y)
        {
            // Ensure move is within _board bounds and target square is empty
            return x >= 0 && x < 8 && y >= 0 && y < 8 && board.board[y][x] == null;
        }
    }
}
