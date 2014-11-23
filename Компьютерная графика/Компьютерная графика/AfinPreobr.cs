using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Компьютерная_графика
{
    /// <summary>
    /// Структура для хранения координат точки из списка
    /// </summary>

    public static class AfinPreobr
    {
        /*const*/ static int N = 4;//Подумать как сделать N глобальной.
        //List<ThreeDPoint> Points = new List<ThreeDPoint>();
        static double[][] Matr;
        //double[] ThreedPoint = new double[N];
        static AfinPreobr()//Выделяем память под матрицу преобразований, считывает точки из input.txt 
        {
            Matr = new double[N][];
            for (int i = 0; i < N; ++i)
            {
                Matr[i] = new double[N];
            }
        }
        static void ClearMatr()
        {
            for (int i = 0; i < Matr.Length; ++i)
            {
                for (int j = 0; j < Matr[0].Length; ++j)
                {
                    Matr[i][j] = 0;
                }
            }
        }
        /// <summary>
        /// Перемножает считанные ранее точки на матрицу преобразования
        /// </summary>
        /// <param name="PreobrMatrix">Матрица преобразования</param>
        static void MultiplyAllPointsAndPreobr(double[][] PreobrMatrix, List<ThreeDPoint> Points)//Перемножает считанные ранее точки в List на матрицу 
        {
            ThreeDPoint tmp = new ThreeDPoint();
            for (int i = 0; i < Points.Count; ++i)
            {
                ThreeDPoint pt = (ThreeDPoint)Points[i].Clone();
                Points[i].x = pt.x * PreobrMatrix[0][0] + pt.y * PreobrMatrix[1][0] + pt.z * PreobrMatrix[2][0] + ThreeDPoint.HelpVar * PreobrMatrix[3][0];
                Points[i].y = pt.x * PreobrMatrix[0][1] + pt.y * PreobrMatrix[1][1] + pt.z * PreobrMatrix[2][1] + ThreeDPoint.HelpVar * PreobrMatrix[3][1];
                Points[i].z = pt.x * PreobrMatrix[0][2] + pt.y * PreobrMatrix[1][2] + pt.z * PreobrMatrix[2][2] + ThreeDPoint.HelpVar * PreobrMatrix[3][2];//HelpVar не меняется никогда при преобразованиях т.к. у всех матриц преобразования последний столбец 0 0 0 1. Поэтому не изменяем HelpVar
                //Points[i]= tmp;
            }
        }
        /// <summary>
        /// Поворот относительно Ox
        /// </summary>
        /// <param name="angle">Угол поворота</param>
        public static void Rx(double angle, List<ThreeDPoint> Points)
        {
            ClearMatr();
            Matr[0][0] = Matr[3][3] = 1;
            Matr[1][1] = Matr[2][2] = Math.Cos(angle);
            Matr[1][2] = Math.Sin(angle);
            Matr[2][1] = -Math.Sin(angle);
            MultiplyAllPointsAndPreobr(Matr, Points);
        }
        /// <summary>
        /// Поворот относительно Oy
        /// </summary>
        /// <param name="angle">Угол поворота</param>
        public static void Ry(double angle, List<ThreeDPoint> Points)
        {
            ClearMatr();
            Matr[0][0] = Matr[2][2] = Math.Cos(angle);
            Matr[2][0] = Math.Sin(angle);
            Matr[0][2] = -Math.Sin(angle);
            Matr[1][1] = Matr[3][3] = 1;
            MultiplyAllPointsAndPreobr(Matr, Points);
        }
        /// <summary>
        /// Поворот относительно Oz
        /// </summary>
        /// <param name="angle">Угол поворота</param>
        public static void Rz(double angle, List<ThreeDPoint> Points)
        {
            ClearMatr();
            Matr[0][0] = Matr[1][1] = Math.Cos(angle);
            Matr[0][1] = Math.Sin(angle);
            Matr[1][0] = -Math.Sin(angle);
            Matr[2][2] = Matr[3][3] = 1;
            MultiplyAllPointsAndPreobr(Matr, Points);
        }
        /// <summary>
        /// Растяжение
        /// </summary>
        /// <param name="alpha">Коэффициент растяжения по х</param>
        /// <param name="beta">Коэффициент растяжения по y</param>
        /// <param name="gamma">Коэффициент растяжения по z</param>
        public static void D(double alpha, double beta, double gamma, List<ThreeDPoint> Points)
        {
            ClearMatr();
            Matr[0][0] = alpha;
            Matr[1][1] = beta;
            Matr[2][2] = gamma;
            Matr[3][3] = 1;
            MultiplyAllPointsAndPreobr(Matr, Points);
        }
        /// <summary>
        /// Отражения относительно плоскости xОу
        /// </summary>
        public static void Mz(List<ThreeDPoint> Points)
        {
            ClearMatr();
            Matr[0][0] = Matr[1][1] = Matr[3][3] = 1;
            Matr[2][2] = -1;
            MultiplyAllPointsAndPreobr(Matr, Points);
        }
        /// <summary>
        /// Отражения относительно плоскости yOz
        /// </summary>
        public static void Mx(List<ThreeDPoint> Points)
        {
            ClearMatr();
            Matr[0][0] = -1;
            Matr[1][1] = Matr[2][2] = Matr[3][3] = 1;
            MultiplyAllPointsAndPreobr(Matr, Points);
        }
        /// <summary>
        /// Отражения относительно плоскости zOx
        /// </summary>
        public static void My(List<ThreeDPoint> Points)
        {
            ClearMatr();
            Matr[1][1] = -1;
            Matr[0][0] = Matr[2][2] = Matr[3][3] = 1;
            MultiplyAllPointsAndPreobr(Matr, Points);
        }
        /// <summary>
        /// Перенос на вектор (lambda,mu,nu,1)
        /// </summary>
        /// <param name="lambda"></param>
        /// <param name="mu"></param>
        /// <param name="nu"></param>
        public static void T(double lambda, double mu, double nu, List<ThreeDPoint> Points)
        {
            ClearMatr();
            for (int i = 0; i < Matr[0].Length; ++i)
            {
                Matr[i][i] = 1;
            }
            Matr[3][0] = lambda;
            Matr[3][1] = mu;
            Matr[3][2] = nu;
            MultiplyAllPointsAndPreobr(Matr, Points);
        }
    }
}

