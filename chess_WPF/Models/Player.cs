using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using chess_WPF.Models.Pisece;

namespace chess_WPF.Models
{
    public class Player
    {
        public TeamSide Team { get; }
        public List<ChessPiece> piseces { get; } = new List<ChessPiece>();

        public Player(Board board, TeamSide team)
        {
            Team = team;
            if (team is TeamSide.White)
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
        
        /// <summary>
        /// Находит короля этого игрока
        /// </summary>
        public King GetKing()
        {
            return piseces.Find(p => p is King) as King;
        }
        
        /// <summary>
        /// Проверяет, есть ли у игрока доступные ходы
        /// </summary>
        public bool HasAvailableMoves()
        {
            return piseces.Any(p => p.validMoves().Count > 0);
        }
        
        /// <summary>
        /// Проверяет, принадлежит ли фигура этому игроку
        /// </summary>
        public bool OwnsPiece(ChessPiece piece)
        {
            return piece?.Team == Team;
        }
        
        /// <summary>
        /// Удаляет фигуру из списка фигур игрока
        /// </summary>
        public void RemovePiece(ChessPiece piece)
        {
            if (piece != null)
            {
                piseces.Remove(piece);
            }
        }
    }
}
