using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess_WPF
{
    internal class XY
    {
        public int X { get; set; }
        public int Y { get; set; }

        public XY(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj is XY)
            {
                XY xy = (XY)obj;
                return X == xy.X && Y == xy.Y;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }
    }
}
