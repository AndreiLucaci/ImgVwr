using System.Linq;
using System.Windows;

namespace ImgVwr
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var currentFile = e.Args.SingleOrDefault();

            var mainWindow = new MainWindow(currentFile);

            mainWindow.Show();
        }
    }
}
