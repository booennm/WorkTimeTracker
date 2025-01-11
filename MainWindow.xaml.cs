using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;
using WorkTimeTracker.ViewModels;

namespace WorkTimeTracker
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void HomePageViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            WorkTimeTracker.ViewModels.HomePageViewModel homePageVM = new();
            HomePageViewControl.DataContext = homePageVM;
        }
    }
}