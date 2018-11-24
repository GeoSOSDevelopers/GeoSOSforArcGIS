using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Accord.Statistics.Analysis;

namespace GeoSOS.Algorithms
{
    public class LogisticRegression
    {
        public void LogisticRegresssionAanlysis(float[,] datas,int rowCount,int columnCount, out double[] coef,
            out double[] odds, out double[] stde, out double[]min, out double[] max)
        {
            double[][] inputs = new double[rowCount][];
            double[] outputs = new double[rowCount];

            //根据给定的数据得到输入、输出数组
            SetInputAndOutputData(datas, rowCount, columnCount, ref inputs, ref outputs);

            // Create a Logistic Regression analysis
            var regression = new LogisticRegressionAnalysis();

            regression.Learn(inputs, outputs);
            //regression.Compute(); // compute the analysis.

            // We can also investigate all parameters individually. For
            // example the coefficients values will be available at the
            // vector
            coef = regression.CoefficientValues;

            // The first value refers to the model's intercept term. We
            // can also retrieve the odds ratios and standard errors:
            odds = regression.OddsRatios;
            stde = regression.StandardErrors;

            //得到置信区间
            Accord.DoubleRange[] confidence = regression.Confidences;
            min = new double[confidence.Length];
            max = new double[confidence.Length];
            for (int i = 0; i < confidence.Length; i++)
            {
                min[i] = confidence[i].Min;
                max[i] = confidence[i].Max;
            }
        }

        private void SetInputAndOutputData(float[,] sampledDatas, int rowCount, int columnCount, ref double[][] inputs, ref double[] outputs)
        { 
            double[] rowValueArray;
            for (int i = 0; i < rowCount; i++)
            {
                rowValueArray = new double[columnCount - 1];
                for (int j = 0; j < columnCount - 1; j++)
                {
                    rowValueArray[j] = Convert.ToDouble(sampledDatas[i,j]);
                }
                inputs[i] = rowValueArray;
                outputs[i] = Convert.ToDouble(sampledDatas[i,columnCount - 1]);
            }
        }
    }
}
