using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
    /// Logika interakcji dla klasy Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        public int windowIndex = 0;
        public Page2()
        {
            InitializeComponent();
            Win_index();
            Barcode.Focus();
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

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            Barcode.Focus();
            if (e.Key == Key.Enter)
            {
                if (((Window2)Application.Current.Windows[windowIndex]).oneormore == true)
                        { 
                for (int i = 0; i < ((Window2)Application.Current.Windows[windowIndex]).allpack_cnt; i++)
                {
                    foreach (var element in ((Window2)Application.Current.Windows[windowIndex]).mainDictionary[i])
                    {
                        if (element["item_id"] == Barcode.Text.ToString())
                        {
                            pack_num.Content = i;
                            user_name.Content = element["user_name"];
                            int itm_packed = 0;
                            int itm_cnt = 0;
                            foreach (var elem in ((Window2)Application.Current.Windows[windowIndex]).mainDictionary[i])
                            {
                                itm_cnt++;
                                if (elem["ispacked"] == "packed")
                                {
                                    itm_packed++;
                                }
                            }
                            how_much_in_pack.Content = itm_packed.ToString() + "/" + itm_cnt.ToString();
                            syg_kod.Content = element["item_id"].ToString();
                            try
                            {
                                clothe_image.Source = new BitmapImage(new Uri(((Window2)Application.Current.Windows[windowIndex]).path_to_images + element["offer_id"].ToString() + ".jpg"));
                            }
                            catch
                            {
                                try
                                {
                                    clothe_image.Source = new BitmapImage(new Uri("assets\\unknown.png", UriKind.Relative));
                                }
                                catch { };
                            };
                        }
                    }
                }
                }
                else
                {
                    for (int i = 0; i < ((Window2)Application.Current.Windows[windowIndex]).allitems_cnt; i++)
                    {
                        if (((Window2)Application.Current.Windows[windowIndex]).mainDictionary_one[i]["item_id"] == Barcode.Text.ToString())
                        {
                            pack_num.Content = i;
                            user_name.Content = ((Window2)Application.Current.Windows[windowIndex]).mainDictionary_one[i]["user_name"];

                            how_much_in_pack.Content = ((Window2)Application.Current.Windows[windowIndex]).allitemspacked_cnt.ToString() + "/" + ((Window2)Application.Current.Windows[windowIndex]).allitems_cnt.ToString();
                            syg_kod.Content = ((Window2)Application.Current.Windows[windowIndex]).mainDictionary_one[i]["item_id"].ToString();
                            try
                            {
                                clothe_image.Source = new BitmapImage(new Uri(((Window2)Application.Current.Windows[windowIndex]).path_to_images + ((Window2)Application.Current.Windows[windowIndex]).mainDictionary_one[i]["offer_id"].ToString() + ".jpg"));
                            }
                            catch
                            {
                                try
                                {
                                    clothe_image.Source = new BitmapImage(new Uri("assets\\unknown.png", UriKind.Relative));
                                }
                                catch { };
                            };
                        }
                    }
                }
                /*MessageBox.Show("dziala");*/
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService navigationService = NavigationService.GetNavigationService(this);
            navigationService.RemoveBackEntry();
            ((Window2)Application.Current.Windows[windowIndex]).box_Count();
            ((Window2)Application.Current.Windows[windowIndex]).ClosePage();
        }
    }
}
