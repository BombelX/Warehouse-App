using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class GaugeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string info)
        {
            if(PropertyChanged!= null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public GaugeViewModel()
        {
            Angle = -85;
            Value = 0;
        }

        int _angle;
        public int Angle
        {
            get
            {
                return _angle;
            }

            private set
            {
                _angle = value;
                NotifyPropertyChanged("Angle");
            }
        }

        public long map(int x, long in_min = 0, long in_max = 200, long out_min = 0, long out_max = 170)
            {
                return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
            }

        int _value;
        public int Value
        {
            get
            {
                return _value;
            }

            set
            {
                if (value >= 0 && value <= 170)
                {
                    _value = value;
                    
                    Angle = value - 85;
                    NotifyPropertyChanged("Value");
                }
                if (value > 170)
                {
                    _value = value;
                    Angle = 172 - 85;
                    NotifyPropertyChanged("Value");
                }
            }
        }
    }
}
