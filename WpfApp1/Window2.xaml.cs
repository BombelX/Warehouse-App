using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Threading;
using System.Data.SQLite;
using System.Net.Sockets;
using System.Collections;
using System.Windows.Input;
using System.Windows.Media;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Data.Entity.Infrastructure;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Documents;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace WpfApp1
{
    public partial class Window2 : Window
    {
        public bool is_it_backup = false;
        public int to_backup = 0;
        public int how_manytimes_on_break = 0;
        public int time_in_toilet;
        public int how_manytimes_in_toilet = 0;
        public string user_name;
        public string selected_depot = "Mag-1";
        public string most = "";
        public bool a_z = true;
        public bool oneormore = true;
        public int allbreak_time = 0;
        public int allitems_cnt = 0;
        public int allitemspacked_cnt = 0;
        public bool isstart_clicked = false;
        public bool isProgres = false;
        public bool is_db_ready = false;
        public int allpack_cnt = 0;
        public float packed = 0;
        public int selected_orders = 0;
        public int ord_num = 0;
        public int order_index = 0;
        public float batteryPercent = 0;
        public long return_id = 0;
        int currentLabelIndex = 0;
        public string path_to_images = AppDomain.CurrentDomain.BaseDirectory + "allegro\\images\\";
        public DoubleAnimation animation = new DoubleAnimation();
        public Dictionary<int, List<Dictionary<string, string>>> mainDictionary = new Dictionary<int, List<Dictionary<string, string>>>();
        public List<Dictionary<string, string>> mainDictionary_one = new List<Dictionary<string, string>>();


        public Window2()
        {
            InitializeComponent();
            Window_Loaded();
            change_buttons_enabled(false);
            gauge.DataContext = new GaugeViewModel();
        }

        private void get_battery_percentage()
        {
            try
            {
                System.Windows.Forms.PowerStatus status = System.Windows.Forms.SystemInformation.PowerStatus;
                batteryPercent = status.BatteryLifePercent * 100;
                var batteryViewModel = baterylvl.DataContext as BatteryViewModel;
                batteryViewModel.Level = Convert.ToInt32(batteryPercent);
                if (batteryPercent <= 5)
                {
                    MessageBox.Show("Bateria Ma Mniej niż 5%", "Podłącz do ładowania", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch { };
        }

        public void change_buttons_enabled(bool enabled = false)
        {
            Nieznaleziono.IsEnabled = enabled;
            start.IsEnabled = enabled;
            foreach (UIElement element in main_grid.Children)
            {
                if (element is Button)
                {
                    Button button = (Button)element;
                    button.IsEnabled = enabled;
                }
            }
        }

        public void change_buttons_big_box(bool enabled = false)
        {
            box_down.IsEnabled = enabled;
            add_package.IsEnabled = enabled;
            boxes.IsEnabled = enabled;
            next_box.IsEnabled = enabled;
            previous_box.IsEnabled = enabled;
            if (!enabled)
            {
                box11.Visibility = Visibility.Visible;
                b11.Visibility = Visibility.Visible;
            }

        }

        public void BackUp(string param, Dictionary<string, string> elem = null)
        {
            if (param == "start")
            {
                string connectionString = "Data Source=backdb.db;Version=3;";
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO maindirectory (`box_id`, `offer_id`, `rack_id`, `item_id`, `section`, `findingpath_id`, `syg_id`, `user_name`, `is_return`, `is_packed`) VALUES (@value1, @value2, @value3, @value4, @value5, @value6, @value7, @value8, @value9, @value10);";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        for (int i = 0; i < allpack_cnt; i++)
                        {
                            foreach (var element in mainDictionary[i])
                            {
                                command.Parameters.AddWithValue("@value1", element["box_id"].ToString());
                                command.Parameters.AddWithValue("@value2", element["offer_id"].ToString());
                                command.Parameters.AddWithValue("@value3", element["rack_id"].ToString());
                                command.Parameters.AddWithValue("@value4", element["item_id"].ToString());
                                command.Parameters.AddWithValue("@value5", element["section"].ToString());
                                command.Parameters.AddWithValue("@value6", element["findingpath_id"].ToString());
                                command.Parameters.AddWithValue("@value7", element["syg_id"].ToString());
                                command.Parameters.AddWithValue("@value8", element["user_name"].ToString()).ToString();
                                command.Parameters.AddWithValue("@value9", element["is_return"].ToString());
                                command.Parameters.AddWithValue("@value10", element["ispacked"].ToString());
                                int rowsInserted = command.ExecuteNonQuery();
                            }
                        }
                    }
                    query = "INSERT INTO config (`a_z`,`oneormore`,`user_name`,`selected_depot`) VALUES (@value1, @value2, @value3, @value4) ;";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@value1", Convert.ToInt32(a_z));
                        command.Parameters.AddWithValue("@value2", Convert.ToInt32(oneormore));
                        command.Parameters.AddWithValue("@value3", user_name.ToString());
                        command.Parameters.AddWithValue("@value4", selected_depot.ToString());
                        command.ExecuteNonQuery();
                    }
                    query = "INSERT INTO stats (`packing_time`,`packing_speed`,`how_many_times_person_visited_toilet`,`how_many_time_person_spent_in_toilet`,`how_many_breaks_person_have_had`,`how_many_time_person_have_spent_on_breaks`,`ord_num`,`selected_orders`,`all_items_packed`,`packed`) VALUES (@value1, @value2, @value3, @value4, @value5, @value6,@value7,@value8,@value9,@value10) ;";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@value1", pack_countdown.ToString());
                        command.Parameters.AddWithValue("@value2", gauge.pack_speed_slider.Value.ToString());
                        command.Parameters.AddWithValue("@value3", how_manytimes_in_toilet.ToString());
                        command.Parameters.AddWithValue("@value4", time_in_toilet.ToString());
                        command.Parameters.AddWithValue("@value5", how_manytimes_on_break.ToString());
                        command.Parameters.AddWithValue("@value6", allbreak_time.ToString());
                        command.Parameters.AddWithValue("@value7", ord_num.ToString());
                        command.Parameters.AddWithValue("@value8", selected_orders.ToString());
                        command.Parameters.AddWithValue("@value9", allitemspacked_cnt.ToString());
                        command.Parameters.AddWithValue("@value10", packed.ToString());
                        command.ExecuteNonQuery();
                    }

                }
            }
            else if (param == "next_box")
            {
                string connectionString = "Data Source=backdb.db;Version=3;";
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE maindirectory SET `is_packed` = @value2 WHERE `offer_id` = @value1;";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@value1", elem["offer_id"].ToString());
                        command.Parameters.AddWithValue("@value2", elem["ispacked"].ToString());
                        command.ExecuteNonQuery();
                    }
                    query = "UPDATE stats SET `packing_time` = @value1,`packing_speed` = @value2, `how_many_times_person_visited_toilet` = @value3,`how_many_time_person_spent_in_toilet` = @value4,`how_many_breaks_person_have_had` = @value5,`how_many_time_person_have_spent_on_breaks` = @value6,`ord_num` = @value7,`selected_orders` = @value8,`all_items_packed` = @value9,`packed` = @value10";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@value1", pack_countdown.ToString());
                        command.Parameters.AddWithValue("@value2", gauge.pack_speed_slider.Value.ToString());
                        command.Parameters.AddWithValue("@value3", how_manytimes_in_toilet.ToString());
                        command.Parameters.AddWithValue("@value4", time_in_toilet.ToString());
                        command.Parameters.AddWithValue("@value5", how_manytimes_on_break.ToString());
                        command.Parameters.AddWithValue("@value6", allbreak_time.ToString());
                        command.Parameters.AddWithValue("@value7", ord_num.ToString());
                        command.Parameters.AddWithValue("@value8", selected_orders.ToString());
                        command.Parameters.AddWithValue("@value9", allitemspacked_cnt.ToString());
                        command.Parameters.AddWithValue("@value10", packed.ToString());
                        command.ExecuteNonQuery();
                    }
                }
            }
            else if (param == "timer" || param == "add_box")
            {
                string connectionString = "Data Source=backdb.db;Version=3;";
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE stats SET `packing_time` = @value1,`packing_speed` = @value2, `how_many_times_person_visited_toilet` = @value3,`how_many_time_person_spent_in_toilet` = @value4,`how_many_breaks_person_have_had` = @value5,`how_many_time_person_have_spent_on_breaks` = @value6,`ord_num` = @value7,`selected_orders` = @value8,`all_items_packed`=@value9,`packed`=@value10";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@value1", pack_countdown.ToString());
                        command.Parameters.AddWithValue("@value2", gauge.pack_speed_slider.Value.ToString());
                        command.Parameters.AddWithValue("@value3", how_manytimes_in_toilet.ToString());
                        command.Parameters.AddWithValue("@value4", time_in_toilet.ToString());
                        command.Parameters.AddWithValue("@value5", how_manytimes_on_break.ToString());
                        command.Parameters.AddWithValue("@value6", allbreak_time.ToString());
                        command.Parameters.AddWithValue("@value7", ord_num.ToString());
                        command.Parameters.AddWithValue("@value8", selected_orders.ToString());
                        command.Parameters.AddWithValue("@value9", allitemspacked_cnt.ToString());
                        command.Parameters.AddWithValue("@value10", packed.ToString());
                        command.ExecuteNonQuery();
                    }
                }
            }
            else if (param == "delete")
            {
                string connectionString = "Data Source=backdb.db;Version=3;";
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM stats";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    query = "DELETE FROM config";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    query = "DELETE FROM maindirectory";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }

        }

        private async void Window_Loaded()
        {

            string ipa = "";
            Barcode.Focus();
            string connetionString;
            SqlConnection cnn;
            int cnt;
            is_db_ready = false;
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipa = ip.ToString();
                }
            }
            ipa = "192.168.1.52";
            connetionString = $@"Data Source={ipa}\SQLEXPRESS;Initial Catalog=Tomek_DB;User ID=Tomek_Admin;Password=ZAQ!2wsxcde3";
            string sql = "SELECT COUNT(DISTINCT NR) AS cnt FROM dbo.DATA";
            cnn = new SqlConnection(connetionString);
            try
            {
                await cnn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(sql, cnn))
                {
                    SqlDataReader reader;
                    using (reader = await cmd.ExecuteReaderAsync())
                    {
                        await reader.ReadAsync();
                        cnt = reader.GetInt32(0);
                    }
                }
                allpack_cnt = cnt;
                // Tworzenie listy słowników i dodawanie jej do głównego słownika  SELECT COUNT(DISTINCT NR) AS cnt FROM dbo.DATA
                int index = 0;
                for (int i = 0; i < cnt; i++)//count wszytkich nr
                {
                    List<Dictionary<string, string>> dictionariesList = new List<Dictionary<string, string>>();
                    sql = $"SELECT MIN(CAST(NR AS int)) FROM dbo.DATA WHERE NR > {index};";
                    using (SqlCommand cmd = new SqlCommand(sql, cnn))
                    {
                        SqlDataReader reader;
                        using (reader = await cmd.ExecuteReaderAsync())
                        {
                            await reader.ReadAsync();
                            index = reader.GetInt32(0);
                        }
                    }

                    // SELECT * FROM dbo.DATA WHERE Nr = 10 AND AUKCJA != '' ;
                    sql = $"SELECT * FROM dbo.DATA WHERE Nr = {index} AND AUKCJA != '' ;";
                    using (SqlCommand cmd = new SqlCommand(sql, cnn))
                    {
                        SqlDataReader reader;
                        using (reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                                dictionary.Add("box_id", reader.GetString(0));
                                dictionary.Add("offer_id", reader.GetString(1));
                                dictionary.Add("rack_id", reader.GetString(2));
                                dictionary.Add("item_id", reader.GetString(3));
                                dictionary.Add("section", reader.GetString(4));
                                dictionary.Add("findingpath_id", reader.GetString(5));
                                dictionary.Add("syg_id", reader.GetString(6));
                                dictionary.Add("user_name", reader.GetString(7));
                                dictionary.Add("is_return", reader.GetString(8).ToString());
                                dictionary.Add("ispacked", "unpacked");
                                allitems_cnt++;
                                dictionariesList.Add(dictionary);

                                mainDictionary_one.Add(dictionary);

                            }
                            mainDictionary.Add(i, dictionariesList);

                        }

                    }

                    sql = "SELECT TOP 1 SUBSTRING(SYG, 1, 2) AS most, COUNT(*) AS count FROM dbo.DATA GROUP BY SUBSTRING(SYG, 1, 2) ORDER BY count DESC;\r\n";
                    using (SqlCommand cmd = new SqlCommand(sql, cnn))
                    {
                        SqlDataReader reader;
                        using (reader = await cmd.ExecuteReaderAsync())
                        {
                            await reader.ReadAsync();
                            most = reader.GetString(0);
                        }
                    }
                }
                /*MessageBox.Show("Baza Załadowana");*/
                is_db_ready = true;
                section.Content = mainDictionary[0][0]["section"];


            }
            catch (Exception ex)

            {
                MessageBox.Show(ex.Message);
                dberrormessage();

            }
        }

        public void dberrormessage()
        {
            MessageBoxResult result = MessageBox.Show($"Błąd połączenia do bazy danych. Czy chcesz ponowić?", "Error", MessageBoxButton.YesNo, MessageBoxImage.Error);
            if (result == MessageBoxResult.Yes)
            {
                Window_Loaded();
            }
            else
            {
                Application.Current.Shutdown();
            }
        }

        private DispatcherTimer pack_timer;
        public int pack_countdown = 0;

        public void packing_stats()
        {
            pack_timer = new DispatcherTimer();
            pack_timer.Interval = TimeSpan.FromSeconds(1);
            pack_timer.Tick += Pack_Timer_Tick;
            pack_timer.Start();

        }

        private void Pack_Timer_Tick(object sender, EventArgs e)  //tu poprawic
        {
            pack_countdown++;
            float items_per_hour = packed / pack_countdown * 3600;
            /*timer_pack.Content = Math.Round(items_per_hour,1).ToString() ;*/
            gauge.pack_speed_slider.Value = items_per_hour;
            get_battery_percentage();
            if (to_backup == 5)
            {
                BackUp("timer");
                to_backup = 0;
            }
            else
            {
                to_backup++;
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Barcode.Focus();
            if (currentLabelIndex <= 0)
            {
                MessageBox.Show("Nie można usunąc więcej elementów", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                Label label = this.FindName("b" + currentLabelIndex) as Label;
                Image img = this.FindName("box" + currentLabelIndex) as Image;
                img.Visibility = Visibility.Collapsed;
                label.Visibility = Visibility.Collapsed;
                currentLabelIndex--;
                selected_orders--;
                ord_num--;
            }
            box_Count();
            last_selected_pack_update();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Barcode.Focus();

            // dodawanie paczki
            if (currentLabelIndex >= 10)
            {
                MessageBox.Show("Nie można usunąc więcej elementów", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                if (ord_num >= allpack_cnt)
                {
                    MessageBox.Show("Nie można dodac kolejnej paczki poniewaz nie ma jej w liście zamowień, cofnij sie do niespakowanych paczek, spakuj zgubione lub zakoncz pakowanie jesli wszystko spakowane", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    currentLabelIndex++;
                    int ilosc = 0;
                    selected_orders++;

                    ilosc = mainDictionary[ord_num].Count();

                    Label label = this.FindName("b" + currentLabelIndex) as Label;
                    Image img = this.FindName("box" + currentLabelIndex) as Image;
                    img.Visibility = Visibility.Visible;

                    label.Visibility = Visibility.Visible;
                    label.Content = $"0/{ilosc}";
                    ord_num++;
                }
            }
            box_Count();
            last_selected_pack_update();


        }
        private void last_selected_pack_update()
        {
            packed_now.Content = "Ostatnia wybrana paczka " + ord_num.ToString() + " z " + allpack_cnt.ToString();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Barcode.Focus();
            //animowana zmiana rozmiaru zdjecia
            Image image = sender as Image;
            ScaleTransform transform = image.RenderTransform as ScaleTransform;
            if (transform != null && transform.ScaleX == 2)
            {
                transform.ScaleX = 1;
                transform.ScaleY = 1;
                image.RenderTransform = transform;
            }
            else
            {
                transform = new ScaleTransform(2, 2);
                image.RenderTransform = transform;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            progres.Value = countdown;
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = progres.Value;
            animation.To = countdown;
            animation.Duration = new Duration(TimeSpan.FromSeconds(1));

            progres.BeginAnimation(ProgressBar.ValueProperty, animation);
            countdown--;
            if (countdown == 0)
            {
                timer.Stop();
            }
        }

        private void ZWROTImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Barcode.Focus();
            //animowana zmiana rozmiaru zdjecia
            Image image = sender as Image;
            image.Visibility = Visibility.Hidden;
        }

        private void RestartTimer()
        {
            countdown = 35;
            progres.Value = countdown;
            timer.Start();
        }

        private DispatcherTimer timer;
        private int countdown = 35;

        private void StartProgressAsync()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

                if (Barcode.Text.ToString() == Item_Id.Content.ToString() || Barcode.Text.ToString() == "H-mag" || Barcode.Text.ToString() == "Mag-h")
                {
                    if (isstart_clicked == true)
                    {
                        if (oneormore == true)
                        {
                            for (int i = 0; i < allpack_cnt; i++)
                            {
                                foreach (var elem in mainDictionary[i])//tutaaajjjjjjjjjjjjjjjj
                                {
                                    if (elem["item_id"] == Item_Id.Content.ToString())
                                    {
                                        elem["ispacked"] = "packed";
                                        packed++;
                                        BackUp("next_box", elem);
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < allitems_cnt; i++)
                            {
                                if (mainDictionary_one[i]["item_id"] == Item_Id.Content.ToString())
                                {
                                    mainDictionary_one[i]["ispacked"] = "packed";
                                    packed++;
                                    BackUp("next_box", mainDictionary_one[i]);
                                }
                            }
                        }
                    }

                    isstart_clicked = false;

                    box_Count();
                    nxt_Click();
                    allitemspacked_cnt++;
                }
                if (Barcode.Text == "Toilet" || Barcode.Text == "toilet")
                {
                    lost_items_frame.Visibility = Visibility.Visible;
                    lost_items_frame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
                    Page3 page3 = new Page3();
                    lost_items_frame.NavigationService.Navigate(page3);

                }
                if (Barcode.Text == "Break" || Barcode.Text == "break")
                {
                    lost_items_frame.Visibility = Visibility.Visible;
                    lost_items_frame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
                    Page4 page4 = new Page4();
                    lost_items_frame.NavigationService.Navigate(page4);

                }
                Barcode.Text = "";
                packing_status();
            }

        }


        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            int ad_num = (10 - selected_orders);
            Barcode.Focus();

            if (currentLabelIndex >= 10)
            {
                MessageBox.Show("Nie można usunac więcej elementów", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                if (ord_num >= allpack_cnt)
                {
                    MessageBox.Show("Nie można dodać kolejnej paczki, ponieważ nie ma jej w liście zamówień cofnij się do niespakowanych paczek, spakuj zgubione lub zakończ pakowanie jeśli wszystko spakowane");
                }
                else
                {
                    if ((10 - selected_orders) >= (allpack_cnt - ord_num))
                    {
                        ad_num = (allpack_cnt - ord_num - 1);
                    }

                    for (int i = ord_num, index = currentLabelIndex + 1; i < ord_num + (ad_num); i++, index++)
                    {
                        Label label = this.FindName("b" + index) as Label;
                        Image img = this.FindName("box" + index) as Image;

                        int ilosc = mainDictionary[i].Count();

                        if (label != null && img != null)
                        {
                            try
                            {
                                img.Source = new BitmapImage(new Uri("assets\\Box-Open.png", UriKind.Relative));
                            }
                            catch { }

                            img.Visibility = Visibility.Visible;
                            label.Visibility = Visibility.Visible;
                            label.Content = $"0/{ilosc}";
                        }
                    }
                }

                if ((10 - selected_orders) >= (allpack_cnt - ord_num))
                {
                    ord_num += (allpack_cnt - ord_num);
                }
                else
                {
                    ord_num += (10 - selected_orders);
                }

                currentLabelIndex = 10;
                selected_orders = 10;
                box_Count();
                last_selected_pack_update();
            }
        }

        public void box_Count()
        {
            if (oneormore == true)
            {
                int items_selected = 0;
                for (int i = ord_num - selected_orders, j = 1; i < ord_num; i++, j++)
                {
                    int packed_count = 0;
                    foreach (var elmt in mainDictionary[i]) //tutaajjjjjjjjjjjjj
                    {
                        if (elmt["ispacked"] == "packed")
                        {
                            packed_count++;
                        }
                    }

                    Label label = this.FindName("b" + (j)) as Label;
                    string cnt = label.Content.ToString();
                    string[] cnt_tab = cnt.Split("/");
                    items_selected += Convert.ToInt32(cnt_tab[1]) - packed_count;
                    label.Content = $"{packed_count}/{cnt_tab[1]}";
                    if (packed_count == Convert.ToInt32(cnt_tab[1]))
                    {
                        Image img = this.FindName("box" + j) as Image;
                        try
                        {
                            img.Source = new BitmapImage(new Uri("assets\\Box-Close.png", UriKind.Relative));
                        }
                        catch { }
                        label.Foreground = new SolidColorBrush(Colors.Green);
                    }
                }
                selected_items_label.Content = items_selected.ToString();
                TimeSpan time = TimeSpan.FromSeconds(items_selected*45);
                string timeFormatted = time.ToString(@"hh\:mm\:ss");


            }
            else
            {
                int packed_count = 0;
                for (int i = 0; i < allitems_cnt; i++)
                {
                    if (mainDictionary_one[i]["ispacked"] == "packed")
                    {
                        packed_count++;
                    }
                }
                b11.Content = $"{packed_count}/{allitems_cnt}";
                if (packed_count == allitems_cnt)
                {
                    Image img = this.FindName("box11") as Image;
                    try
                    {
                        img.Source = new BitmapImage(new Uri("assets\\Box-Close.png", UriKind.Relative));
                    }
                    catch { }
                    b11.Foreground = new SolidColorBrush(Colors.Green);
                }
            }
        }


        private void nxt_Click()
        {
            if (Item_Id.Content.ToString() != "Wszystko")
            {
                start.Visibility = Visibility.Hidden;
            }
            if (isProgres == false)
            {
                StartProgressAsync();
                isProgres = true;
            }
            else
            {
                if (Item_Id.Content.ToString() != "Wszystko")
                {
                    packed++;
                }
                RestartTimer();
            }

            box_Count();
            order_index = 0;
            Barcode.Focus();
            List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
            if (oneormore == true)
            {
                for (int i = ord_num - selected_orders; i < ord_num; i++)
                {
                    foreach (var element in mainDictionary[i]) //tutajjjjjjjjjjjjjj
                    {
                        if (element["ispacked"] != "packed" && element["ispacked"] != "lost")
                        {
                            list.Add(element);
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < allitems_cnt; i++)
                {
                    if (mainDictionary_one[i]["ispacked"] != "packed" && mainDictionary_one[i]["ispacked"] != "lost")
                    {
                        list.Add(mainDictionary_one[i]);
                    }
                }
            }
            List<Dictionary<string, string>> sortedList = list.OrderBy(x => int.Parse(x["findingpath_id"])).ThenBy(x => int.Parse(new string(x["item_id"].Where(char.IsDigit).ToArray()))).ToList();
            if (a_z == false)
            {

                sortedList = list.OrderByDescending(x => int.Parse(x["findingpath_id"])).ThenByDescending(x => int.Parse(new string(x["item_id"].Where(char.IsDigit).ToArray()))).ToList();
            }
            //List<Dictionary<string, string>> sortedList = list.OrderBy(x => int.Parse(x["findingpath_id"])).ThenBy(x => int.Parse(x["item_id"])).ToList();
            try
            {
                if (order_index == sortedList.Count())
                {
                    box_Count();
                    Item_Id.Content = "Wszystko";
                    Rack_id.Content = "Spakowane";
                    start.Visibility = Visibility.Visible;
                    return;
                }
            }
            catch { }

            try
            {
                prev_item_id.Content = sortedList[order_index - 1]["item_id"];
                prev_rack_id.Content = sortedList[order_index - 1]["rack_id"];
            }
            catch
            {
                prev_item_id.Content = "Pierwsza Rzecz";
                prev_rack_id.Content = "To Dopiero";
            }
            Item_Id.Content = sortedList[order_index]["item_id"];
            Rack_id.Content = sortedList[order_index]["rack_id"];
            if (sortedList[order_index]["is_return"] == "1")
            {
                return_id = Convert.ToInt64(sortedList[order_index]["offer_id"]);
                ZWROT.Visibility = Visibility.Visible;
                POKZWROT.Visibility = Visibility.Visible;
            }
            else
            {
                ZWROT.Visibility = Visibility.Hidden;
                POKZWROT.Visibility = Visibility.Hidden;
            }
            if (oneormore == true)
            {

                for (int i = 0; i < mainDictionary[Convert.ToInt32(sortedList[order_index]["box_id"]) - 1].Count(); i++)
                {
                    var elem = mainDictionary[Convert.ToInt32(sortedList[order_index]["box_id"]) - 1];
                    if (elem[i]["offer_id"] == sortedList[order_index]["offer_id"])
                    {
                        mainDictionary[Convert.ToInt32(sortedList[order_index]["box_id"]) - 1][i]["ispacked"] = "packed";
                        BackUp("next_box", elem[i]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < allitems_cnt; i++)
                {
                    if (mainDictionary_one[i]["offer_id"] == sortedList[order_index]["offer_id"])
                    {
                        mainDictionary_one[i]["ispacked"] = "packed";
                        BackUp("next_box", mainDictionary_one[i]);
                    }
                }
            }

            string url = path_to_images + sortedList[order_index]["offer_id"].ToString() + ".jpg";
            try
            {
                clothes.Source = new BitmapImage(new Uri(url));
            }
            catch
            {
                try
                {
                    clothes.Source = new BitmapImage(new Uri("assets\\unknown.png", UriKind.Relative));
                }
                catch { };
            };
            order_index++;
        }
        public void packing_status()
        {
            float procent = Convert.ToSingle(allitemspacked_cnt) / Convert.ToSingle(allitems_cnt);
            pack_status.Value = Convert.ToInt32((Math.Round(procent, 2) * 100));
            proc.Text = Convert.ToInt32((Math.Round(procent, 2) * 100)).ToString() + "%";
            to_end.Content = (allitems_cnt - allitemspacked_cnt).ToString();
        }
        private void POKZWROT_Click(object sender, RoutedEventArgs e)
        {
            string url = "";
            if (oneormore == true)
            {
                for (int i = ord_num - selected_orders; i < ord_num; i++)
                {
                    if (url != "")
                    {
                        break;
                    }

                    foreach (var element in mainDictionary[i]) //tutajj
                    {
                        if (element["item_id"] == Item_Id.Content.ToString())
                        {
                            url = path_to_images + element["offer_id"] + ".jpg";
                            break;
                        }
                    }
                }

            }
            else
            {
                for (int i = 0; i < allitems_cnt; i++)
                {
                    if (mainDictionary_one[i]["item_id"] == Item_Id.Content.ToString())
                    {
                        url = path_to_images + mainDictionary_one[i]["offer_id"] + ".jpg";
                        break;
                    }
                }
            }


            try
            {
                ZWROT_IMG.Source = new BitmapImage(new Uri(url));
            }
            catch
            {
                try
                {
                    clothes.Source = new BitmapImage(new Uri("assets\\unknown.png", UriKind.Relative));
                }
                catch { };
            };
            ZWROT_IMG.Visibility = Visibility.Visible;
            MessageBox.Show(return_id.ToString());
        }

        private void Nieznaleziono_Click(object sender, RoutedEventArgs e)
        {
            if (isstart_clicked == true)
            {
                if (oneormore == true)
                {
                    for (int i = 0; i < allpack_cnt; i++)
                    {
                        foreach (var elem in mainDictionary[i])//tutaaajjjjjjjjjjjjjjjj
                        {
                            if (elem["item_id"] == Item_Id.Content.ToString())
                            {
                                elem["ispacked"] = "lost";
                                lost_items.Content = (Convert.ToInt32(lost_items.Content) + 1).ToString();
                                BackUp("next_box", elem);
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < allitems_cnt; i++)
                    {
                        if (mainDictionary_one[i]["item_id"] == Item_Id.Content.ToString())
                        {
                            mainDictionary_one[i]["ispacked"] = "lost";
                            lost_items.Content = (Convert.ToInt32(lost_items.Content) + 1).ToString();
                            BackUp("next_box", mainDictionary_one[i]);
                        }
                    }
                }
                
            }

            if (Item_Id.Content.ToString() != "Wszystko")
            {
                start.Visibility = Visibility.Hidden;
            }
            if (isProgres == false)
            {
                StartProgressAsync();
                isProgres = true;
            }
            else
            {
                RestartTimer();
            }

            box_Count();
            order_index = 0;
            Barcode.Focus();
            List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
            if (oneormore == true)
            {
                for (int i = ord_num - selected_orders; i < ord_num; i++)
                {
                    foreach (var element in mainDictionary[i]) //tutajjjjjjjjjjjjjj
                    {
                        if (element["ispacked"] != "packed" && element["ispacked"] != "lost")
                        {
                            list.Add(element);
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < allitems_cnt; i++)
                {
                    if (mainDictionary_one[i]["ispacked"] != "packed" && mainDictionary_one[i]["ispacked"] != "lost")
                    {
                        list.Add(mainDictionary_one[i]);
                    }
                }
            }

            List<Dictionary<string, string>> sortedList = list.OrderBy(x => int.Parse(x["findingpath_id"])).ThenBy(x => int.Parse(new string(x["item_id"].Where(char.IsDigit).ToArray()))).ToList();
            if (a_z == false)
            {

                sortedList = list.OrderByDescending(x => int.Parse(x["findingpath_id"])).ThenByDescending(x => int.Parse(new string(x["item_id"].Where(char.IsDigit).ToArray()))).ToList();
            }
            if (order_index == sortedList.Count())
            {
                box_Count();
                Item_Id.Content = "Wszystko";
                Rack_id.Content = "Spakowane";
                start.Visibility = Visibility.Visible;
                return;
            }

            try
            {
                prev_item_id.Content = sortedList[order_index - 1]["item_id"];
                prev_rack_id.Content = sortedList[order_index - 1]["rack_id"];
            }
            catch
            {
                prev_item_id.Content = "Pierwsza Rzecz";
                prev_rack_id.Content = "To Dopiero";
            }
            Item_Id.Content = sortedList[order_index]["item_id"];
            Rack_id.Content = sortedList[order_index]["rack_id"];
            if (sortedList[order_index]["is_return"] == "1")
            {
                return_id = Convert.ToInt64(sortedList[order_index]["offer_id"]);
                ZWROT.Visibility = Visibility.Visible;
                POKZWROT.Visibility = Visibility.Visible;
            }
            else
            {
                ZWROT.Visibility = Visibility.Hidden;
                POKZWROT.Visibility = Visibility.Hidden;
            }

            if (isstart_clicked == false) {
                isstart_clicked = false;
                if (oneormore == true)
            {

                for (int i = 0; i < mainDictionary[Convert.ToInt32(sortedList[order_index]["box_id"]) - 1].Count(); i++)
                {
                    var elem = mainDictionary[Convert.ToInt32(sortedList[order_index]["box_id"]) - 1];
                    if (elem[i]["offer_id"] == sortedList[order_index]["offer_id"])
                    {
                        mainDictionary[Convert.ToInt32(sortedList[order_index]["box_id"]) - 1][i]["ispacked"] = "lost";
                        BackUp("next_box", elem[i]);
                        lost_items.Content = (Convert.ToInt32(lost_items.Content) + 1).ToString();
                    }
                }
            }
            else
            {
                for (int i = 0; i < allitems_cnt; i++)
                {
                    if (mainDictionary_one[i]["offer_id"] == sortedList[order_index]["offer_id"])
                    {
                        mainDictionary_one[i]["ispacked"] = "lost";
                        BackUp("next_box", mainDictionary_one[i]);
                        lost_items.Content = (Convert.ToInt32(lost_items.Content) + 1).ToString();
                    }
                }
            }

            string url = path_to_images + sortedList[order_index]["offer_id"].ToString() + ".jpg";
            try
            {
                clothes.Source = new BitmapImage(new Uri(url));
            }
            catch
            {
                try
                {
                    clothes.Source = new BitmapImage(new Uri("assets\\unknown.png", UriKind.Relative));
                }
                catch { };
            };
            }
            order_index++;
            box_Count();
            
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (selected_orders > 0)
            {
                MessageBox.Show("Aby przesunąć paczkę dokończ pakować aktualnie wybrane paczki", "Info", MessageBoxButton.OK);
                return;
            }

            if (ord_num < allpack_cnt)
            {
                ord_num++;
            }
            last_selected_pack_update();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            if (selected_orders > 0)
            {
                MessageBox.Show("Aby przesunąć paczkę dokończ pakować aktualnie wybrane paczki", "Info", MessageBoxButton.OK);
                return;
            }

            if (ord_num > 0)
            {
                ord_num--;
            }
            last_selected_pack_update();
        }

        public void ClosePage()
        {
            lost_items_frame.Visibility = Visibility.Hidden;
            Barcode.Focus();
        }

        public void Button_Click_7(object sender, RoutedEventArgs e)
        {
            if (!is_db_ready)
            {
                MessageBox.Show("Baza danych jeszcze się nie załadowała", "Info", MessageBoxButton.OK);
                return;
            }

            lost_items_frame.Visibility = Visibility.Visible;
            lost_items_frame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
            if (oneormore == true)
            {
                Page1 page1 = new Page1(mainDictionary, allpack_cnt);
                lost_items_frame.NavigationService.Navigate(page1);
            }
            else
            {
                Page1 page1 = new Page1(mainDictionary_one, allitems_cnt);
                lost_items_frame.NavigationService.Navigate(page1);
            }
        }

        public void lost_refresh()
        {
            lost_items_frame.Visibility = Visibility.Visible;
            lost_items_frame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
            if (oneormore == true)
            {
                Page1 page1 = new Page1(mainDictionary, allpack_cnt);
                lost_items_frame.NavigationService.Navigate(page1);
            }
            else
            {
                Page1 page1 = new Page1(mainDictionary_one, allitems_cnt);
                lost_items_frame.NavigationService.Navigate(page1);
            }
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            if (selected_orders > 0)
            {
                if (isProgres == false)
                {
                    StartProgressAsync();
                    isProgres = true;
                }
                else
                {
                    RestartTimer();
                }

            }

            box_Count();
            order_index = 0;
            Barcode.Focus();
            List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
            if (oneormore == true)
            {
                for (int i = ord_num - selected_orders; i < ord_num; i++)
                {
                    foreach (var element in mainDictionary[i]) //tutajjjjjjjjjjjjjj
                    {
                        if (element["ispacked"] != "packed" && element["ispacked"] != "lost")
                        {
                            list.Add(element);
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < allitems_cnt; i++)
                {
                    if (mainDictionary_one[i]["ispacked"] != "packed" && mainDictionary_one[i]["ispacked"] != "lost")
                    {
                        list.Add(mainDictionary_one[i]);
                    }
                }
            }

            List<Dictionary<string, string>> sortedList = list.OrderBy(x => int.Parse(x["findingpath_id"])).ThenBy(x => int.Parse(new string(x["item_id"].Where(char.IsDigit).ToArray()))).ToList();
            if (a_z == false)
            {

                sortedList = list.OrderByDescending(x => int.Parse(x["findingpath_id"])).ThenByDescending(x => int.Parse(new string(x["item_id"].Where(char.IsDigit).ToArray()))).ToList();
            }
            if (order_index == sortedList.Count())
            {
                box_Count();
                Item_Id.Content = "Wszystko";
                Rack_id.Content = "Spakowane";
                return;
            }

            try
            {
                prev_item_id.Content = sortedList[order_index - 1]["item_id"];
                prev_rack_id.Content = sortedList[order_index - 1]["rack_id"];
            }
            catch
            {
                prev_item_id.Content = "Pierwsza Rzecz";
                prev_rack_id.Content = "To Dopiero";
            }
            Item_Id.Content = sortedList[order_index]["item_id"];
            Rack_id.Content = sortedList[order_index]["rack_id"];
            if (sortedList[order_index]["is_return"] == "1")
            {
                return_id = Convert.ToInt64(sortedList[order_index]["offer_id"]);
                ZWROT.Visibility = Visibility.Visible;
                POKZWROT.Visibility = Visibility.Visible;
            }
            else
            {
                ZWROT.Visibility = Visibility.Hidden;
                POKZWROT.Visibility = Visibility.Hidden;
            }

            string url = path_to_images + sortedList[order_index]["offer_id"] + ".jpg";
            try
            {
                clothes.Source = new BitmapImage(new Uri(url));
            }
            catch
            {
                try
                {
                    clothes.Source = new BitmapImage(new Uri("assets\\unknown.png", UriKind.Relative));
                }
                catch { };
            };

            order_index++;

            if (selected_orders != 0)
            {
                start.Visibility = Visibility.Hidden;
            }
            isstart_clicked = true;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            lost_items_frame.Visibility = Visibility.Visible;
            lost_items_frame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
            Page2 page2 = new Page2();
            lost_items_frame.NavigationService.Navigate(page2);
        }

        public void set_lost_items_count(int lost)
        {
            lost_items.Content = lost.ToString();
            packing_stats();
        }
    }
}

