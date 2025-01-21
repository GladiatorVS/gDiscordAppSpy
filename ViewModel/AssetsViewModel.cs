using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using gDiscordAppSpy.Model;
using static System.Net.WebRequestMethods;

namespace gDiscordAppSpy.ViewModel
{
    public class AssetsViewModel : INotifyPropertyChanged
    {
        //https://cdn.discordapp.com/app-assets/830463580553216060/830465188997365770.png?size=64

        private DiscordAppModel discordApp;

        public ObservableCollection<Asset> Assets
        {
            get { return discordApp.Assets; }
        }

        public ICommand UpdateDisocrdAppCommand;
        public ICommand BackButtonCommand { get; set; }
        public AssetsViewModel(object AppVM)
        {
            discordApp = new DiscordAppModel();
            UpdateDisocrdAppCommand = new RelayCommand(o => UpdateDiscordApp(o));
            BackButtonCommand = new RelayCommand(o => (AppVM as ApplicationViewModel).ChangePageCommand.Execute("main"));
        }

        private void UpdateDiscordApp(object o)
        {
            discordApp = o as DiscordAppModel;
            OnPropertyChanged("Assets");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
