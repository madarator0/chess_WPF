using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess_WPF.Models.Pisece
{
    internal class Queen : ChessPiece
    {
        public Queen(TeamSide team, int x, int y, Board board) : base(team, x, y, board, team is TeamSide.White ? "../../Resources/WpiceQ.png" : "../../Resources/BpiceQ.png") { }

        public override List<XY> GetAllMoves()
        {
            List<XY> steps = new List<XY>();

            // Combine rook and bishop moves
            Rook rook = new Rook(Team, xy.X, xy.Y, board);
            Bishop bishop = new Bishop(Team, xy.X, xy.Y, board);

            steps.AddRange(rook.GetAllMoves());
            steps.AddRange(bishop.GetAllMoves());

            return steps;
        }
    }

}
