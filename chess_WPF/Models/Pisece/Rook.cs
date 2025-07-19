using System;
using System.Collections.Generic;
using System.Windows.Documents;

namespace chess_WPF.Models.Pisece
{
    internal class Rook : ChessPiece
    {
        public Rook(TeamSide team, int x, int y, Board board) : base(team, x, y, board, team is TeamSide.White ? "../../Resources/WpiceR.png" : "../../Resources/BpiceR.png") { }

        public override List<XY> allSteps()
        {
            List<XY> steps = new List<XY>();

            // Generate steps in all four directions
            steps.AddRange(GenerateStepsInDirection(1, 0));  // Right
            steps.AddRange(GenerateStepsInDirection(-1, 0)); // Left
            steps.AddRange(GenerateStepsInDirection(0, 1));  // Down
            steps.AddRange(GenerateStepsInDirection(0, -1)); // Up

            return steps;
        }

        private IEnumerable<XY> GenerateStepsInDirection(int dx, int dy)
        {
            List<XY> steps = new List<XY>();
            int newX = xy.X + dx;
            int newY = xy.Y + dy;

            while (isValidMove(newX, newY))
            {
                steps.Add(new XY(newX, newY));

                if (board.board[newY][newX] != null && board.board[newY][newX].Team != Team)
                {
                    break; // Stop if the move captures an opponent's piece
                }

                newX += dx;
                newY += dy;
            }

            return steps;
        }

        private bool isValidMove(int x, int y)
        {
            // Ensure move is within _board bounds and target square is either empty or contains opponent piece
            return x >= 0 && x < 8 && y >= 0 && y < 8 && (board.board[y][x] == null || board.board[y][x].Team != Team);
        }
    }
}

