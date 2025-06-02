using System.Windows;
using UPMurtazinIS121.ViewModel;

namespace UPMurtazinIS121
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new IngredientsViewModel();
        }
        private void SuppliersButton_Click(object sender, RoutedEventArgs e)
        {
            var suppliersWindow = new SuppliersWindow
            {
                Owner = this
            };
            suppliersWindow.ShowDialog();
        }
    }
}