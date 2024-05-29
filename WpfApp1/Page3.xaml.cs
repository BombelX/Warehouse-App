using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Diagnostics.Eventing.Reader;
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
    /// Logika interakcji dla klasy Page3.xaml
    /// </summary>
    public partial class Page3 : Page
    {
        public int windowIndex = 0;
        Dictionary<int, string> slownik = new Dictionary<int, string>()
        {
            { 1, "jedynki" },
            { 2, "dwojeczki" },
            { 3, "trojeczki???" }
        };

        public Page3()
        {
            InitializeComponent();
            toilet_timer();
            Win_index();
            ((Window2)Application.Current.Windows[windowIndex]).how_manytimes_in_toilet++;

        }

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

        private int wc_countdown = 0;
        private DispatcherTimer wc_timer;
        private void toilet_timer()
        {
            wc_timer = new DispatcherTimer();
            wc_timer.Interval = TimeSpan.FromSeconds(1);
            wc_timer.Tick += Pack_Timer_Tick;
            wc_timer.Start();
        }

        private void Pack_Timer_Tick(object sender, EventArgs e)
        {
            wc_countdown++;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            toilet_time(1);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            toilet_time(2);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            toilet_time(3);
        }

        private void toilet_time(int type_of_activity)
        {

            belt_1.Visibility = Visibility.Hidden; belt_2.Visibility = Visibility.Hidden;
            piss.Visibility = Visibility.Hidden; shit.Visibility = Visibility.Hidden; vomit.Visibility = Visibility.Hidden;
            back_to_packing.Visibility = Visibility.Visible;
            curtain.Visibility = Visibility.Visible;
            MessageBox.Show($"Miłej {slownik[type_of_activity]} {((Window2)Application.Current.Windows[windowIndex]).user_name} ;)");

        }

        private void back_to_packing_Click(object sender, RoutedEventArgs e)
        {
            NavigationService navigationService = NavigationService.GetNavigationService(this);
            navigationService.RemoveBackEntry();
            ((Window2)Application.Current.Windows[windowIndex]).pack_countdown -= wc_countdown;
            ((Window2)Application.Current.Windows[windowIndex]).time_in_toilet += wc_countdown;
            ((Window2)Application.Current.Windows[windowIndex]).ClosePage();
            ((Window2)Application.Current.Windows[windowIndex]).box_Count();
        }
    }
}
