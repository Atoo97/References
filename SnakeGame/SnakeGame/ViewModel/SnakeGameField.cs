using SnakeGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.ViewModel
{
    public class SnakeGameField : ViewModelBase
    {

        private Color _color;

        public Color Color
        {
            get { return _color; }
            set
            {
                if (_color != value)
                {
                    _color = value;
                    OnPropertyChanged();
                }
            }
        }

        
        public Int32 X { get; set; }
        public Int32 Y { get; set; }

        public DelegateCommand FieldChangeCommand { get; set; }
    }
}
