using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess_WPF.Models.Pisece
{
    public abstract class ChessPiece
    {
        protected Board board;
        public TeamSide Team { get; set; }

        protected string imagePath;
        public string ImagePath
        {
            get
            {
                return imagePath;
            }
        }

        private XY XY;

        public XY xy
        {
            get { return XY; }
        }

        public ChessPiece(TeamSide team, int x, int y, Board board, string imagePath)
        {
            Team = team;
            XY = new XY(x, y);
            this.board = board;
            this.imagePath = imagePath;
        }

        public void move(int x, int y)
        {
            XY.X = x;
            XY.Y = y;
        }

        public abstract List<XY> allSteps();

        public List<XY> validMoves()
        {
            var validMoves = new List<XY>();
            foreach (var move in allSteps())
            {
                if (board.isValidMove(this, move))
                {
                    validMoves.Add(move);
                }
            }
            return validMoves;
        }

    }
}
