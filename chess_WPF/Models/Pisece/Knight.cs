using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess_WPF.Models.Pisece
{
    internal class Knight : ChessPiece
    {
        public Knight(TeamSide team, int x, int y, Board board) : base(team, x, y, board, team is TeamSide.White ? "../../Resources/WpiseN.png" : "../../Resources/BpiseN.png") { }

        public override List<XY> GetAllMoves()
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
                    if ((board.board[ny][nx] == null || board.board[ny][nx].Team != Team))
                    {
                        steps.Add(new XY(nx, ny));
                    }
                }
            }

            return steps;
        }
    }
}
