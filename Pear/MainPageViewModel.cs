using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using PearLib;

namespace Pear
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        //private Peard _peard = new Peard();

        public MainPageViewModel()
        {
            InitializeDefaultState();
        }

        private ObservableCollection<PearDevice> _devices;
        public ObservableCollection<PearDevice> Devices
        {
            get { return _devices; }
            set
            {
                if (_devices != value)
                {
                    _devices = value;
                    RaisePropertyChanged(nameof(Devices));
                }

            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    RaisePropertyChanged(nameof(IsLoading));
                }
            }
        }

        private Visibility _visibility;
        public Visibility ProgressBarVisibility
        {
            get { return _visibility; }
            set
            {
                if (value != _visibility)
                {
                    _visibility = value;
                    RaisePropertyChanged(nameof(ProgressBarVisibility));
                }
            }
        }

        private void InitializeDefaultState()
        {
            //_peard.Connect();
            ProgressBarVisibility = Visibility.Collapsed;
            IsLoading = false;
        }

        public void RefreshDevices()
        {
            Debug.WriteLine("refreshing device list");
            ProgressBarVisibility = Visibility.Visible;
            IsLoading = true;

            //Devices = new ObservableCollection<PearDevice>(_peard.QueryDevices());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
