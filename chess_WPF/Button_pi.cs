using chess_WPF.Models.Pisece;
using System.Windows;
using System.Windows.Controls;

namespace chess_WPF
{
    public class Button_pi : Button
    {
        public static readonly DependencyProperty PieceProperty =
            DependencyProperty.Register(
                "Piece",
                typeof(ChessPiece),
                typeof(Button_pi),
                new PropertyMetadata(null));

        public ChessPiece Piece
        {
            get { return (ChessPiece)GetValue(PieceProperty); }
            set { SetValue(PieceProperty, value); }
        }
    }
}