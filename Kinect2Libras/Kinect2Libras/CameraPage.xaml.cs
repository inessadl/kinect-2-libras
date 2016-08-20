using LightBuzz.Vitruvius;
using Microsoft.Kinect;
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
    /// Interaction logic for CameraPage.xaml
    /// </summary>
    public partial class CameraPage : Page
    {

        private int index = 0;

        KinectSensor _sensor;
        MultiSourceFrameReader _reader;
        PlayersController _playersController;

        bool _displaySkeleton;

        public CameraPage()
        {
            InitializeComponent();

            
            _sensor = KinectSensor.GetDefault();

            if (_sensor != null)    // pega o sensor que está ativo
            {
                _sensor.Open();     // "abre" o sensor

                // especifica quais streams poderão ser acessados
                _reader = _sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Depth | FrameSourceTypes.Infrared | FrameSourceTypes.Body);
                _reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;

                _playersController = new PlayersController();
                _playersController.BodyEntered += UserReporter_BodyEntered;
                _playersController.BodyLeft += UserReporter_BodyLeft;
                _playersController.Start();
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_playersController != null)
            {
                _playersController.Stop();
            }

            if (_reader != null)
            {
                _reader.Dispose();
            }

            if (_sensor != null)
            {
                _sensor.Close();
            }
        }

        // botão de navegação "voltar"
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        // botão para acessar câmera em cores
        private void Color_Click(object sender, RoutedEventArgs e)
        {
            viewer.Visualization = Visualization.Color;
        }

        // botão para acessar câmera de profundidade
        private void Depth_Click(object sender, RoutedEventArgs e)
        {
            viewer.Visualization = Visualization.Depth;
        }

        // botão para acessar câmera de infravermelho
        private void Infrared_Click(object sender, RoutedEventArgs e)
        {
            viewer.Visualization = Visualization.Infrared;
        }

        private void Skeleton_Checked(object sender, RoutedEventArgs e)
        {
            _displaySkeleton = true;
        }

        private void Skeleton_Unchecked(object sender, RoutedEventArgs e)
        {
            _displaySkeleton = false;
        }

        // dispara quando um frame de um dos tipos de dados é encontrado
        void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            var reference = e.FrameReference.AcquireFrame();

            // Color
            using (var frame = reference.ColorFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    if (viewer.Visualization == Visualization.Color)
                    {
                        viewer.Image = frame.ToBitmap();
                    }
                }
            }

            // Depth
            using (var frame = reference.DepthFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    if (viewer.Visualization == Visualization.Depth)
                    {
                        viewer.Image = frame.ToBitmap();
                    }
                }
            }

            // Infrared
            using (var frame = reference.InfraredFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    if (viewer.Visualization == Visualization.Infrared)
                    {
                        viewer.Image = frame.ToBitmap();
                    }
                }
            }

            // Body
            using (var frame = reference.BodyFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    var bodies = frame.Bodies();

                    _playersController.Update(bodies);

                    foreach (Body body in bodies)
                    {
                        if (_displaySkeleton)
                        {
                            viewer.DrawBody(body);
                        }
                    }
                }
            }
        }

        void UserReporter_BodyEntered(object sender, UsersControllerEventArgs e)
        {
            // A new user has entered the scene.
        }

        void UserReporter_BodyLeft(object sender, UsersControllerEventArgs e)
        {
            // A user has left the scene.
            viewer.Clear();
        }

        // captura a tela e salva em um arquivo .jpg
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), ++this.index+"kinect-screenshot.jpg");

            (viewer.Image as WriteableBitmap).Save(path);
        }


    }
}



