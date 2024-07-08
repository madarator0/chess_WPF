using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess_WPF
{
    internal class Bishop : pisece
    {
        public Bishop(bool team, int x, int y, Board board) : base(team, "B", x, y, board) { }

        public override List<XY> allSteps()
        {
            List<XY> steps = new List<XY>();

            // Move diagonally
            for (int dx = -1; dx <= 1; dx += 2)
            {
                for (int dy = -1; dy <= 1; dy += 2)
                {
                    for (int i = 1; i < 8; i++)
                    {
                        int nx = xy.X + dx * i;
                        int ny = xy.Y + dy * i;

                        // Check if the new position is within the board bounds and a valid move
                        if (isValidMove(nx, ny))
                        {
                            steps.Add(new XY(nx, ny));

                            // If the new position is occupied by a piece, stop the diagonal movement in this direction
                            if (board.board[ny][nx] != null)
                                break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            return steps;
        }

        private bool isValidMove(int x, int y)
        {
            // Ensure move is within board bounds and is not occupied by a piece of the same team
            return x >= 0 && x < 8 && y >= 0 && y < 8 && (board.board[y][x] == null || board.board[y][x].team != team);
        }
    }

}
