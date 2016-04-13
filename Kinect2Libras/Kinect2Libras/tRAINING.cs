using MathNet.Numerics.LinearAlgebra.Complex;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Kinect2Libras
{
    class Training
    {
        private Matrix<double> trainingSet;
        private Vector<int> answer;
       

        Training(string pathDataSet, float trainingRate)
        {
                //"...\\...\\...\\...\\...\\...\\kinect-2-libras\\Python Code\\trainingGestureAlt.py";
            var reader = new StreamReader(File.OpenRead(pathDataSet));

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(';');

               
                
             }

         }


    }

}
