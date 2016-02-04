using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kinect2Libras
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public void FingerTracking_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new FingerTrackingPage());
        }

        public void HeadTracking_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HeadTrackingPage());
        }

        public void SimpleGesture_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SimpleGesturePage());
        }

        public void LibrasGesture_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LibrasGesturePage());
        }

        public void Tutorial_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TutorialPage());
        }

        public void About_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AboutPage());
        }


    }
}
