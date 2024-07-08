using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess_WPF
{
    internal class Queen : pisece
    {
        public Queen(bool team, int x, int y, Board board) : base(team, "Q", x, y, board) { }

        public override List<XY> allSteps()
        {
            List<XY> steps = new List<XY>();

            // Combine rook and bishop moves
            Rook rook = new Rook(team, xy.X, xy.Y, board);
            Bishop bishop = new Bishop(team, xy.X, xy.Y, board);

            steps.AddRange(rook.allSteps());
            steps.AddRange(bishop.allSteps());

            return steps;
        }
    }

}
