using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Clients.CalibrationModule.ViewModels;
using Clients.CalibrationModule.Windows;

namespace Clients.CalibrationModule {
    public partial class App {
        protected override void OnStartup(StartupEventArgs s) {
            base.OnStartup(s);
            MainWindow = new MainWindow();
            MainWindow.Show();
            Task.Factory.StartNew(() => {
                Thread.Sleep(1000);
            })
            .ContinueWith(task => {
                var mainViewModel = new MainViewModel();
                mainViewModel.RequestClose += (sender, e) => MainWindow.Close();
                MainWindow.DataContext = mainViewModel;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
