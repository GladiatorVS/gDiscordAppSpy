using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;


namespace gDiscordAppSpy.Model
{
    

    public class DiscordAppModel
    {
        private string ResURL = "https://cdn.discordapp.com/app-assets/{_APPID_}/{_ASSET_}.png";
        public DiscordAppModel()
        {
            Assets = new ObservableCollection<Asset>();
        }

        public ObservableCollection<Asset> Assets { get; set; }
        public string AppID { get; set; }


        //public void SaveAllAssets()
        //{
        //    CommonOpenFileDialog dialog = new CommonOpenFileDialog();
        //    dialog.IsFolderPicker = true;

        //    if (dialog.ShowDialog() != CommonFileDialogResult.Ok || Assets.Count == 0)
        //    {
        //        return;
        //    }
        //    for (int i = 0; i < Assets.Count(); i++)
        //    {
        //        SaveAsset(Assets[i], $"{dialog.FileName}{Path.DirectorySeparatorChar}{Assets[i].Name}.png");
        //    }
        //    System.Windows.MessageBox.Show($"Saved {Assets.Count()} files");
        //}

        public void SaveSingleAsset(Asset asset)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Image|.png";

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return;

            SaveAsset(asset, saveFileDialog.FileName);
        }

        public void SaveAsset(Asset asset, string saveAs)
        {
            string imgurl = asset.ImageUrl;
            imgurl = imgurl.Replace("?size=64", "");
            WebClient client = new WebClient();
            client.DownloadFile(imgurl, saveAs);
        }

        public void LoadImages(int img_size = 64)
        {
            string imgUrl = $"{ResURL}?size=64";

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