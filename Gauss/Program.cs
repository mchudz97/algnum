using System;
using System.Threading;

namespace Gauss
{
    class Program
    {
        
        static void Main(string[] args)
        {

            int size = 100;
            MatrixGenerator gen = new MatrixGenerator(size);

            /*for (int i = 100; i <= size; i+=100)
            {
                
                Matrix<float> a1 = new Matrix<float>(i, gen.A, gen.X);
                
                Matrix<float> a2 = new Matrix<float>(i, gen.A, gen.X);
                Matrix<float> a3 = new Matrix<float>(i, gen.A, gen.X);

                Matrix<double> a4 = new Matrix<double>(i, gen.A, gen.X);
                Matrix<double> a5 = new Matrix<double>(i, gen.A, gen.X);
                Matrix<double> a6 = new Matrix<double>(i, gen.A, gen.X);

                a1.Exec(Matrix<float>.Choices.G,"t1.csv", "n1.csv" );
                a2.Exec(Matrix<float>.Choices.PG, "t2.csv", "n2.csv");
                a3.Exec(Matrix<float>.Choices.FG, "t3.csv", "n3.csv");
                a4.Exec(Matrix<double>.Choices.G, "t4.csv", "n4.csv");
                a5.Exec(Matrix<double>.Choices.PG, "t5.csv", "n5.csv");
                a6.Exec(Matrix<double>.Choices.FG, "t6.csv", "n6.csv");
            }*/
            Matrix<ModVar> a7 = new Matrix<ModVar>(size, gen.A, gen.X);
            Matrix<ModVar> a8 = new Matrix<ModVar>(size, gen.A, gen.X);
            Matrix<ModVar> a9 = new Matrix<ModVar>(size, gen.A, gen.X);
            a7.Exec(Matrix<ModVar>.Choices.G, "t7.csv", "n7.csv");
            a8.Exec(Matrix<ModVar>.Choices.PG, "t8.csv", "n8.csv");
            a9.Exec(Matrix<ModVar>.Choices.FG,"t9.csv", "n9.csv");

        }
    }
}
