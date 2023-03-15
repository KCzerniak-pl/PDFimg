using System.Windows;

namespace PDFimg.Views
{
    /// <summary>
    /// Interaction logic for MainWindowView.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {
        public MainWindowView()
        {
            InitializeComponent();
        }

        private void HeaderLoaded(object sender, RoutedEventArgs e)
        {
            TitleBar.MouseLeftButtonDown += (s, e) =>
            {
                DragMove();
            };
        }
    }
}
