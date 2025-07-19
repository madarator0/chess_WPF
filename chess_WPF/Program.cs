using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess_WPF
{
    class Program
    {
        public static bool isOut(int x,  int y)
        {
            return x < 0 || x > 7 || y < 0 || y > 7;
        }
    }
}
