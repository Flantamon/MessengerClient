using Messenger.HelperClasses;
using Messenger.MVVM.View;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Messenger
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            TcpClientWrapper.Connect("127.0.0.1", 1337);
        }
    }
}
