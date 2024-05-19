using Messenger.Core;
using Messenger.HelperClasses;
using Messenger.MVVM.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Messenger.Themes
{
    /// <summary>
    /// Interaction logic for ContactCard.xaml
    /// </summary>
    public partial class ContactCard : ResourceDictionary
    {
        
        public MainViewModel DataContext { get; private set; }
    }
}
