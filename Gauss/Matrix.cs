using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace Gauss
{
    class Matrix<T>
    {
        public enum Choices { G, PG, FG };
        private int size;
        ArrayList swaps;
        private int red;
        public T[][] MatA { get; set; }
        public T[] MatX { get; set; }
        public T[] MatX2 { get; set; }
        public T[] MatB { get; set; }
        public int[] Xmemory;
        

        public Matrix(int size, int[][] t1, int[] t2)
        {
            this.size = size;
            MatA = new T[size][];
            MatB = new T[size];
            MatX = new T[size];
            Xmemory = new int[size];
            red = (int)Math.Pow(2, 16);
            for (int col = 0; col < this.size; col++)
            {
                MatA[col] = new T[this.size];
                for (int i = 0; i < this.size; i++)
                {
                    MatA[col][i] = this.Convert(t1[col][i]);
                }
                MatX[col] = this.Convert(t2[col]);
                Xmemory[col] = col;
            }
            swaps = new ArrayList(0);
            this.generateB();
        }
        public void exec(Choices a)
        {
            this.ToFile(this.MatA);
            this.ToFile(this.MatX);
            T[][] t1 = this.MatA;
            T[] t2 = this.MatB;
            switch (a)
            {
                case Choices.G:
                    G(t1, t2);
                    break;
                case Choices.PG:
                    PG(t1, t2);
                    break;
                case Choices.FG:
                    FG(t1, t2);
                    break;
                default:
                    break;
            }
           

        }
        private void G(T[][] tab1, T[] tab2)
        {
            GaussG(tab1, tab2);
            this.ToFile(this.Reverse(this.Eliminate(tab1, tab2)));

        }
        private void PG(T[][] tab1,T[] tab2)
        {
            GaussPG(tab1, tab2);

            this.ToFile(this.Reverse(this.Eliminate(tab1, tab2)));
        }
        private void FG(T[][] tab1, T[] tab2)
        {
            GaussFG(tab1, tab2);
            
            T[] tab = this.Reverse(this.Eliminate(tab1, tab2));
            T[] temp = new T[this.size];
            /*foreach (Tuple<int, int> tuple in this.swaps)
            {
                T temp = tab[tuple.Item1];
                tab[tuple.Item1] = tab[tuple.Item2];
                tab[tuple.Item2] = temp;
            }*/
            for (int i = 0; i < this.size; i++)
            {
                temp[Xmemory[i]] = tab[i];
            }
            this.ToFile(tab);
            
        }
        public T[] Eliminate(T[][] tab1, T[] tab2)
        {
            T[] x= new T[this.size];
            for (int i = this.size-1; i>=0; i--)
            {
                for (int j = this.size-1; j > i; j--)
                {
                    tab2[i] -= (dynamic)tab1[j][i] * x[this.size-1-j];
                }
                x[this.size-1-i] = (dynamic)tab2[i] / tab1[i][i];
            }
            return x;
        }
        public void GaussG(T[][] tabA, T[] tabB)
        {
            for (int i = 0; i < this.size-1; i++)
            {
                ResetColumn(i, tabA, tabB);
            }
        }
        public void GaussPG(T[][] tabA, T[] tabB)
        {
            for (int i = 0; i <this.size; i++)
            {
                Point p = this.getBiggestPG(tabA, i);
                RowSwap(tabA, tabB, i, p.Y);
                ResetColumn(i, tabA, tabB);
            }
        }
        public void GaussFG(T[][] tabA, T[] tabB)
        {
            

            for (int i=0;i<this.size; i++)
            {
                Point p = this.getBiggestFG(tabA, i);
                
                RowSwap(tabA, tabB, i, p.Y);
                ColumnSwap(tabA, i, p.X);
                ResetColumn(i, tabA, tabB);
            }
        }
        public void RowSwap(T[][] tab1, T[] tab2, int i, int j)
        {
            if (i == j) return;
            for (int k = i; k < this.size; k++)
            {
                T temp = tab1[k][i];
                tab1[k][i] = tab1[k][j];
                tab1[k][j] = temp;
            }
            T temp2 = tab2[i];
            tab2[i] = tab2[j];
            tab2[j] = temp2;
        }
        public void ColumnSwap(T[][] tab, int i, int j)
        {
            if (i == j) return;
            int tempX = Xmemory[i];
            Xmemory[i] = Xmemory[j];
            Xmemory[j] = tempX;
            /*T[] temp = tab[i];
            tab[i] = tab[j];
            tab[j] = temp;*/
            for (int k = 0; k < this.size; k++)
            {
                T temp = tab[i][k];
                tab[i][k] = tab[j][k];
                tab[j][k] = temp;
            }
            swaps.Add(Tuple.Create(i, j));
            

        }

        private Point getBiggestFG(T[][] tab, int p) //FG
        {
            Point point = new Point(p, p);
            T val = tab[p][p];
            for (int i = p; i < this.size; i++)
            {
                for (int j = p; j < this.size; j++)
                {
                    if (ABScheck(tab[i][j],(dynamic)val))
                    {
                        val = tab[i][j];
                        point.X = i;
                        point.Y = j;
                    }
                }
            }
            return point;
        }
        private Point getBiggestPG(T[][] tab, int p)
        {
            Point point = new Point(p, p);
            T val = tab[p][p];
            for (int i = p; i < this.size; i++)
            {
                if (ABScheck(tab[p][i], (dynamic)val))
                {
                    val = tab[p][i];
                    point.Y = i;
                }
            }
            return point;
        }

        private void ResetColumn(int i, T[][] t1, T[] t2)
        {
            
                T resetVal = t1[i][i];
                for (int j = i+1; j < this.size; j++) //j jest nastepna wartoscia w kolumnnie pod przekatna
                {
                    T check = (dynamic)t1[i][j] / resetVal; // wyznaczamy iloczyn zerujacy
                    for (int k = i; k < this.size; k++)// jedziemy po wierszu
                    {
                        t1[k][j] = t1[k][j]-(dynamic)t1[k][i] * (dynamic)check; 
                    
                    }
                
                    t2[j] = t2[j] - ((dynamic)t2[i]* (dynamic)check); // t2 macierz B 
                
                }
            
        }
        
        
        private dynamic Convert(int val)
        {
            if (typeof(T) == typeof(float))
            {
                return (float)val / (float)red;
            }
            if (typeof(T) == typeof(double))
            {
                return (double)val / (double)red;
            }
            if (typeof(T) == typeof(ModVar))
            {
                return new ModVar(val, red);
            }
            return 0;
        }
        private bool ABScheck(dynamic a, dynamic b)
        {
            if (typeof(T) == typeof(ModVar))
            {
                return (dynamic)a>b;
            }
            return Math.Abs(a) > Math.Abs(b);
            
        }
        private T[] Reverse(T[] t)
        {
            T[] temp = new T[this.size];
            for(int i = this.size - 1; i >= 0; i--)
            {
                temp[this.size-1 - i] = t[i];
            }
            return temp;
        }
        private dynamic CreateZero()
        {
            if (typeof(T) == typeof(ModVar))
            {
                return new ModVar(0, 0);
            }
            return 0;
        }
        private void generateB() 
        {
            for (int i = 0; i < this.size; i++)
            {
                T num = CreateZero();  // startowa wartosc 0
                for (int j = 0; j < this.size; j++)
                {
                    dynamic temp = MatA[j][i];
                    temp = temp * MatX[j];
                    num = num + temp;
                }
                 MatB[i] = num;
                

            }
        }
        
        public void ToFile(T[] tab)
        {
            StreamWriter file = File.AppendText("results.txt");
            for (int  i = 0;  i < this.size;  i++)
            {
                file.WriteLine("[ "+ tab[i]+" ]");
            }
            file.WriteLine("\n\n");
            file.Close();
        }
        public void ToFile(T[][] tab)
        {
            StreamWriter file = File.AppendText("results.txt");
            for (int i = 0; i < this.size; i++)
            {
                file.Write("[ ");
                for (int j = 0; j < this.size; j++)
                {
                    file.Write(tab[j][i] +"\t");
                }
                file.WriteLine("]\n\n");
            }
            file.Close();
        }
    }
}
