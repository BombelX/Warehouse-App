using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Input;
using System.Windows.Documents;
using System.Threading.Tasks;

namespace WpfApp1
{
    /// <summary>
    /// Logika interakcji dla klasy Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {

        public string user_name = "Użytkownik";
        public Window2 window2 = new Window2();

        public Window1()
        {
            InitializeComponent();
            Barcode.Focus();
        }

        private void Magazyn1_Click(object sender, RoutedEventArgs e)
        {
            if (!window2.is_db_ready)
            {
                MessageBox.Show("Baza nie zaladowana");
                return;
            }
            this.Content = window2.Content;
            window2.BackUp("delete");
            window2.BackUp("start");
            window2.packing_stats();
            window2.lost_items_frame.Visibility = Visibility.Visible;
            window2.lost_items_frame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
            Page5 page5 = new Page5(window2.allpack_cnt, window2.batteryPercent, window2.allitems_cnt, window2.most);
            window2.lost_items_frame.NavigationService.Navigate(page5);
        }

        private void Barcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                user_name = Barcode.Text;
                welcome_screen.Visibility = Visibility.Hidden;
                welcome_textblock.Text = "Witaj: " + user_name + "!";
                window2.user_name = user_name;
                e.Handled = true;
            }
        }

        private void Barcode_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Barcode.Focus();
        }

        private async void Kopia_Click(object sender, RoutedEventArgs e)
        {
            if (!window2.is_db_ready)
            {
                MessageBox.Show("Baza nie zaladowana");
                return;
            }
            int lost_cnt = 0;
            int sel_ord = 0;
            int ord_num = 0;
            string connectionString = "Data Source=backdb.db;Version=3;";
            await Task.Run(() =>
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string sql = $"SELECT * FROM maindirectory WHERE `is_packed` = 'lost' OR `is_packed` = 'packed';";
                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        using (SQLiteDataReader reader = (SQLiteDataReader)command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                for (int i = 0; i < window2.allpack_cnt; i++)
                                {
                                    foreach (var element in window2.mainDictionary[i])
                                    {
                                        if (element["offer_id"] == reader.GetString(1))
                                        {
                                            if (reader.GetString(9) == "lost")
                                            {
                                                lost_cnt++;
                                            }

                                            element["ispacked"] = reader.GetString(9);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    sql = $"SELECT * FROM config;";
                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        using (SQLiteDataReader reader = (SQLiteDataReader)command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                window2.a_z = Convert.ToBoolean(Convert.ToInt16(reader.GetInt32(0)));
                                window2.oneormore = Convert.ToBoolean(Convert.ToInt16(reader.GetInt32(1)));
                                window2.user_name = reader.GetString(2);
                                window2.selected_depot = reader.GetString(3);
                            }
                        }
                    }
                    sql = $"SELECT * FROM stats;";
                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        using (SQLiteDataReader reader = (SQLiteDataReader)command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                window2.pack_countdown = Convert.ToInt32(reader.GetString(0));
                                window2.how_manytimes_in_toilet = Convert.ToInt32(reader.GetString(2));
                                window2.time_in_toilet = Convert.ToInt32(reader.GetString(3));
                                window2.how_manytimes_on_break = Convert.ToInt32(reader.GetString(4));
                                window2.allbreak_time = Convert.ToInt32(reader.GetString(5));
                                ord_num = Convert.ToInt32(reader.GetString(6));
                                sel_ord = Convert.ToInt32(reader.GetString(7));
                                window2.packed = Convert.ToInt32(reader.GetString(8));
                                window2.allitemspacked_cnt = Convert.ToInt32(reader.GetString(9));
                            }
                        }
                    }
                }

            });
            int box_inx = ord_num - sel_ord;
            window2.ord_num = box_inx;
            for (int i = 0; i < sel_ord; i++)
            {
                window2.add_package.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
            }
            window2.set_lost_items_count(lost_cnt);
            window2.box_Count();
            backup_recovered.Fill = System.Windows.Media.Brushes.Green;
            window2.change_buttons_enabled(true);
            window2.packing_status();
            this.Content = window2.Content;

        }
    }
}
