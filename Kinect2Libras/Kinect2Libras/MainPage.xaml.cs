using LightBuzz.Vitruvius.FingerTracking;
using Microsoft.Kinect;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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

        public void Camera_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CameraPage());
        }

        public void Tutorial_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TutorialPage());
        }

        public void About_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AboutPage());
        }

        private void Button_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }

    }
}
