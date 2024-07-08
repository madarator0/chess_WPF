using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess_WPF
{
    internal class Player
    {
        private Board board;
        public bool team { get; }
        public List<pisece> piseces { get; } = new List<pisece>();

        public Player(Board board, bool team)
        {
            this.board = board;
            this.team = team;
            if (team)
            {
                piseces.Add(new King(team, 4, 0, board));
                piseces.Add(new Queen(team, 3, 0, board));
                piseces.Add(new Rook(team, 0, 0, board));
                piseces.Add(new Rook(team, 7, 0, board));
                piseces.Add(new Bishop(team, 2, 0, board));
                piseces.Add(new Bishop(team, 5, 0, board));
                piseces.Add(new Knight(team, 1, 0, board));
                piseces.Add(new Knight(team, 6, 0, board));
                for (int i = 0; i < 8; i++)
                {
                    piseces.Add(new Pawn(team, i, 1, board));
                }
            } 
            else
            {
                piseces.Add(new King(team, 4, 7, board));
                piseces.Add(new Queen(team, 3, 7, board));
                piseces.Add(new Rook(team, 0, 7, board));
                piseces.Add(new Rook(team, 7, 7, board));
                piseces.Add(new Bishop(team, 2, 7, board));
                piseces.Add(new Bishop(team, 5, 7, board));
                piseces.Add(new Knight(team, 1, 7, board));
                piseces.Add(new Knight(team, 6, 7, board));
                for (int i = 0; i < 8; i++)
                {
                    piseces.Add(new Pawn(team, i, 6, board));
                }
            }
        }
        public pisece choose()
        {
            pisece res;
            Console.WriteLine("Выберите фигуру (e5)");
            while (true) 
            {
                XY tmp = Program.getValidCoordinates();
                if (!Program.isOut(tmp.X, tmp.Y)) 
                {
                    res = board.board[tmp.Y][tmp.X];
                    if (res != null)
                    {
                        if (res.team == team)
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("это не ваша фигура");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("клетка пуста");
                }
            } 
            return res;
        }
    }
}
