using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace gDiscordAppSpy.ViewModel
{
    public class InfoViewModel : INotifyPropertyChanged
    {
        private string lastError;
        public string LastError
        {
            get { return lastError; }
            set
            {
                lastError = value;
                OnPropertyChanged("LastError");
            }
        }
        public ICommand SetErrorTextCommand { get; set; }
        public ICommand BackButtonCommand { get; set; }
        //private ICommand _changePageCommand;
        public InfoViewModel(object appViewModel)
        {
            lastError = "";
            SetErrorTextCommand = new RelayCommand(o => SetErrorClick(o));
            //_changePageCommand = (appViewModel as ApplicationViewModel).ChangePageCommand;
            BackButtonCommand = new RelayCommand(o => (appViewModel as ApplicationViewModel).ChangePageCommand.Execute("main"));
        }

        

        private void SetErrorClick(object sender) => LastError = sender.ToString();

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
