using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Logika interakcji dla klasy Page5.xaml
    /// </summary>
    public partial class Page5 : Page
    {
        public int windowIndex = 0;
        public Page5(int cnt, float batteryPercent, int allitems_cnt, string most)
        {
            InitializeComponent();
            Win_index();
            Start_Clock_Timer();
            ord_cnt_label.Content = cnt.ToString();
            items_count_label.Content = allitems_cnt.ToString();
            most_label.Content = most;
            Hello_Name.Content = "Witaj " + ((Window2)Application.Current.Windows[windowIndex]).user_name + " !";
        }

        // Funckja szukajca indexu głównego okna
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

        private DispatcherTimer clock_timer;

        private void Start_Clock_Timer()
        {
            clock_timer = new DispatcherTimer();
            clock_timer.Interval = TimeSpan.FromSeconds(1);
            clock_timer.Tick += Timer_Tick;
            clock_timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            current_datetime();
            get_battery_percentage();
        }

        private void current_datetime()
        {
            DateTime now = DateTime.Now;
            double seconds = now.TimeOfDay.TotalSeconds;
            TimeSpan time = TimeSpan.FromSeconds(seconds + (((Window2)Application.Current.Windows[windowIndex]).allitems_cnt * 45));
            TimeSpan acttime = TimeSpan.FromSeconds(seconds);
            string timeFormatted = time.ToString(@"hh\:mm\:ss");
            string act_timeFormatted = acttime.ToString(@"hh\:mm\:ss");
            act_time_label.Content = act_timeFormatted;
            timetoend_label.Content = timeFormatted;
        }

        private void get_battery_percentage()
        {
            try
            {
                System.Windows.Forms.PowerStatus status = System.Windows.Forms.SystemInformation.PowerStatus;
                ((Window2)Application.Current.Windows[windowIndex]).batteryPercent = status.BatteryLifePercent * 100;
                var batteryViewModel = baterylvl.DataContext as BatteryViewModel;
                batteryViewModel.Level = Convert.ToInt32(((Window2)Application.Current.Windows[windowIndex]).batteryPercent);
                if (((Window2)Application.Current.Windows[windowIndex]).batteryPercent <= 5)
                {
                    MessageBox.Show("Bateria Ma Mniej niż 5%", "Podłącz do ładowania", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch { };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            NavigationService navigationService = NavigationService.GetNavigationService(this);
            navigationService.RemoveBackEntry();
            ((Window2)Application.Current.Windows[windowIndex]).change_buttons_enabled(true);
            ((Window2)Application.Current.Windows[windowIndex]).change_buttons_big_box(((Window2)Application.Current.Windows[windowIndex]).oneormore);
            ((Window2)Application.Current.Windows[windowIndex]).box_Count();
            ((Window2)Application.Current.Windows[windowIndex]).ClosePage();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ((Window2)Application.Current.Windows[windowIndex]).a_z = true;
            zaelipse.Fill = new SolidColorBrush(Colors.Transparent);
            azelipse.Fill = new SolidColorBrush(Colors.Green);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ((Window2)Application.Current.Windows[windowIndex]).a_z = false;
            zaelipse.Fill = new SolidColorBrush(Colors.Green);
            azelipse.Fill = new SolidColorBrush(Colors.Transparent);

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            ((Window2)Application.Current.Windows[windowIndex]).oneormore = false;
            alotofboxeselipse.Fill = new SolidColorBrush(Colors.Transparent);
            oneboxelipse.Fill = new SolidColorBrush(Colors.Green);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            ((Window2)Application.Current.Windows[windowIndex]).oneormore = true;
            alotofboxeselipse.Fill = new SolidColorBrush(Colors.Green);
            oneboxelipse.Fill = new SolidColorBrush(Colors.Transparent);
        }

    }
}
