using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using chess_WPF.Models.Pisece;

namespace chess_WPF.ViewModels
{
    public class CellViewModel : INotifyPropertyChanged
    {
        public int X { get; }
        public int Y { get; }
        private ChessPiece piece;
        private string background;
        public ICommand ClickCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ChessPiece Piece
        {
            get => piece;
            set
            {
                piece = value;
                OnPropertyChanged(nameof(Piece));
                OnPropertyChanged(nameof(ImagePath)); // уведомляем об изменении картинки
            }
        }

        public string Background
        {
            get => background;
            set { background = value; OnPropertyChanged(nameof(Background)); }
        }

        public string ImagePath => Piece?.ImagePath;

        public CellViewModel(int x, int y, ICommand clickCommand)
        {
            X = x;
            Y = y;
            ClickCommand = clickCommand;
            Background = ((x + y) % 2 == 0) ? "LightGreen" : "DimGray";
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
