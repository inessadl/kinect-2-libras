using MathNet.Numerics.LinearAlgebra.Complex;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinect2Libras
{
    class Training
    {
        
       


        Training(string pathDataSet,float trainingRate) {
            //"...\\...\\...\\...\\...\\...\\kinect-2-libras\\Python Code\\trainingGestureAlt.py";
            var reader = new StreamReader(File.OpenRead(pathDataSet));

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(';');

                listA.Add(values[0]);
                listB.Add(values[1]);
            }

        }


    }
}
