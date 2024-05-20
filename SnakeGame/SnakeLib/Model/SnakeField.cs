using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Model
{
    public class SnakeField
    {
        private bool _isBorder = false;
        private bool _isFood = false;

        public bool Border
        {
            get { return _isBorder; }
            set
            {
                if (_isBorder != value)
                {
                   _isBorder = value;

                }
            }
        }

        public bool Food
        {
            get { return _isFood; }
            set
            {
                if (_isFood != value)
                {
                    _isFood = value;

                }
            }
        }

        public Int32 X { get; set; }
        public Int32 Y { get; set; }

    }
}
