using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess_WPF
{
    internal class Knight : pisece
    {
        public Knight(bool team, int x, int y, Board board) : base(team, "N", x, y, board) { }

        public override List<XY> allSteps()
        {
            List<XY> steps = new List<XY>();

            // Knight moves
            int[] dx = { 1, 2, 2, 1, -1, -2, -2, -1 };
            int[] dy = { 2, 1, -1, -2, -2, -1, 1, 2 };

            for (int i = 0; i < 8; i++)
            {
                int nx = xy.X + dx[i];
                int ny = xy.Y + dy[i];
                if (!Program.isOut(nx, ny))
                {
                    if ((board.board[ny][nx] == null || board.board[ny][nx].team != team))
                    {
                        steps.Add(new XY(nx, ny));
                    }
                }
            }

            return steps;
        }
    }
}
