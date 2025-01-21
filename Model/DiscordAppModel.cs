using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace gDiscordAppSpy.Model
{
    public class DiscordAppModel
    {
        
        //public struct Asset
        //{
        //    public string id;
        //    public int type;
        //    public string name;
        //}

        

        //public bool LoadAssets(string data)
        //{
        //    //
            
        //    return LoadByID(data);
        //}

        //private string GET(string url)
        //{
        //    WebRequest reqGET = WebRequest.Create(url);
        //    try
        //    {
        //        return new StreamReader(reqGET.GetResponse().GetResponseStream()).ReadToEnd();
        //    }
        //    catch (Exception)
        //    {
        //        return "";
        //    }
        //}

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
    }
}
