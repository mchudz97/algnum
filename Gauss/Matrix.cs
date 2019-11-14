using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Text;

namespace Gauss
{
    class Matrix<T>
    {
        public enum Choices { G, PG, FG };
        private int size;
        private int red;
        public T[][] MatA;
        public T[] MatX;
        public T[] MatB;
        public int[] Xmemory;
        Stopwatch sw;

        public Matrix(int size, int[][] t1, int[] t2)
        {
            this.size = size;
            MatA = new T[size][];
            MatB = new T[size];
            MatX = new T[size];
            Xmemory = new int[size];
            sw = new Stopwatch();
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
            this.GenerateB();
        }
        public void Exec(Choices a, String pathnorm, String pathtime)
        {
            String path1 = "results.txt";
            /*String path2 = "timers.txt";
            String path3 = "norm.txt";*/
            /*this.ToFile(this.MatA, path3);
            this.ToFile(this.MatX, path3);*/
            T[][] t1 = this.MatA;
            T[] t2 = this.MatB;
            T[] results=new T[this.size];
            sw.Start();
            switch (a)
            { 
                case Choices.G:

                    results=G(t1, t2);
                    break;
                case Choices.PG:
                    results=PG(t1, t2);
                    break;
                case Choices.FG:
                    results=FG(t1, t2);
                    break;
                default:
                    break;
            }

            
            sw.Stop();
            results = this.Errors(results);
            this.ToFile(results, path1, a.ToString());
            StreamWriter file = File.AppendText(pathtime);
            file.WriteLine( this.size + "\t"+typeof(T).ToString() + "\t" + a.ToString() + "\t" + sw.Elapsed);
            file.Close();
            Console.WriteLine("n="+this.size+"\t"+typeof(T).ToString() + "\t" + a.ToString() + "\t" + sw.Elapsed);
            file= File.AppendText(pathnorm);
            file.WriteLine(this.size + "\t" + typeof(T).ToString() + "\t" + a.ToString() + "\t" + this.Norm(results));
            file.Close();
        }

        private T[] G(T[][] tab1, T[] tab2)
        {
            GaussG(tab1, tab2);
            return this.Reverse(this.Eliminate(tab1, tab2));

        }

        private T[] PG(T[][] tab1, T[] tab2)
        {
            GaussPG(tab1, tab2);

            return this.Reverse(this.Eliminate(tab1, tab2));
        }

        private T[] FG(T[][] tab1, T[] tab2)
        {
            GaussFG(tab1, tab2);
            
            T[] tab = Reverse(this.Eliminate(tab1, tab2));
            T[] temp = new T[this.size];
            
            for (int i = 0; i < this.size; i++)
            {
                temp[Xmemory[i]] = tab[i];
            }
            return temp;
            
        }
        private T Norm(T[] err)
        {
            T num = this.CreateZero();
            for(int i = 0; i < this.size; i++)
            {
                num = (dynamic)err[i] * err[i];
            }
            return num;
        }
        private T[] Errors(T[] tab)
        {
            T[] errors = new T[this.size];
            for (int i = 0; i < this.size; i++)
            {
                errors[i] = ABS((dynamic)tab[i]-this.MatX[i]);
            }
            return errors;
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
                Point p = this.GetBiggestPG(tabA, i);
                RowSwap(tabA, tabB, i, p.Y);
                ResetColumn(i, tabA, tabB);
            }
        }

        public void GaussFG(T[][] tabA, T[] tabB)
        {
            

            for (int i=0;i<this.size; i++)
            {
                Point p = this.GetBiggestFG(tabA, i);
                
                
                ColumnSwap(tabA, i, p.X);
                RowSwap(tabA, tabB, i, p.Y);
                ResetColumn(i, tabA, tabB);
                int tempX = Xmemory[i];
                Xmemory[i] = Xmemory[p.X];
                Xmemory[p.X] = tempX;
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
            
            /*T[] temp = tab[i];
            tab[i] = tab[j];
            tab[j] = temp;*/
            for (int k = 0; k < this.size; k++)
            {
                T temp = tab[i][k];
                tab[i][k] = tab[j][k];
                tab[j][k] = temp;
            }
            
            

        }

        private Point GetBiggestFG(T[][] tab, int p) //FG
        {
            Point point = new Point(p, p);
            T val = tab[p][p];
            for (int i = p; i < this.size; i++)
            {
                for (int j = p; j < this.size; j++)
                {
                    if (ABS(tab[i][j])>ABS(val))//ABScheck(tab[i][j],(dynamic)val)
                    {
                        val = tab[i][j];
                        point.X = i;
                        point.Y = j;
                    }
                }
            }
            return point;
        }

        private Point GetBiggestPG(T[][] tab, int p)
        {
            Point point = new Point(p, p);
            T val = tab[p][p];
            for (int i = p; i < this.size; i++)
            {
                if (ABS(tab[p][i]) > ABS(val))
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
        private dynamic ABS(T a)
        {
            if(typeof(T)== typeof(ModVar))
            {
                ModVar m= new ModVar(0, 0);
                if (a < (dynamic)m)
                {
                    ModVar m2 = new ModVar(-1, 1);
                    return a * (dynamic)m2;
                }
                return a;
            }
            return Math.Abs((dynamic)a);
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
        
        private void GenerateB() 
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
        public void ToFile(T[] tab,String path, String method)
        {
            StreamWriter file = File.AppendText(path);
            file.WriteLine(method);
            file.Close();
            this.ToFile(tab, path);
            

        }
        public void ToFile(T[] tab, String path)
        {
            StreamWriter file = File.AppendText(path);
            for (int  i = 0;  i < this.size;  i++)
            {
                file.WriteLine( tab[i]);
            }
            file.WriteLine("\n\n");
            file.Close();
        }
        public void ToFile(T[][] tab, String path)
        {
            StreamWriter file = File.AppendText(path);
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
