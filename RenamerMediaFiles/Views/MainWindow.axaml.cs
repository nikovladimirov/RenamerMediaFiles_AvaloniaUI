using Avalonia.Controls;

namespace RenamerMediaFiles.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }
        
        public MainWindow()
        {
            Instance = this;
            InitializeComponent();
        }
    }
}