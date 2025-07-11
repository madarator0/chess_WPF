using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess_WPF.Models.Pisece
{
    public abstract class ChessPiece
    {
        public const bool W = true;
        public const bool B = false;

        protected Board board;
        public bool team { get; set; }

        protected string Symbol;

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

        public ChessPiece(bool team, string symbol, int x, int y, Board board, string imagePath)
        {
            this.team = team;
            Symbol = symbol;
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
