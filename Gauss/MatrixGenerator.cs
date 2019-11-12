using System;
using System.Collections.Generic;
using System.Text;

namespace Gauss
{
    class MatrixGenerator
    {
        Random rnd;
        int red;
        int size;
        public int[] X;
        public int[][] A;

        public MatrixGenerator(int size)
        {
            this.size = size;
            this.rnd = new Random();
            this.red = (int)Math.Pow(2, 16);
            X = new int[this.size];
            A = new int[this.size][];
            for (int row = 0; row < size; row++)
            {
                A[row] = new int[size];
                
            }
            generateAX();
        }
        private void generateAX()
        {
            for (int i = 0; i < this.size; i++)
            {
                for (int j = 0; j < this.size; j++)
                {
                    this.A[i][j]=rnd.Next(-(red), red - 1);
                }
                this.X[i] = rnd.Next(-(red), red - 1);
            }
        }
        /*public ModVar[][] generateA()
        {
            ModVar[][] temp= new 
            for (int i = 0; i < this.size; i++)
            {
                for (int j = 0; j < this.size; j++)
                {
                    this.A[i][j] = rnd.Next(-(red), red - 1);
                }
                this.X[i] = rnd.Next(-(red), red - 1);
            }
        }*/

    }
}
