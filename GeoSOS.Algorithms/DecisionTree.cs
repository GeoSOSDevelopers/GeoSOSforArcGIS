using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Accord.MachineLearning.DecisionTrees;
using Accord.MachineLearning.DecisionTrees.Learning;
using Accord.Controls;
using Accord.Statistics.Analysis;

namespace GeoSOS.Algorithms
{
    public class DecisionTreeGenerator
    {
        public DecisionTree GenerateDecisionTree(int inputsCount,ref double[][] inputs,ref int[] outputs,
            int outputClassNum,List<string> listVariablesName,int neiWindowSize, int landuseTypesCount)
        {
            DecisionVariable[] variable = new DecisionVariable[inputsCount];
            
            for(int i=0;i<inputsCount-2;i++) 
            {
                DecisionVariable v = new DecisionVariable (listVariablesName[i],DecisionVariableKind.Continuous);
                variable[i] = v;
            };
            DecisionVariable dv = new DecisionVariable(listVariablesName[inputsCount - 2], neiWindowSize * neiWindowSize + 1);
            variable[inputsCount - 2] = dv;
            DecisionVariable dv2 = new DecisionVariable(listVariablesName[inputsCount - 1], landuseTypesCount);
            variable[inputsCount - 1] = dv2;

            DecisionTree tree = new DecisionTree(variable, outputClassNum);
            C45Learning c45 = new C45Learning(tree);

            //double error = c45.Run(inputs, outputs);
            tree = c45.Learn(inputs, outputs);

            return tree;
        }

        public ConfusionMatrix GetDecisionTreeAccuracy(DecisionTree tree, ref double[][] inputs, ref int[] outputs)
        {
            int[] actual = new int[inputs.Length];
            for (int i = 0; i < inputs.Length; i++)
            {
                actual[i] = tree.Compute(inputs[i]);
            }

            ConfusionMatrix confusionMatrix = new ConfusionMatrix(actual, outputs, 1, 0);
            return confusionMatrix;
        }

        public void DataPrepare(float[,] datas, int rowCount, int columnCount, ref float[][] inputs, ref int[] outputs)
        {            
            float[] inputRowsValueArray;
            for (int i = 0; i < rowCount; i++)
            {
                inputRowsValueArray = new float[columnCount - 1];
                for (int j = 0; j < inputRowsValueArray.Length; j++)
                {
                    inputRowsValueArray[j] = Convert.ToSingle(datas[i,j]);
                }
                inputs[i] = inputRowsValueArray;
                outputs[i] = Convert.ToInt32(datas[i, columnCount - 1]);
            }
        }
    }
}
