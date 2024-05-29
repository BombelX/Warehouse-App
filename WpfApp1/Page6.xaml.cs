using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Logika interakcji dla klasy Page6.xaml
    /// </summary>
    public partial class Page6 : Page
    {
        public int windowIndex = 0;
        public Page6()
        {
            InitializeComponent();
            Win_index();
            //Timers_start();
        }

        /// <summary>
        /// Funckja szukajca indexu głównego okna
        /// </summary>
        private void Win_index()
        {
            int index = 0;
            foreach (System.Windows.Window window in Application.Current.Windows)
            {
                if (window.Title == "MainWindow")
                {
                    windowIndex = index;
                }
                index++;
            }
        }

        //private int break_countdown = 0;
        //private DispatcherTimer break_timer;
        /*private void Timers_start()
        {
            break_timer = new DispatcherTimer();
            break_timer.Interval = TimeSpan.FromSeconds(1);
            break_timer.Tick += Pack_Timer_Tick;
            break_timer.Start();
        }*/
    }

}
