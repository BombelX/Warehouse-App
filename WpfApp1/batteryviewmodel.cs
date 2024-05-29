using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection.Metadata;
using System.Text;

namespace WpfApp1
{
    public class BatteryViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        public BatteryViewModel()
        {
            Level = 0;
        }

        int _level;
        public string Percentage
        {
            get
            {
                return _level + "%";
            }
        }
        public int Level
        {
            get
            {
                return _level;
            }

            set
            {
                _level = value;
                NotifyPropertyChanged("Level");
                NotifyPropertyChanged("Percentage");
            }
        }

    }
}
