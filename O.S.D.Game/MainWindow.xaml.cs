using System.Windows;

namespace O.S.D.Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(IGameHub vm)
        {
            DataContext = vm;
            InitializeComponent();
        }
    }
}
