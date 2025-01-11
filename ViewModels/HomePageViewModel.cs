using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace WorkTimeTracker.ViewModels
{
    public class HomePageViewModel : INotifyPropertyChanged
    {
        private Stopwatch stopwatch = new();
        private DispatcherTimer timer;

        private string time;
        public string Time
        {
            get { return time; }
            set
            {
                time = value;
                OnPropertyChanged(nameof(Time));
            }
        }

        public ICommand StartTrackingCommand { get; }

        public HomePageViewModel()
        {
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += Timer_Tick;


            StartTrackingCommand = new RelayCommand(_ => StartTracking());
        }

        private void StartTracking()
        {
            timer.Start();
            stopwatch.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Time = stopwatch.Elapsed.ToString(@"hh\:mm\:ss");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
