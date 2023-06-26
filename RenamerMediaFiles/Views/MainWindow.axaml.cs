using Avalonia.Controls;

namespace RenamerMediaFiles.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            // Application.Current.DispatcherUnhandledException+= CurrentOnDispatcherUnhandledException;
        }

        // private void CurrentOnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        // {
        //     Console.WriteLine(e.Exception.ToString());
        //     MessageBox.Show(e.Exception.Message, Application.Current.MainWindow?.Title, MessageBoxButton.OK, MessageBoxImage.Error);
        // }
        //
        // protected override void OnClosing(CancelEventArgs e)
        // {
        //     if(Application.Current != null)
        //         Application.Current.DispatcherUnhandledException -= CurrentOnDispatcherUnhandledException;
        // }
    }
}