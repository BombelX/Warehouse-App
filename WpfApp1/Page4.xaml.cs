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
using System.Windows.Threading;

namespace WpfApp1
{
    /// <summary>
    /// Logika interakcji dla klasy Page4.xaml
    /// </summary>
    public partial class Page4 : Page
    {
        public int windowIndex = 0;
        public Page4()
        {
            InitializeComponent();
            Win_index();
            Timers_start();
            ((Window2)Application.Current.Windows[windowIndex]).how_manytimes_on_break++;

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

        private int break_countdown = 0;
        private DispatcherTimer break_timer;
        private void Timers_start()
        {
            break_timer = new DispatcherTimer();
            break_timer.Interval = TimeSpan.FromSeconds(1);
            break_timer.Tick += Pack_Timer_Tick;
            break_timer.Start();
        }

        private void Pack_Timer_Tick(object sender, EventArgs e)
        {
            break_countdown++;
            int allbreak_time = ((Window2)Application.Current.Windows[windowIndex]).allbreak_time;
            now_break.Content = convtime(break_countdown);
            in_total_break.Content = convtime(break_countdown+allbreak_time);


        }

        private string convtime(int numofsec)
        {
            TimeSpan time = TimeSpan.FromSeconds(numofsec);
            string timeString = time.ToString("hh\\:mm\\:ss");
            return timeString;
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            ((Window2)Application.Current.Windows[windowIndex]).allbreak_time += break_countdown;
            break_timer.Stop();
            NavigationService navigationService = NavigationService.GetNavigationService(this);
            navigationService.RemoveBackEntry();
            ((Window2)Application.Current.Windows[windowIndex]).box_Count();
            ((Window2)Application.Current.Windows[windowIndex]).ClosePage();

        }
    }
}
