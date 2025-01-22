using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using gDiscordAppSpy.Model;
using Microsoft.WindowsAPICodePack.Dialogs;


namespace gDiscordAppSpy.ViewModel
{
    public class AssetsViewModel : INotifyPropertyChanged
    {
        private DiscordAppModel discordApp;

        public ObservableCollection<Asset> Assets
        {
            get { return discordApp.Assets; }
        }

        public ICommand UpdateDisocrdAppCommand;
        public ICommand BackButtonCommand { get; set; }
        public ICommand SaveAllButtonCommand { get; set; }
        public ICommand ClickItemCommand { get; set; }

        //
        private ICommand _IsLoading { get; set; }
        private ICommand _LoadingStatus { get; set; }
        public AssetsViewModel(object AppVM)
        {
            discordApp = new DiscordAppModel();
            UpdateDisocrdAppCommand = new RelayCommand(o => UpdateDiscordApp(o));
            BackButtonCommand       = new RelayCommand(o => (AppVM as ApplicationViewModel).ChangePageCommand.Execute("main"));
            SaveAllButtonCommand    = new RelayCommand(o => SaveAllButtonClick(o));
            ClickItemCommand        = new RelayCommand(o => ClickItem(o));

            _IsLoading = (AppVM as ApplicationViewModel).IsLoadingCommand;
            _LoadingStatus = (AppVM as ApplicationViewModel).SetLoadingStatus;
        }

        private void ClickItem(object o)
        {
            discordApp.SaveSingleAsset((o as Asset));
        }

        private async void SaveAllButtonClick(object o)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;

            if (dialog.ShowDialog() != CommonFileDialogResult.Ok || Assets.Count == 0)
                return;

            _IsLoading.Execute(true);
            
            Task task = Task.Run(() =>
            {
                for (int i = 0; i < Assets.Count(); i++)
                {
                    _LoadingStatus.Execute($"[{i+1}/{Assets.Count()}] Downloading {Assets[i].Name}");
                    discordApp.SaveAsset(Assets[i], $"{dialog.FileName}{Path.DirectorySeparatorChar}{Assets[i].Name}.png");
                }
            });

            await task;

            _IsLoading.Execute(false);
        }

        private void UpdateDiscordApp(object o)
        {
            discordApp = o as DiscordAppModel;
            OnPropertyChanged(nameof(Assets));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
