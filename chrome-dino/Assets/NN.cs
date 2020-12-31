using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NN
{
    public int inpNodes;
    public int hidNodes;

    public double[][] weight_hi = new double[1][];
    public double[][] weight_oh = new double[1][];

    public double[][] bias_h = new double[1][];
    public double[][] bias_o = new double[1][];

    public float fitness = 0f;

    public NN()
    {

    }

    #region MATRIX
    // MATRIX THINGS FROM HERE

    private static System.Random random = new System.Random();

    private static double RandomNumberBetween(double minValue, double maxValue)
    {
        var next = random.NextDouble();

        return minValue + (next * (maxValue - minValue));
    }
    
    public static float Sigmoid(double value)
    {
        return 1.0f / (1.0f + (float) System.Math.Exp(-value));
    }

    public static double [][] MatCreate(int rows, int cols)
    {
        double[][] result = new double[rows][];
        for (int i = 0; i < rows; ++i)
            result[i] = new double[cols];
        return result;
    }

    public static double[][] randomize(double[][] matrix, int rows, int cols)
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                matrix[i][j] = RandomNumberBetween(-1, 1);
            }
        }
        return matrix;
    }

    public static double[][] Multiply(double[][] a, double[][] b, int a_rows, int a_cols, int b_rows, int b_cols)
    {
        double[][] result = NN.MatCreate(a_rows, b_cols);
        for (int i = 0; i < a_rows; i++)
        {
            for (int j = 0; j < b_cols; j++)
            {
                result[i][j] = 0;
                for (int k = 0; k < a_cols; k++)
                {
                    result[i][j] += a[i][k] * b[k][j];
                }
            }
        }
        return result;
    }

    public static double[][] Add(double[][] a, double[][] b, int a_rows, int a_cols)
    {
        double[][] result = NN.MatCreate(a_rows, a_cols);
        for (int i = 0; i < a_rows; i++)
        {
            for (int j = 0; j < a_cols; j++)
            {
                result[i][j] = a[i][j] + b[i][j];
            }
        }
        return result;
    }

    public double[][] copyMatrix(double[][] inputMatrix)
    {
        double[][] newMatrix = NN.MatCreate(inputMatrix.Length, inputMatrix[0].Length);
        for (int i = 0; i < inputMatrix.Length; i++)
        {
            for (int j = 0; j < inputMatrix[0].Length; j++)
            {
                newMatrix[i][j] = inputMatrix[i][j];
            }
        }
        return newMatrix;
    }

    // MATRIX THINGS END HERE
    #endregion

    public NN(int inpNodes, int hidNodes)
    {
        this.inpNodes = inpNodes;
        this.hidNodes = hidNodes;

        weight_hi = NN.MatCreate(hidNodes, inpNodes);
        weight_oh = NN.MatCreate(1, hidNodes);

        // randomize the both weight matrices betn -1 and 1
        weight_hi = randomize(weight_hi, hidNodes, inpNodes);
        weight_oh = randomize(weight_oh, 1, hidNodes);

        // bias matrix for hidden nodes - hidden nodes x 1
        // bias matrix for output nodes - output nodes x 1
        bias_h = NN.MatCreate(hidNodes, 1);
        bias_o = NN.MatCreate(1, 1);
        
        // randomize both bias matrix
        bias_h = randomize(bias_h, hidNodes, 1);
        bias_o = randomize(bias_o, 1, 1);
    }

    public double[][] feedForward(double[][] input) // input array dimensions -> inputNode x 1
    {
        // hidden layer matrix = multiply matrices -> weight matrix (hidden x input) and input
        double[][] hidden_matrix = NN.Multiply(weight_hi, input, hidNodes, inpNodes, inpNodes, 1);

        // hidden layer matrix -> add bias matrix for hidden nodes
        hidden_matrix = NN.Add(hidden_matrix, bias_h, hidNodes, 1);

        // activation function (sigmoid function)
        for (int i = 0; i < hidNodes; i++)
        {
            hidden_matrix[i][0] = NN.Sigmoid(hidden_matrix[i][0]);
        }



        // output layer matrix = multiply matrices -> weight matrix (output x hidden) and hidden layer matrix
        double[][] output_matrix = NN.Multiply(weight_oh, hidden_matrix, 1, hidNodes, hidNodes, 1);

        // output layer matrix -> add bias matrix for output nodes
        output_matrix = NN.Add(output_matrix, bias_o, 1, 1);

        // activation function (sigmoid function)
        output_matrix[0][0] = NN.Sigmoid(output_matrix[0][0]);

        return output_matrix;
    }

    public NN copyNN()
    {
        return new NN(this.inpNodes, this.hidNodes);
    }

    public double[][] mutateEveryElem(double[][] matrix, float mutation_rate)
    {
        double[][] newMatrix = NN.MatCreate(matrix.Length, matrix[0].Length);
        for (int i = 0; i < matrix.Length; i++)
        {
            for (int j = 0; j < matrix[0].Length; j++)
            {
                if (NN.RandomNumberBetween(0, 1) < mutation_rate)
                    newMatrix[i][j] = NN.RandomNumberBetween(-1, 1);
                else
                    newMatrix[i][j] = matrix[i][j];
            }
        }

        return newMatrix;
    }

    public void mutateNN(float mutation_rate)
    {
        this.weight_hi = mutateEveryElem(this.weight_hi, mutation_rate);
        this.weight_oh = mutateEveryElem(this.weight_oh, mutation_rate);
        this.bias_h = mutateEveryElem(this.bias_h, mutation_rate);
        this.bias_o = mutateEveryElem(this.bias_o, mutation_rate);
    }

    
}
