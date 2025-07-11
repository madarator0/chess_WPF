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
        public string symbol
        {
            get
            {
                return Symbol;
            }
        }

        private XY XY;

        public XY xy
        {
            get { return XY; }
        }

        public ChessPiece(bool team, string symbol, int x, int y, Board board)
        {
            this.team = team;
            Symbol = symbol;
            XY = new XY(x, y);
            this.board = board;
        }

        public void move(int x, int y )
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

        public virtual string ImagePath
        {
            get
            {
                if (board == null) return null;
                var boardType = team ? "W" : "B";
                string symbolKey = symbol;
                var dict = team
                    ? board.GetType().GetField("Wpice", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(board) as Dictionary<string, string>
                    : board.GetType().GetField("Bpice", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(board) as Dictionary<string, string>;
                if (dict != null && dict.ContainsKey(symbolKey))
                    return dict[symbolKey];
                return null;
            }
        }
    }
}
