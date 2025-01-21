using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace gDiscordAppSpy.ViewModel
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        private Control selectedPage;
        private string dataText;
        public ObservableCollection<Asset> Assets { get; set; }
        public ObservableCollection<Control> Pages { get; set; }
        public Control SelectedPage
        {
            get { return selectedPage; }
            set
            {
                selectedPage = value;
                OnPropertyChanged("SelectedPage");
            }
        }

        public string DataText
        {
            get { return dataText; }
            set
            {
                dataText = value;
                OnPropertyChanged("DataText");
            }
        }

        public ICommand GetAssetsCommand { get; set; }

        public ApplicationViewModel()
        {
            Assets = new ObservableCollection<Asset>();
            GetAssetsCommand = new RelayCommand(o => GetAssetsClick(""));

            selectedPage = new View.MainPage(this);
        }
        private void GetAssetsClick(object sender)
        {
            //MessageBox.Show(DataText);
            //detect type -> id_app,id_user,asset_url

            //Assets.Clear();

            //if the data is id_user -> (...)

            //if the data is asset_url -> convent to id_app

            string url = "https://discord.com/api/v9/oauth2/applications/{APP_ID}/assets";
            url = url.Replace("{APP_ID}", dataText);

            WebRequest reqGET = WebRequest.Create(url);
            string ret;
            try
            {
                ret = new StreamReader(reqGET.GetResponse().GetResponseStream()).ReadToEnd();
            }
            catch (Exception)
            {
                ret = "";
            }

            if (ret == "")
            {
                MessageBox.Show("Ops!");
                return;
            }

            Assets = JsonConvert.DeserializeObject<ObservableCollection<Asset>>(ret);
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
