using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;


namespace gDiscordAppSpy.Model
{
    

    public class DiscordAppModel
    {
        public DiscordAppModel()
        {
            Assets = new ObservableCollection<Asset>();
        }

        public ObservableCollection<Asset> Assets { get; set; }
        public string AppID { get; set; }



        public void LoadImages(int img_size = 64)
        {
            string imgUrl = "https://cdn.discordapp.com/app-assets/{_APPID_}/{_ASSET_}.png?size=64";

            //id to Images
            for (int i = 0; i < Assets.Count; i++)
            {
                string text = imgUrl.Replace("{_APPID_}", AppID).Replace("{_ASSET_}", Assets[i].Id);
                Assets[i].ImageUrl = text;
                //Assets[i].Image = new Image();
                //string uriString = text;
                //BitmapImage bitmapImage = new BitmapImage();
                //bitmapImage.BeginInit();
                //bitmapImage.UriSource = new Uri(uriString, UriKind.Absolute);
                //bitmapImage.EndInit();
                //Assets[i].Image.Source = bitmapImage;
            }
        }
    }
}