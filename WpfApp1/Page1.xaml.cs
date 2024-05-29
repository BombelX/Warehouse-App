using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
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
    /// Logika interakcji dla klasy Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        private int windowIndex = 0;
        public List<string> found = new List<string>();
        public List<Dictionary<string, string>> lost = new List<Dictionary<string, string>>();

        public Page1(Dictionary<int, List<Dictionary<string, string>>> mainDictionary, int cnt)
        {
            InitializeComponent();
            Win_index();
            Barcode.Focus();
            for (int i = 0; i < cnt; i++)
            {
                foreach (var element in mainDictionary[i])
                {
                    if (element["ispacked"] == "lost")
                    {
                        lost.Add(element);
                    }
                }
            }

            Lost_Generation();

        }

        public Page1(List<Dictionary<string, string>> mainDictionary_one, int cnt)
        {
            InitializeComponent();
            Win_index();
            Barcode.Focus();
            int howmanylost = 0;
            for (int i = 0; i < cnt; i++)
            {
                if (mainDictionary_one[i]["ispacked"].ToString() == "lost")
                {
                    lost.Add(mainDictionary_one[i]);
                    howmanylost++;

                }
            }

            Lost_Generation();

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

        private void Lost_Generation()
        {
            Barcode.Focus();
            foreach (var element in lost)
            {
                Image image = new Image();
                image.MaxHeight = 300;
                image.Width = 400;

                WrapPanel wrap = new WrapPanel();
                wrap.Name = element["item_id"].Replace(".", "");

                StackPanel info_stack = new StackPanel();

                TextBlock textBlock = new TextBlock();
                textBlock.Text = element["item_id"];
                textBlock.FontSize = 30;
                textBlock.FontWeight = FontWeights.Bold;
                info_stack.Children.Add(textBlock);

                TextBlock textBlock2 = new TextBlock();
                textBlock2.Text = "Nr. Paczki: " + element["box_id"].ToString();
                textBlock2.FontSize = 18;
                info_stack.Children.Add(textBlock2);

                TextBlock textBlock3 = new TextBlock();
                textBlock3.Text = "Sekcja: " + element["section"].ToString();
                textBlock3.FontSize = 18;
                info_stack.Children.Add(textBlock3);

                if (element["is_return"].ToString() == "1")
                {
                    Button btn = new Button();
                    btn.Content = "Pokaż Zwrot";

                    string url2 = ((Window2)Application.Current.Windows[windowIndex]).path_to_images + element["offer_id"].ToString() + ".jpg";
                    try
                    {
                        ((Window2)Application.Current.Windows[windowIndex]).ZWROT_IMG.Source = new BitmapImage(new Uri(url2));
                    }
                    catch
                    {
                        try
                        {
                            ((Window2)Application.Current.Windows[windowIndex]).ZWROT_IMG.Source = new BitmapImage(new Uri("assets\\unknown.png", UriKind.Relative));
                        }
                        catch { };
                    };
                    ((Window2)Application.Current.Windows[windowIndex]).ZWROT_IMG.Visibility = Visibility.Visible;
                    info_stack.Children.Add(btn);
                } 
                

                string url = ((Window2)Application.Current.Windows[windowIndex]).path_to_images + element["offer_id"] + ".jpg";
                try
                {
                    image.Source = new BitmapImage(new Uri(url));
                    wrap.Children.Add(image);
                }
                catch
                {
                    try
                    {
                        image.Source = new BitmapImage(new Uri("assets\\unknown.png", UriKind.Relative));
                        wrap.Children.Add(image);
                    }
                    catch { };
                }

                wrap.Children.Add(info_stack);
                stack.Children.Add(wrap);

                Barcode.Focus();
            }
        }
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {
                foreach (var element in lost)
                {
                    if (Barcode.Text.ToString() == element["item_id"].ToString())
                    {
                        found.Add(element["item_id"].ToString());

                        if (((Window2)Application.Current.Windows[windowIndex]).oneormore)
                        {
                            for (int i = 0; i < ((Window2)Application.Current.Windows[windowIndex]).allpack_cnt; i++)
                            {
                                foreach (var elem in ((Window2)Application.Current.Windows[windowIndex]).mainDictionary[i])
                                {
                                    if (elem["ispacked"] == "lost" && element["item_id"] == elem["item_id"])
                                    {
                                        elem["ispacked"] = "packed";
                                        ((Window2)Application.Current.Windows[windowIndex]).packed++;
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < ((Window2)Application.Current.Windows[windowIndex]).allitems_cnt; i++)
                            {
                                if (((Window2)Application.Current.Windows[windowIndex]).mainDictionary_one[i]["ispacked"] == "lost" && ((Window2)Application.Current.Windows[windowIndex]).mainDictionary_one[i]["item_id"] == element["item_id"])
                                {
                                    ((Window2)Application.Current.Windows[windowIndex]).mainDictionary_one[i]["ispacked"] = "packed";
                                    ((Window2)Application.Current.Windows[windowIndex]).packed++;
                                }

                            }
                        }

                    }
                    for (int i = stack.Children.Count - 1; i >= 0; i--)
                    {
                        UIElement item = stack.Children[i];
                        if (item is FrameworkElement fe && fe.Name == element["item_id"].Replace(".", ""))
                        {
                            stack.Children.Remove(fe);
                            ((Window2)Application.Current.Windows[windowIndex]).ClosePage();
                            ((Window2)Application.Current.Windows[windowIndex]).lost_refresh();
                        }

                    }
                    Barcode.Text = "";
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService navigationService = NavigationService.GetNavigationService(this);
            navigationService.RemoveBackEntry();
            try
            {
                ((Window2)Application.Current.Windows[windowIndex]).box_Count();
                ((Window2)Application.Current.Windows[windowIndex]).ClosePage();
            }
            catch (Exception ex)
            {
                MessageBox.Show("A handled exception just occurred: " + ex.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
