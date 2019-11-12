using System;

namespace Gauss
{
    class Program
    {
        
        static void Main(string[] args)
        {
            MatrixGenerator gen = new MatrixGenerator(5);
            Matrix<ModVar> a = new Matrix<ModVar>(5, gen.A, gen.X);
            a.exec(Matrix<ModVar>.Choices.PG);
            Matrix<ModVar> a1 = new Matrix<ModVar>(5, gen.A, gen.X);
            a1.exec(Matrix<ModVar>.Choices.G);
            Matrix<ModVar> a2 = new Matrix<ModVar>(5, gen.A, gen.X);
            a2.exec(Matrix<ModVar>.Choices.FG);
            a2.ToFile(a2.MatA);
            /*Matrix<double> a3 = new Matrix<double>(5, gen.A, gen.X);
            a3.exec(Matrix<double>.Choices.FG);
            a3.ToFile(a3.MatA);*/
        }
    }
}
