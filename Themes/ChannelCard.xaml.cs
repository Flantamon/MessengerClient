using Messenger.MVVM.ViewModel;
using System.Windows;
using System.Windows.Input;

namespace Messenger.Themes
{
    /// <summary>
    /// Логика взаимодействия для ChannelCard.xaml
    /// </summary>
    public partial class ChannelCard : ResourceDictionary
    {
        public MainViewModel DataContext { get; private set; }
    }
}
