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

        public static XY getValidCoordinates()
        {
            int x = -1, y = -1;
            bool valid = false;

            while (!valid)
            {
                Console.Write("Enter your move (e.g., e2): ");
                string input = Console.ReadLine().ToLower();

                if (input.Length == 2 && input[0] >= 'a' && input[0] <= 'h' && char.IsDigit(input[1]))
                {
                    x = input[0] - 'a';
                    y = int.Parse(input[1].ToString()) - 1; // Adjust to zero-indexed
                    if (!isOut(x, y))
                    {
                        valid = true;
                    }
                    else
                    {
                        Console.WriteLine("Coordinates out of bounds. Please enter again.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter again.");
                }
            }

            return new XY(x, y);
        }
    }
}
