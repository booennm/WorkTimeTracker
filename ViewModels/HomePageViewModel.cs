﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using WorkTimeTracker.Helpers;

namespace WorkTimeTracker.ViewModels
{
    public class HomePageViewModel : INotifyPropertyChanged
    {
        private Stopwatch stopwatch = new();
        private DispatcherTimer timer;

        private string time = "00:00:00";
        public string Time
        {
            get { return time; }
            set
            {
                time = value;
                OnPropertyChanged(nameof(Time));
            }
        }

        private ObservableCollection<string> workLogs = new();
        public ObservableCollection<string> WorkLogs
        {
            get { return workLogs; }
            set
            {
                workLogs = value;
                OnPropertyChanged(nameof(WorkLogs));
            }
        }

        public bool IsTracking
        {
            get { return stopwatch.IsRunning; }
        }

        public ICommand ToggleTrackingCommand { get; }

        public HomePageViewModel()
        {
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += Timer_Tick;

            ToggleTrackingCommand = new RelayCommand(_ => ToggleTracking());

            LoadWorkLogs();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Time = stopwatch.Elapsed.ToString(@"hh\:mm\:ss");
        }

        private void LoadWorkLogs()
        {
            try
            {
                WorkLogs.Clear();
                using (var reader = DatabaseHelper.GetWorkLogs())
                {
                    while (reader.Read())
                    {
                        string logEntry = $"{reader["Timestamp"]}: {reader["Duration"]}";
                        WorkLogs.Add(logEntry);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error loading worklogs", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ToggleTracking()
        {
            if(!stopwatch.IsRunning)
            {
                timer.Start();
                stopwatch.Start();
            }
            else
            {
                timer.Stop();
                stopwatch.Stop();
                if (DatabaseHelper.SaveWorkLog(Time))
                {
                    Time = "00:00:00";
                    stopwatch.Reset();
                    LoadWorkLogs();
                }
                else
                {
                    MessageBox.Show("Failed to save work log.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            OnPropertyChanged(nameof(IsTracking));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
