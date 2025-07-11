using chess_WPF.Models.Pisece;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace chess_WPF.Models
{
    public class Board
    {
        private Player player1;
        private Player player2;
        public Player attacking       { get; private set; }
        public Player protecting      { get; private set; }

        public ChessPiece[][] board       { get; private set; }

        private Dictionary<string, string> Bpice = new Dictionary<string, string>();
        private Dictionary<string, string> Wpice = new Dictionary<string, string>();

        public Board()
        {
            player1 = new Player(this, ChessPiece.W);
            player2 = new Player(this, ChessPiece.B);
            board = new ChessPiece[8][];
            for (int i = 0; i < 8; i++)
            {
                board[i] = new ChessPiece[8];
            }
            attacking = player1;
            protecting = player2;
            initializePiseces();
            Bpice.Add("K", GetProjectDirectory("Resources\\BpiceK.png"));
            Bpice.Add("Q", GetProjectDirectory("Resources\\BpiceQ.png"));
            Bpice.Add("B", GetProjectDirectory("Resources\\BpiceB.png"));
            Bpice.Add("R", GetProjectDirectory("Resources\\BpiceR.png"));
            Bpice.Add("N", GetProjectDirectory("Resources\\BpiseN.png"));
            Bpice.Add("P", GetProjectDirectory("Resources\\BpiceP.png"));

            Wpice.Add("K", GetProjectDirectory("Resources\\WpiceK.png"));
            Wpice.Add("Q", GetProjectDirectory("Resources\\WpiceQ.png"));
            Wpice.Add("B", GetProjectDirectory("Resources\\WpiceB.png"));
            Wpice.Add("R", GetProjectDirectory("Resources\\WpiceR.png"));
            Wpice.Add("N", GetProjectDirectory("Resources\\WpiseN.png"));
            Wpice.Add("P", GetProjectDirectory("Resources\\WpiceP.png"));
        }

        private string GetProjectDirectory(params string[] paths)
        {
            string projectDirectory = Path.Combine(AppContext.BaseDirectory, "..", "..");
            return Path.GetFullPath(Path.Combine(projectDirectory, Path.Combine(paths)));
        }

        private void initializePiseces()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    board[i][j] = null;
                }
            }

            foreach (ChessPiece p in player1.piseces)
            {
                board[p.xy.Y][p.xy.X] = p;
            }

            foreach (ChessPiece p in player2.piseces)
            {
                board[p.xy.Y][p.xy.X] = p;
            }
        }

        public bool canMove(List<XY> allSteps, XY xy)
        {
            ChessPiece tmp = board[xy.Y][xy.X];
            if (!(tmp is King))
            {
                return allSteps.Contains(xy);
            }
            return false;
        }

        public bool isCheck(Player player)
        {
            var king = player.piseces.Find(p1 => p1 is King);
            foreach (var p in attacking.piseces)
            {
                if (p.allSteps().Contains(king.xy))
                {
                    return true;
                }
            }
            return false;
        }

        public bool isCheckmate(Player player)
        {
            foreach (var p in player.piseces)
            {
                if (p.validMoves().Count > 0)
                {
                    return false;
                }
            }
            return true;
        }

        public bool isValidMove(ChessPiece piece, XY move)
        {
            var originalPosition = piece.xy;
            var targetPiece = board[move.Y][move.X];

            board[move.Y][move.X] = piece;
            board[originalPosition.Y][originalPosition.X] = null;
            piece.move(move.X, move.Y);

            bool valid = !isCheck(protecting);

            board[move.Y][move.X] = targetPiece;
            board[originalPosition.Y][originalPosition.X] = piece;
            piece.move(originalPosition.X, originalPosition.Y);

            return valid;
        }

        public void kill(ChessPiece pisece)
        {
            protecting.piseces.Remove(pisece);

            // Удаляем фигуру с доски
            if (pisece != null)
            {
                var pos = pisece.xy;
                if (pos.Y >= 0 && pos.Y < 8 && pos.X >= 0 && pos.X < 8)
                    board[pos.Y][pos.X] = null;
            }
        }

        public bool teamA()
        {
            return attacking.team;
        }

        public void swap()
        {
            Player tmp = attacking;
            attacking = protecting;
            protecting = tmp;
        }

        public void SwapPieces(int x1, int y1, int x2, int y2)
        {
            var piece1 = board[y1][x1];
            var piece2 = board[y2][x2];

            board[y1][x1] = piece2;
            board[y2][x2] = piece1;

            // Обновляем координаты фигур, если они не null
            if (piece1 != null)
                piece1.move(x2, y2);
            if (piece2 != null)
                piece2.move(x1, y1);
        }

        public class MoveResult
        {
            public bool Success { get; set; }
            public bool IsCheck { get; set; }
            public bool IsCheckmate { get; set; }
            public string Message { get; set; }
        }

        public MoveResult TryMove(int fromX, int fromY, int toX, int toY)
        {
            var result = new MoveResult();
            var movingPiece = board[fromY][fromX];
            if (movingPiece == null)
            {
                result.Success = false;
                result.Message = "Нет фигуры для перемещения.";
                return result;
            }

            // Проверка принадлежности фигуры текущему игроку
            if (movingPiece.team != attacking.team)
            {
                result.Success = false;
                result.Message = "Нельзя ходить чужой фигурой.";
                return result;
            }

            // Получение всех возможных ходов
            var validMoves = movingPiece.validMoves();
            var targetXY = new XY(toX, toY);
            if (!validMoves.Contains(targetXY))
            {
                result.Success = false;
                result.Message = "Недопустимый ход.";
                return result;
            }

            // Взятие фигуры противника
            var targetPiece = board[toY][toX];
            if (targetPiece != null && targetPiece.team != movingPiece.team)
            {
                kill(targetPiece);
            }

            // Перемещение фигуры
            SwapPieces(fromX, fromY, toX, toY);

            // Смена очереди
            swap();

            // Проверка шаха и мата
            result.IsCheck = isCheck(protecting);
            result.IsCheckmate = isCheckmate(protecting);
            result.Success = true;
            if (result.IsCheckmate)
                result.Message = $"{(protecting.team ? "Белым" : "Чёрным")} мат!";
            else if (result.IsCheck)
                result.Message = $"{(protecting.team ? "Белым" : "Чёрным")} шах!";
            else
                result.Message = "Ход выполнен.";

            return result;
        }
    }
}
