using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        private async void OnClick1(object sender, RoutedEventArgs e)
        {
            int i = 0;
            Nieznaleziono.Background = Brushes.Pink;
            while (i < 45)
            {
                progres.Value = i * (100 / 45);
                await Task.Delay(200);

                i++;


            }
        }
    }

}
