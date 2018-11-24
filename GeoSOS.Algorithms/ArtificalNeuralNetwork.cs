using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Accord.Neuro.Learning;
using Accord.Neuro;
using ZedGraph;
using GeoSOS.CommonLibrary;

namespace GeoSOS.Algorithms
{
    public class ArtificalNeuralNetwork
    {
        private  ActivationNetwork network;

        public ActivationNetwork ANNActivationNetwork
        {
            get { return network; }
        }

        public double TarinNetwork(float[][] inputs,float[][] outputs, int samplingCellsCount,  int inputNeuronsCount, 
            int outputNeuronsCount,int hiddenLayerNeuronsCount, double learningRate,
            int iterations, System.Windows.Forms.Label labelTrainTest, System.Windows.Forms.Label labelTrainningAccuracy,
            System.Windows.Forms.Label labelValidationAccuracy,ZedGraphControl zedGraphControl)
        {
            network = new ActivationNetwork(
               new SigmoidFunction(), inputNeuronsCount, hiddenLayerNeuronsCount, outputNeuronsCount);
            ParallelResilientBackpropagationLearning teacher = new ParallelResilientBackpropagationLearning(network);
            teacher.LearningRate = learningRate;

            double[][] trainInput = GeneralOpertor.GetArray(inputs, 0.8, true, inputNeuronsCount);
            double[][] testInput = GeneralOpertor.GetArray(inputs, 0.8, false, inputNeuronsCount);
            double[][] trainOutput = GeneralOpertor.GetArray(outputs, 0.8, true, outputNeuronsCount);
            double[][] testOutput = GeneralOpertor.GetArray(outputs, 0.8, false, outputNeuronsCount);

            int iteration = 0;
            double error = 1;

            zedGraphControl.GraphPane.CurveList.Clear();
            PointPairList list = new PointPairList();

            while ((error > 0.01) && (iteration <= iterations))
            {
                error = teacher.RunEpoch(trainInput, trainOutput) * 2 / samplingCellsCount;
                if (iteration == 0)
                {
                    list.Add(iteration, error);                    
                    zedGraphControl.GraphPane.AddCurve("Error", list, System.Drawing.Color.FromArgb(255,0,0));
                    zedGraphControl.GraphPane.AxisChange();
                }
                else if ((iteration % 30 == 0)||(iteration == iterations))
                {
                   zedGraphControl.GraphPane.CurveList[0].AddPoint (iteration, error);
                    zedGraphControl.GraphPane.AxisChange();
                    labelTrainTest.Text = "Iteration：" + iteration + "， Error：" + error.ToString("0.00000");
                    Application.DoEvents();
                }
                zedGraphControl.Refresh();
                iteration++;
            }

            double accuracy = GetAccuracy(trainInput, trainOutput, network);
            labelTrainningAccuracy.Text = (accuracy * 100).ToString("0.000") + " %";
            accuracy = GetAccuracy(testInput, testOutput, network);
            labelValidationAccuracy.Text = (accuracy * 100).ToString("0.000") + " %";

            return error;
        }

        private double GetAccuracy(double[][] input, double[][] output, ActivationNetwork network)
        {
            double accuracy = 0d;
            double numCorrect = 0d;
            double numWrong = 0d;
            for (int i = 0; i < output.Length; i++)
            {
                double[] t = network.Compute(input[i]);
                int maxIndex = MaxIndex(t); // which cell in yValues has largest value?

                double[] r = output[i];
                if (r[maxIndex] == 1.0) // ugly. consider AreEqual(double x, double y)
                    ++numCorrect;
                else
                    ++numWrong;
            }
            accuracy = numCorrect / (numCorrect + numWrong);
            return accuracy;
        }

        private int MaxIndex(double[] vector) // helper for Accuracy()
        {
            // index of largest value
            int bigIndex = 0;
            double biggestVal = vector[0];
            for (int i = 0; i < vector.Length; ++i)
            {
                if (vector[i] > biggestVal)
                {
                    biggestVal = vector[i]; bigIndex = i;
                }
            }
            return bigIndex;
        }
    }
}
