using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LightBuzz.Vitruvius.FingerTracking;
using Microsoft.Kinect;
using LightBuzz.Vitruvius;
using System.Threading;
using LibSVMsharp.Helpers;
using LibSVMsharp;
using LibSVMsharp.Core;
using LibSVMsharp.Extensions;
using System.Text;
using System.Threading.Tasks;
//using MathNet.Numerics.LinearAlgebra;

namespace Kinect2Libras
{
    /// <summary>
    /// Interaction logic for FingerTrackingPage.xaml
    /// </summary>
    public partial class FingerTrackingPage : Page
    {
        private Thread backgroundRecognition;
        private List<DepthPointEx> rightHandFingers = null;
        private KinectSensor _sensor = null;
        private InfraredFrameReader _infraredReader = null;
        private DepthFrameReader _depthReader = null;
        private BodyFrameReader _bodyReader = null;
        private IList<Body> _bodies;
        private Body _body;
        private bool gestureRecorded=false;
        //private SVMModel gestureModel;


        // Create a new reference of a HandsController.
        private HandsController _handsController = null;

        public FingerTrackingPage()
        {
            
            InitializeComponent();
            //this.gestureModel = SVM.LoadModel(@"..\..\models\gesture.txt");
            //invoca a thread que tomara conta do method rodando em background
            this.backgroundRecognition = new Thread(this.backgroundGestureRecognition);
            

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

            this.backgroundRecognition.Start();
        }

        /*
        private void TrainingModel(object sender, RoutedEventArgs e) {
            // Load the datasets: In this example I use the same datasets for training and testing which is not suggested
            SVMProblem trainingSet = SVMProblemHelper.Load(@"..\..\database\gestureDataSet.txt");
            SVMProblem testSet = SVMProblemHelper.Load(@"..\..\database\test.txt");
            Console.WriteLine(trainingSet.Length);

            // Normalize the datasets if you want: L2 Norm => x / ||x||
            //trainingSet = trainingSet.Normalize(SVMNormType.L2);
            //testSet = testSet.Normalize(SVMNormType.L2);

            // Select the parameter set
            double inGamma = 1 / trainingSet.Length;
            SVMParameter parameter = new SVMParameter();
            parameter.Type = SVMType.C_SVC;
            parameter.Kernel = SVMKernelType.LINEAR;
            parameter.C = 5;
            parameter.Gamma = inGamma;

            // Do cross validation to check this parameter set is correct for the dataset or not
            double[] crossValidationResults; // output labels
            int nFold = 10;
            trainingSet.CrossValidation(parameter, nFold, out crossValidationResults);
            
            // Evaluate the cross validation result
            // If it is not good enough, select the parameter set again
            double crossValidationAccuracy = trainingSet.EvaluateClassificationProblem(crossValidationResults);

            // Train the model, If your parameter set gives good result on cross validation
            SVMModel model = trainingSet.Train(parameter);

            // Save the model
            SVM.SaveModel(model, @"..\..\models\gesture.txt");

            // Predict the instances in the test set
            double[] testResults = testSet.Predict(model);

            // Evaluate the test results
            int[,] confusionMatrix;
            double testAccuracy = testSet.EvaluateClassificationProblem(testResults, model.Labels, out confusionMatrix);

            // Print the resutls
            Console.WriteLine("\n\nCross validation accuracy: " + crossValidationAccuracy);
            Console.WriteLine("\nTest accuracy: " + testAccuracy);
            Console.WriteLine("\nConfusion matrix:\n");

            // Print formatted confusion matrix
            Console.Write(String.Format("{0,6}", ""));
            for (int i = 0; i < model.Labels.Length; i++)
                Console.Write(String.Format("{0,5}", "(" + model.Labels[i] + ")"));
            Console.WriteLine();
            for (int i = 0; i < confusionMatrix.GetLength(0); i++)
            {
                Console.Write(String.Format("{0,5}", "(" + model.Labels[i] + ")"));
                for (int j = 0; j < confusionMatrix.GetLength(1); j++)
                    Console.Write(String.Format("{0,5}", confusionMatrix[i, j]));
                Console.WriteLine();
            }

        }*/

        //Metodo responsavel pela verificacao dos gestos realizados. Todos os gestos são captuados e verificados por threads separados do sistema
        private void backgroundGestureRecognition() {

            while (true) {

                //responsavel por capturar o gesto, alem de testa-lo quanto sua integridade (se capturou os 5 dedos corretamente, se pelo menos 1 dos 5 pontos está em desacordo com o normal)
                //while (this.rightHandFingers == null)
                //{    
                //    this.receiveHand();
                //}

                SVMNode[] fingers = new SVMNode[20];

                

                fingers[0] = new SVMNode(1, 0.6);
                fingers[1] = new SVMNode(2, -23.71);
                fingers[2] = new SVMNode(3, 17.6);
                fingers[3] = new SVMNode(4, -14.71);
                fingers[4] = new SVMNode(5, 17.6);
                fingers[5] = new SVMNode(6, 0.29);
                fingers[6] = new SVMNode(7, -17.4);
                fingers[7] = new SVMNode(8, 31.29);
                fingers[8] = new SVMNode(9, -16.4);
                fingers[9] = new SVMNode(10, -12.71);
                fingers[10] = new SVMNode(11, 0.3);
                fingers[11] = new SVMNode(12, -11.86);
                fingers[12] = new SVMNode(13, 12.1);
                fingers[13] = new SVMNode(14, -7.36);
                fingers[14] = new SVMNode(15, 12.1);
                fingers[15] = new SVMNode(16, 0.14);
                fingers[16] = new SVMNode(17, -8.7);
                fingers[17] = new SVMNode(18, 24.98);
                fingers[18] = new SVMNode(19, -11.26);
                fingers[19] = new SVMNode(20, -6.36);

                //int k = 0;
                //for (int i = 0; i < 10; i++) {
                //    if (i != 5)
                //    {
                //        fingers[k] = new SVMNode(k+1, this.rightHandFingers[i].X - this.rightHandFingers[5].X);
                //        k++;
                //        fingers[k] = new SVMNode(k+1, this.rightHandFingers[i].Y - this.rightHandFingers[5].Y);
                //        k++;
                //    }

                //}
                this.rightHandFingers = null;
                Console.WriteLine(gestureModel.Predict(fingers));
                //gestureForm.Text = Convert.ToString(gestureModel.Predict(fingers));

                //Serve para dispachar um thread adicional somente para alteração do elementos da interface
                Dispatcher.Invoke(() => { gestureForm.Text = Convert.ToString(gestureModel.Predict(fingers)); }); 

                //serve para inibir pegar ruídos, ou ficar 'randomizando' suas decisões por não capturar o gesto corretamente
                Thread.Sleep(2000);

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
                if (calculaGesto.IsAlive&& this.gestureRecorded ==false)
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
            
            int answer = Convert.ToInt32(this.nameGesture);

            String gesture = this.nameGesture + " ";
            for (int i = 0; i < 10; i++) {
                if (i != 5) {
                    int k = i + 1;
                    double aux1 = this.rightHandFingers[i].X - this.rightHandFingers[5].X;
                    double aux2 = this.rightHandFingers[i].Y - this.rightHandFingers[5].Y;
                    gesture+= k + ":" + aux1 + " " + (k + 1) + ":" + aux2 + " ";
                }
            }
            //String aux = "[" + this.rightHandFingers[0].X + "-" + this.rightHandFingers[0].Y + "-" + this.rightHandFingers[1].X 
            //    + "-" + this.rightHandFingers[1].Y + "-" + this.rightHandFingers[2].X + "-" + this.rightHandFingers[2].Y + "-" 
            //    + this.rightHandFingers[3].X +"-" + this.rightHandFingers[3].Y + "-" + this.rightHandFingers[4].X + "-"
            //    + this.rightHandFingers[4].Y +"-"+ this.rightHandFingers[5].X +"-"+ this.rightHandFingers[5].Y + "-" +
            //    this.rightHandFingers[6].X + "-" + this.rightHandFingers[6].Y + "-" +
            //    this.rightHandFingers[7].X + "-" + this.rightHandFingers[7].Y + "-" +
            //    this.rightHandFingers[8].X + "-" + this.rightHandFingers[8].Y + "-" +
            //    this.rightHandFingers[9].X + "-" + this.rightHandFingers[9].Y + "-" +
            //    this.rightHandFingers[10].X + "-" + this.rightHandFingers[10].Y + "-" + answer + "]\n";

            System.IO.File.AppendAllText(@"..\..\database\gestureDataSet.txt", gesture);
            //System.IO.File.AppendAllText(@"C:\Users\Lucas Tortelli\Desktop\FingerTracking\New\kinect-2-libras\Recorded\fingers.txt", aux);
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
                    
                    
                    if (fingers.Count == 11)
                    {
                        float resultado = 0;

                        //Repetira essa sequencia ate que nao haja discrepancia e passar por toda contagem
                        // Caso haja discrepancia, o laco ira abortar
                        while (!discrepante && cont <5)
                        {
                            //Verifica se a variabilidade dos parâmetros em X e Y, caso algum for muito discrepante, então e considerado um ruido
                            // Entao e recalculado
                            Decimal resultX = Math.Abs((Decimal)fingers[cont].X - (Decimal)fingers[cont + 1].X);
                            //Console.WriteLine(resultX);
                            Decimal resultY = Math.Abs((Decimal)fingers[cont].Y - (Decimal)fingers[cont + 1].Y);
                            Console.WriteLine(fingers[cont].Y+"-"+fingers[cont + 1].Y+" = "+resultY);
                            
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
