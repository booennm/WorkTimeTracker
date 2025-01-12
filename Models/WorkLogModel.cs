using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTimeTracker.Models
{
    public class WorkLogModel
    {
        public class WorkLog : INotifyPropertyChanged
        {
            //example model:

            private DateTime date;
            private string title;
            private string duration;

            public DateTime Date
            {
                get
                {
                    return date;
                }

                set
                {
                    if (date != value)
                    {
                        date = value;
                        RaisePropertyChanged("Date");
                    }
                }
            }

            public string Title
            {
                get { return title; }

                set
                {
                    if (title != value)
                    {
                        title = value;
                        RaisePropertyChanged("Title");
                    }
                }
            }

            public string Duration
            {
                get { return duration; }

                set
                {
                    if (duration != value)
                    {
                        duration = value;
                        RaisePropertyChanged("Duration");
                    }
                }
            }

            /* example
            public string FullName
            {
                get
                {
                    return firstName + " " + lastName;
                }
            }*/

            public event PropertyChangedEventHandler PropertyChanged;

            private void RaisePropertyChanged(string property)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(property));
                }
            }
        }
    }
}
