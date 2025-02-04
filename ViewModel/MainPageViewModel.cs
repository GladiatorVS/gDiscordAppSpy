﻿using System;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using gDiscordAppSpy.Model;
using Newtonsoft.Json;

namespace gDiscordAppSpy.ViewModel
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private string dataText;
        public string DataText
        {
            get { return dataText; }
            set
            {
                dataText = value;
                OnPropertyChanged(nameof(DataText));
            }
        }
        public ICommand GetAssetsCommand { get; set; }

        private ICommand _processDataCommand;
        public MainPageViewModel(ApplicationViewModel appVM)
        {
            _processDataCommand = appVM.ProcessDataCommand;
            GetAssetsCommand = new RelayCommand(o => GetAssetsClick(""));
        }

        private void GetAssetsClick(object sender)
        {
            string id = dataText;
            string pattern = @"^(https://)?cdn\.discordapp\.com/app-assets/(\d+)/\d+\.png$"; //asset url pattern

            if (Regex.IsMatch(dataText, pattern))
            {
                Match match = Regex.Match(dataText, pattern);
                string text = match.Groups[2].Value;
                id = text;
            }

            _processDataCommand.Execute(id);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}