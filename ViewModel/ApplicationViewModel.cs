using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using gDiscordAppSpy.Model;
using Newtonsoft.Json;
using Windows.UI.WebUI;

namespace gDiscordAppSpy.ViewModel
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        private bool showLoading = false;

        public bool ShowLoading
        {
            get { return showLoading; }
            set
            {
                if (showLoading != value)
                {
                    showLoading = value;
                    OnPropertyChanged(nameof(ShowLoading));
                }
            }
        }
       
        

        private Control selectedPage;
        private string loadingStatus;

        public string LoadingStatus
        {
            get { return loadingStatus; }
            set
            {
                loadingStatus = value;
                OnPropertyChanged(nameof(LoadingStatus));
            }
        }

        public Dictionary<string,Control> Pages { get; set; }
        public Control SelectedPage
        {
            get { return selectedPage; }
            set
            {
                selectedPage = value;
                OnPropertyChanged(nameof(SelectedPage));
            }
        }

        public DiscordAppModel discordApp;

        public ICommand ChangePageCommand       { get; }
        public ICommand _setErrorTextCommand    { get; }
        public ICommand _updateDisocrdAppCommand{ get; }

        public ICommand ProcessDataCommand      { get; }

        public ICommand IsLoadingCommand        { get; }
        public ICommand SetLoadingStatus        { get; }

        public ApplicationViewModel()
        {
            Pages = new Dictionary<string, Control>();

            discordApp = new DiscordAppModel();

            ChangePageCommand = new RelayCommand(o => ChangePageClick(o));
            IsLoadingCommand = new RelayCommand(o => ShowLoading = (bool)o);
            ProcessDataCommand = new RelayCommand(o => ProcessData(o));
            SetLoadingStatus = new RelayCommand(o => LoadingStatus = o.ToString());

            InfoViewModel info_vm = new InfoViewModel(this);
            _setErrorTextCommand = info_vm.SetErrorTextCommand;

            AssetsViewModel asset_vm = new AssetsViewModel(this);
            _updateDisocrdAppCommand = asset_vm.UpdateDisocrdAppCommand;

            Pages.Add("assets", new View.AssetsPage(asset_vm));
            Pages.Add("info", new View.InfoPage(info_vm));
            Pages.Add("main", new View.MainPage(new MainPageViewModel(this)));

            selectedPage = Pages["main"];

            loadingStatus = "";
        }

        private async void ProcessData(object o)
        {
            discordApp.Assets.Clear();
            
            LoadingStatus = "getin from server";
            IsLoadingCommand.Execute(true);

            string id = o.ToString();

            string url = "https://discord.com/api/v9/oauth2/applications/{APP_ID}/assets";
            url = url.Replace("{APP_ID}", id);

            string ret = "";
            Task task = Task.Run(() => {
                WebRequest reqGET = WebRequest.Create(url);
                try
                {
                    ret = new StreamReader(reqGET.GetResponse().GetResponseStream()).ReadToEnd();
                }
                catch (Exception)
                {
                    ret = "";
                }
            });

            await task;

            IsLoadingCommand.Execute(false);

            if (ret == "")
            {
                _setErrorTextCommand.Execute("Ne udalos found application");
                ChangePageCommand.Execute("info");
                return;
            }

            discordApp.Assets = JsonConvert.DeserializeObject<ObservableCollection<Asset>>(ret);

            if (discordApp.Assets.Count == 0)
            {
                _setErrorTextCommand.Execute("Ne udalos found assets");
                ChangePageCommand.Execute("info");
                return;
            }

            discordApp.AppID = id;

            discordApp.LoadImages();

            _updateDisocrdAppCommand.Execute(discordApp);

            //to asset page
            ChangePageCommand.Execute("assets");
        }

        private void ChangePageClick(object sender)
        {
            Control control;
            Pages.TryGetValue(sender.ToString(), out control);

            if (control != null)
                SelectedPage = control;
            else
                MessageBox.Show($"[{sender.ToString()}] - this page is not exist.", "ops!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /*
                 //private bool LoadByID(string id)
                //{
                //    string url = "https://discord.com/api/v9/oauth2/applications/{APP_ID}/assets";
                //    url = url.Replace("{APP_ID}", id);

                //    string data = GET(id);

                //    if (data == "")
                //        return false;

                //    Assets = JsonConvert.DeserializeObject<ObservableCollection<Asset>>(data);

                //    return true;
                //}
         */

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
