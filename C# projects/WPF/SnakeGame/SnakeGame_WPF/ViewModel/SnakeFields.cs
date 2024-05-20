using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Threading.Tasks;

namespace SnakeGame_WPF.ViewModel
{
    /// <summary>
    /// Kígyó típusa.
    /// </summary>
    public class SnakeFields : ViewModelBase
    {
        private Int32 _Xcoordinate;
        private Int32 _Ycoordinate;
        private BitmapImage? _Image;

        /// <summary>
        /// X koordináta beállítása.
        /// </summary>
        public Int32 Xcoordinate
        {
            get { return _Xcoordinate; }
            set
            {
                if (_Xcoordinate != value)
                {
                    _Xcoordinate = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Y koordináta beállítása.
        /// </summary>
        public Int32 Ycoordinate
        {
            get { return _Ycoordinate; }
            set
            {
                if (_Ycoordinate != value)
                {
                    _Ycoordinate = value;
                    OnPropertyChanged();
                }
            }
        }


        /// <summary>
        /// Kép lekérdezése, vagy beállítása.
        /// </summary>
        /// 
        public BitmapImage Image
        {
            get { return _Image!; }
            set
            {
                if (_Image != value)
                {
                    _Image = value;
                    OnPropertyChanged();
                }
            }
        }



        // public BitmapImage? Image { get; set; }



    }
}
