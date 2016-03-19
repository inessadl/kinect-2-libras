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
using LightBuzz.Vitruvius.FingerTracking;
using Microsoft.Kinect;
using LightBuzz.Vitruvius;
using System.Threading;

namespace Kinect2Libras
{
    /// <summary>
    /// Interaction logic for FingerTrackingPage.xaml
    /// </summary>
    public partial class FingerTrackingPage : Page
    {
        private List<DepthPointEx> rightHandFingers = null;
        private KinectSensor _sensor = null;
        private InfraredFrameReader _infraredReader = null;
        private DepthFrameReader _depthReader = null;
        private BodyFrameReader _bodyReader = null;
        private IList<Body> _bodies;
        private Body _body;
        private bool gestureRecorded=false;

        // Create a new reference of a HandsController.
        private HandsController _handsController = null;

        public FingerTrackingPage()
        {
            InitializeComponent();

            _sensor = KinectSensor.GetDefault();

            if (_sensor != null)
            {
                _depthReader = _sensor.DepthFrameSource.OpenReader();
                _depthReader.FrameArrived += DepthReader_FrameArrived;

                _infraredReader = _sensor.InfraredFrameSource.OpenReader();
                _infraredReader.FrameArrived += InfraredReader_FrameArrived;

                _bodyReader = _sensor.BodyFrameSource.OpenReader();
                _bodyReader.FrameArrived += BodyReader_FrameArrived;
                _bodies = new Body[_sensor.BodyFrameSource.BodyCount];

                // Initialize the HandsController and subscribe to the HandsDetected event.
                _handsController = new HandsController();
                _handsController.HandsDetected += HandsController_HandsDetected;

                _sensor.Open();
            }
        }

        private void DepthReader_FrameArrived(object sender, DepthFrameArrivedEventArgs e)
        {
            canvas.Children.Clear();

            using (DepthFrame frame = e.FrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    // 2) Update the HandsController using the array (or pointer) of the depth depth data, and the tracked body.
                    using (KinectBuffer buffer = frame.LockImageBuffer())
                    {
                        _handsController.Update(buffer.UnderlyingBuffer, _body);
                    }
                }
            }
        }

        private void InfraredReader_FrameArrived(object sender, InfraredFrameArrivedEventArgs e)
        {
            using (var frame = e.FrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    camera.Source = frame.ToBitmap();
                }
            }
        }

        private void BodyReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            using (var bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    bodyFrame.GetAndRefreshBodyData(_bodies);

                    _body = _bodies.Where(b => b.IsTracked).FirstOrDefault();
                }
            }
        }

        private void HandsController_HandsDetected(object sender, HandCollection e)
        {
            // Display the results!

            if (e.HandLeft != null)
            {
                // Draw contour.
                foreach (var point in e.HandLeft.ContourDepth)
                {
                    DrawEllipse(point, Brushes.Green, 2.0);
                }

                // Draw fingers.
                foreach (var finger in e.HandLeft.Fingers)
                {
                    DrawEllipse(finger.DepthPoint, Brushes.White, 4.0);
                }
            }

            if (e.HandRight != null)
            {
                // Draw contour.
                foreach (var point in e.HandRight.ContourDepth)
                {
                    DrawEllipse(point, Brushes.Blue, 2.0);
                }

                // Draw fingers.
                foreach (var finger in e.HandRight.Fingers)
                {
                    DrawEllipse(finger.DepthPoint, Brushes.White, 4.0);
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_bodyReader != null)
            {
                _bodyReader.Dispose();
                _bodyReader = null;
            }

            if (_depthReader != null)
            {
                _depthReader.Dispose();
                _depthReader = null;
            }

            if (_infraredReader != null)
            {
                _infraredReader.Dispose();
                _infraredReader = null;
            }

            if (_sensor != null)
            {
                _sensor.Close();
                _sensor = null;
            }
        }

        private void DrawEllipse(DepthSpacePoint point, Brush brush, double radius)
        {
            Ellipse ellipse = new Ellipse
            {
                Width = radius,
                Height = radius,
                Fill = brush
            };

            canvas.Children.Add(ellipse);

            Canvas.SetLeft(ellipse, point.X - radius / 2.0);
            Canvas.SetTop(ellipse, point.Y - radius / 2.0);
        }

        // botão para voltar para a página inicial
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        /// <summary>
        /// Metodo chamado ao pressionar o botao Gravar
        /// Realiza a criacao de 2 Threads, um para a captura de gestos e o outro para evitar o congelamento total do sistema
        /// Sendo assim o primeiro thread e abortado quando 5segundos sao transcorridos e nenhum gesto e adquirido
        /// Isto e feito, pois a API Finger Tracking nao consegue continuar enviandos novos pontos da mao, ela congela nos ultimos replicando-os
        /// </summary>
        private void Record_Click(object sender, RoutedEventArgs e)
        {
            bool aborted = false;
            
            while (!aborted && !this.gestureRecorded)
            {

                Thread calculaGesto = new Thread(recordGesture);
                Thread time = new Thread(tempo);

                //Inicia os threads
                //Thread para capturaro gesto
                calculaGesto.Start();

              	//Thread para calcular o tempo descorrido  
                time.Start();

                //BusyWaiting para a thread time
                while (time.IsAlive) ;

                //Transcorridos 5segundos e a thread de captura ainda ativa, ela e abortada e uma nova tentativa e necessaria
                if (calculaGesto.IsAlive)
                {
                    calculaGesto.Abort();
                    aborted = true;
                }

            }

            //Verifica se algum gesto foi capturado
            //Os comandos escritos em Console.WriteLine devem ser substituidos por alguma mensagem no GUI
            if (!this.gestureRecorded)
            {
                Console.WriteLine("Sorry, Try Again!");

            }
            else{
            	Console.WriteLine("Gesture Recorded");
            }

            this.gestureRecorded = false;
            this.rightHandFingers = null;
        }

        /// <summary>
        /// Responsavel por capturar o gesto feito com a mão direita
        /// </summary>
        private void recordGesture() {
            List<DepthPointEx> fingers;

            while (this.rightHandFingers == null)
            {
                this.receiveHand();
            }

            this.gestureRecorded = true;

            //Monta uma string para o Python, posteriormente será feita diretamente em C#
            String aux = "[" + this.rightHandFingers[0].X.ToString() + "," + this.rightHandFingers[0].Y.ToString() + "," + this.rightHandFingers[1].X.ToString() + "," + this.rightHandFingers[1].Y.ToString() + "," + this.rightHandFingers[2].X.ToString() + "," + this.rightHandFingers[2].Y.ToString() + "," + this.rightHandFingers[3].X.ToString() + "," + this.rightHandFingers[3].Y.ToString() + "," + this.rightHandFingers[4].X.ToString() + "," + this.rightHandFingers[4].Y.ToString() + "]\n";
            System.IO.File.AppendAllText(@"fingers.txt", aux);
        }

        /// <summary>
        /// Responsavel por receber os pontos de cada dedo e verificar a integridade da lista adquirida
        /// Elimina valores discrepantes (considerado ruidos de captura)
        /// Elimina a possibilidade de inexistencia de valores na lista adquirida
        /// </summary>
        private void receiveHand() {
            bool condition = false;
            bool discrepante = false;
            
            List<DepthPointEx> fingers = null;
            while (!condition) {
                fingers = null;
                int cont = 0;
                fingers = this._handsController.getFingers();
                
                if (fingers != null)
                {
                	//Somente continuara com listas que contiverem mais de 5 dedos (meio obvio)
                    if (fingers.Count == 5)
                    {
                        float resultado = 0;

                        //Repetira essa sequencia ate que nao haja discrepancia e passar por toda contagem
                        // Caso haja discrepancia, o laco ira abortar
                        while (!discrepante && cont <4)
                        {
                            //Verifica se a variabilidade dos parâmetros em X e Y, caso algum for muito discrepante, então e considerado um ruido
                            // Entao e recalculado
                            float resultX = Math.Abs(fingers[cont].X - fingers[cont + 1].X);
                            Console.WriteLine(resultX);
                            float resultY = Math.Abs(fingers[cont].Y - fingers[cont + 1].Y);
                            Console.WriteLine(resultY);
                            
                           	//Discrepancia foi setado como valores acima de 200,ou seja, uma distancia de 200 do mapa de coordenadas
                            if (resultX > 200)
                            {
                                discrepante = true;

                            }
                            else if (resultY > 200)
                            {
                                discrepante = true;
                            }

                            cont++;
                        }

                        //Ausencia de discrepancia, entao a lista esta completa e perfeita
                        if (discrepante == false)
                        {
                            condition = true;
                        }
                    }
                }

           }

           // Atualiza a lista de dedos, com os dedos recebidos (ja verificados quanto sua integridade)
           this.rightHandFingers = fingers;
        }

        // Metodo exclusivo da Thread time, delimita tempo de espera para captura de Gesto
        static void tempo()
        {
            System.Threading.Thread.Sleep(5000); 
        }

    }
}