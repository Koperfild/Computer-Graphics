﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Компьютерная_графика
{
    public class ThreeDPoint:ICloneable
    {
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
        public const int HelpVar = 1;
        public ThreeDPoint():base()
        {
        }
        public ThreeDPoint(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    public class Edge
    {
        public bool Visible { get; set; }
        public ThreeDPoint Point1 { get; set; }
        public ThreeDPoint Point2 { get; set; }
        public Edge(ThreeDPoint Point1, ThreeDPoint Point2)
        {
            this.Visible = true;
            this.Point1 = Point1;
            this.Point2 = Point2;
        }
        /// <summary>
        /// Compares 2 edges by z derivative
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static int CompareByZ(Edge e1, Edge e2)
        {
            //Нормируем вектора граней и сравниваем по z координате
            double z1=Math.Abs(e1.Point2.z-e1.Point1.z)/(Math.Sqrt(Math.Pow(e1.Point2.x-e1.Point1.x,2)+Math.Pow(e1.Point2.y-e1.Point1.y,2)+Math.Pow(e1.Point2.z-e1.Point1.z,2)));//Можно и без Math.Sqrt Для быстродействия. Рез-т одинаковый
            double z2 = Math.Abs(e2.Point2.z - e2.Point1.z) / (Math.Sqrt(Math.Pow(e2.Point2.x - e2.Point1.x, 2) + Math.Pow(e2.Point2.y - e2.Point1.y, 2) + Math.Pow(e2.Point2.z - e2.Point1.z, 2)));
            return z2.CompareTo(z1);
        }
        /// <summary>
        /// Finds cross point of 2 edges. If 2 Edges doesn't have common point it returns null
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns>Point of crossing or null</returns>
        public static ThreeDPoint CrossPointofEdges(Edge e1,Edge e2){
            double A1, B1, A2, B2, C1, C2;
            B1 = e1.Point2.x - e1.Point1.x;
            A1 = -(e1.Point2.y - e1.Point1.y);//- because of directing vector (
            C1 = -A1 * e1.Point1.x - B1 * e1.Point1.y;

            B2 = e2.Point2.x - e2.Point1.x;
            A2 = -(e2.Point2.y - e2.Point1.y);//- because of directing vector (
            C2 = -A1 * e2.Point1.x - B1 * e2.Point1.y;
            double[,] M=new double[2,3]{
                {A1,B1,C1},
                {A2,B2,C2}
            };
            //Solving system of 2 linear equations (for each edge)
            ThreeDPoint CrossPoint = new ThreeDPoint();
            if (LinearEquationSolver.Solve(M))//Проверить изменяется ли матрица
            {
                
                CrossPoint.x = M[0, 2];
                CrossPoint.y = M[1, 2];
                //return CrossPoint;
            }
            //defining of belonging point to edge by distance from 2 points of edge. Math.Abs(e1.Point1.x - e1.Point2.x) is distance(by Ox) between 2 points of edge
            if (Math.Abs(e1.Point1.x - CrossPoint.x) < Math.Abs(e1.Point1.x - e1.Point2.x) && Math.Abs(e1.Point2.x - CrossPoint.x) < Math.Abs(e1.Point1.x - e1.Point2.x))
            {
                return CrossPoint;
            }
            else return null;
        }


    }
    public class BoundBindingException:Exception//Для использования в конструкторе Bound
    {
        public BoundBindingException(string msg):base(msg)//Наследование базового конструктора от Exception
        {
        }
    }
    /// <summary>
    /// Грань многогранника
    /// </summary>
    public class Bound:ICloneable//Грань
    {
        List<Edge> Edges = new List<Edge>();
        public List<Edge> getEdges()
        {
            return Edges;
        }
        public Edge getEdge(int i)
        {
            return Edges[i];
        }
        public void AddEdge(Edge e)
        {
            Edges.Add(e);
        }
        public Bound():base()
        {
        }
        /// <summary>
        /// Задание грани с помощью рёбер
        /// </summary>
        /// <param name="v">массив рёбер, или рёбра через запятую</param>
        /// <exception cref="BoundBindingException">Возникает если число рёбер меньше 3</exception>"
        public Bound(List<Edge> v)//Задание грани рёбрами
        {
            /*if (v.Count <= 2)
            {
                throw new BoundBindingException("Incorrect input of bound");
            }
            for (int i = 0; i < v.Count; ++i)
            {
                Edges.Add(v[i]);
            }*/
            Edges = v;
        }
        public object Clone()
        {
            Bound b = new Bound();
            for (int i = 0; i < this.Edges.Count; ++i)
            {
                ThreeDPoint pt1=new ThreeDPoint();
                pt1.x=this.Edges[i].Point1.x;
                pt1.y=this.Edges[i].Point1.y;
                pt1.z=this.Edges[i].Point1.z;
                
                ThreeDPoint pt2=new ThreeDPoint();
                pt2.x=this.Edges[i].Point2.x;
                pt2.y=this.Edges[i].Point2.y;
                pt2.z=this.Edges[i].Point2.z;
                
                Edge e=new Edge(pt1,pt2);
                b.Edges.Add(e);
            }
            return b;
            //throw new NotImplementedException();
        }
    }
    public class PolyHedron
    {
        List<ThreeDPoint> Points = new List<ThreeDPoint>();
        List<Edge> Edges = new List<Edge>();
        List<Bound> Bounds = new List<Bound>();
        public List<ThreeDPoint> getPoints()
        {
            return Points;
        }
        public List<Edge> getEdges()
        {
            return Edges;
        }
        public List<Bound> getBounds()
        {
            return Bounds;
        }
        //public static PolyHedron polyHedron = new PolyHedron("input.vtk");
        /// <summary>
        /// Reads PolyHedron points from file with path location
        /// </summary>
        /// <param name="path">location of file</param>
        public PolyHedron(string path)//Доделывать
        {
            readPoints(path);
        }
        /// <summary>
        /// Reads points coords(x,y,z) from path. Add this points to List
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <exception cref="BoundBindingException">thrown if didn't find anchors in file like Points or Polygons</exception>
        void readPoints(string path)
        {
            ThreeDPoint pt = new ThreeDPoint();
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            string Point="";
            string Polygon;
            const string Anchor1 = "POINTS";
            const string Anchor2 = "POLYGONS";
            int N;//stores quantity of Anchor i entity to read (count of lines)
            string[] PointCoordinates;//stores separated line
            double shift = 0.4;
            if (ShiftLines(Anchor1, file, out N) == 0)
            {
                while (N > 0 && (Point = file.ReadLine()) != null)
                {
                    double x, y, z;//Переменная для использования в tryparse. В ней нельзя юзать ссылочную переменную с out
                    PointCoordinates = Point.Split(new char[] { ' ', '\t', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                    ThreeDPoint tmp = new ThreeDPoint();
                    double.TryParse(PointCoordinates[0].Replace(".",","), out x);
                    tmp.x = x+shift;
                    double.TryParse(PointCoordinates[1].Replace(".", ","), out y);
                    tmp.y = y+shift;
                    double.TryParse(PointCoordinates[2].Replace(".", ","), out z);
                    tmp.z = z+shift;
                    Points.Add(tmp);
                    --N;
                }
                if (Point == null) { throw new BoundBindingException("Error of "+Anchor1+" format in file" + path); }
                if (ShiftLines(Anchor2, file, out N) == 0)
                {
                    
                    while (N > 0 && (Point = file.ReadLine()) != null)
                    {
                        Bound tmpBound = new Bound();
                        int x, y;//Переменная для использования в tryparse. В ней нельзя юзать ссылочную переменную с out. Хранят индексы точки для задания ребра
                        PointCoordinates = Point.Split(new char[] { ' ', '\t', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                        int PointsPerBound;
                        int.TryParse(PointCoordinates[0], out PointsPerBound);
                        if (PointCoordinates.Length-1 != PointsPerBound)
                        {
                            throw new BoundBindingException("Error of " + Anchor2 + " format in file " + path);
                        }
                        List<Edge> tmpEdges = new List<Edge>();//List of edges forming 1 bound
                        //Forming list of edges of 1 bound
                        for (int i = 1; i < PointsPerBound; ++i)//
                        {
                            if (!int.TryParse(PointCoordinates[i], out x) || !int.TryParse(PointCoordinates[i + 1], out y))
                            {
                                throw new BoundBindingException("Error of " + Anchor2 + " format in file" + path);
                            }
                            Edge e = new Edge(Points[x], Points[y]);
                            tmpBound.AddEdge(NewEdge(e));
                        }
                        //Замыкаем ребром последнюю и первую точки грани
                        tmpBound.AddEdge(NewEdge(new Edge(Points[int.Parse(PointCoordinates[PointsPerBound])],Points[int.Parse(PointCoordinates[1])])));
                        Bounds.Add(tmpBound);//Adding current bound to list of bounds
                        --N;
                    }
                }
                else throw new BoundBindingException("Error reading "+Anchor2+" of PolyHedron from file " + path);
            }
            else throw new BoundBindingException("Error reading "+Anchor1+" of PolyHedron from file "+path);
        }
        /// <summary>
        /// Returns pointer to the Edge. If the edge wasn't found in list of already added in list edges it creates and returns new Edge. Otherwise it returns pointer to input edge
        /// </summary>
        /// <param name="e">created outdoors edge</param>
        Edge NewEdge(Edge e)
        {
            //bool flag = false;
            for (int i = 0; i < Edges.Count /*&& !flag*/; ++i)
            {
                if (e.Point1 == Edges[i].Point1 && e.Point2 == Edges[i].Point2 || e.Point1 == Edges[i].Point2 && e.Point2 == Edges[i].Point1)
                {
                    //flag = true;
                    return e = Edges[i];
                }
            }
            Edges.Add(e);
            return e;
        }
        /// <summary>
        /// Used in readPoints
        /// Shifts text till the Anchor 1st word in line. If found writes 2nd value (quantity of Anchor) to PointsCount
        /// </summary>
        /// <param name="Anchor">Element name to search as 1st word in line</param>
        /// <param name="file">StreamReader which to continue to shift</param>
        /// <param name="Count">Quantity of Anchor elements</param>
        /// <returns>0 if shifted successfully\n-1 if shifted till the end of file without success</returns>
        int ShiftLines(string Anchor, System.IO.StreamReader file, out int Count)
        {
            string[] PointCoordinates;//stores separated line
            string Point;//stores entre line 
            while ((Point = file.ReadLine()) != null)
            {
                PointCoordinates = Point.Split(new char[] { ' ', '\t', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (string.Compare(PointCoordinates[0], Anchor) == 0)
                {
                    int.TryParse(PointCoordinates[1], out Count);
                    return 0;//all is ok. Unnesseccary text is shifted before end of file
                }
            }
            Count = 0;
            return -1;//Not ok. Shifted till the end of file
        }
        //Можно в будущем этот метод переделать с условием if внутри for. Где будет проверятся ребро на видимость
        public void Draw(System.Drawing.Graphics g,PointF center)
        {
            System.Drawing.Pen p=new System.Drawing.Pen(System.Drawing.Color.Blue,2);
            for (int i=0;i<Edges.Count;++i){
                if (Edges[i].Visible)
                {
                    PointF P1=new PointF((float)Edges[i].Point1.x,(float)Edges[i].Point1.y);//Можно подправить мой класс Point с double на float
                    PointF P2=new PointF((float)Edges[i].Point2.x,(float)Edges[i].Point2.y);
                    P1.X = P1.X + center.X;//shift to center and stretch for scale coefficient 
                    P1.Y = P1.Y + center.Y;
                    P2.X = P2.X + center.X;
                    P2.Y = P2.Y + center.Y;
                    g.DrawLine(p,P1,P2);
                }
            }
            p.Dispose();
        }
        public void Z_Buffer(){
            //Null inititalization for all edges in the beginning of algorithm. Then if bound is visible all its edges visible property sets to True
            for (int i=0;i<getEdges().Count;++i){
                getEdges()[i].Visible=false;
            }
            ThreeDPoint midPoint=new ThreeDPoint();
            for (int i=0;i<Points.Count;++i){
                midPoint.x+=getPoints()[i].x;
                midPoint.y+=getPoints()[i].y;
                midPoint.z+=getPoints()[i].z;
            }
            midPoint.x/=getPoints().Count;
            midPoint.y/=getPoints().Count;
            midPoint.z/=getPoints().Count;
            ThreeDPoint[] tmpPoint = new ThreeDPoint[3];
            for (int i = 0; i < getBounds().Count; ++i)
            {
                tmpPoint[0] = getBounds()[i].getEdge(0).Point1;
                tmpPoint[1] = getBounds()[i].getEdge(0).Point2;
                if (getBounds()[i].getEdge(0).Point1 == getBounds()[i].getEdge(1).Point1 || getBounds()[i].getEdge(0).Point2 == getBounds()[i].getEdge(1).Point1)
                {
                    tmpPoint[2] = getBounds()[i].getEdge(1).Point2;
                }
                else
                {
                    tmpPoint[2] = getBounds()[i].getEdge(1).Point1;
                }


                //Defining of bound coefficients
                double A, B, C, D;

                A = (tmpPoint[0].y * (tmpPoint[1].z - tmpPoint[2].z) + tmpPoint[1].y * (tmpPoint[2].z - tmpPoint[0].z) + tmpPoint[2].y * (tmpPoint[0].z - tmpPoint[1].z)) + 100;
                B = (tmpPoint[0].z * (tmpPoint[1].x - tmpPoint[2].x) + tmpPoint[1].z * (tmpPoint[2].x - tmpPoint[0].x) + tmpPoint[2].z * (tmpPoint[0].x - tmpPoint[1].x));
                C = (tmpPoint[0].x * (tmpPoint[1].y - tmpPoint[2].y) + tmpPoint[1].x * (tmpPoint[2].y - tmpPoint[0].y) + tmpPoint[2].x * (tmpPoint[0].y - tmpPoint[1].y));
                D = (-(tmpPoint[0].x * (tmpPoint[1].y * tmpPoint[2].z - tmpPoint[2].y * tmpPoint[1].z) + tmpPoint[1].x * (tmpPoint[2].y * tmpPoint[0].z - tmpPoint[0].y * tmpPoint[2].z) + tmpPoint[2].x * (tmpPoint[0].y * tmpPoint[1].z - tmpPoint[1].y * tmpPoint[0].z)));
                double sign = A * (float)midPoint.x + B * (float)midPoint.y + C * (float)midPoint.z + D;
                if (sign > 0)
                {
                    A = -A;
                    B = -B;
                    C = -C;
                }
                //Scalar multiplication which shows direction of bound normal relative to Oxy plane (0,0,-1)
                double scalarProizv = C * (-1);//Multiplying for -1 because normal to Oxy is (0,0,-1) and scalar multiplying is just -1 multiplying
                if (scalarProizv > 0)//if >0 then it is faced to us
                {
                    for (int k = 0; k < Bounds[i].getEdges().Count; ++k)
                    {
                        Bounds[i].getEdge(k).Visible = true;
                    }
                }
                /*for (int k = 0; k < panel1.Width; ++k)
                {
                    Z = Z(x, y);//Вставлять итерационную формулу и перед ней расчёт z[0]. Перед всем этим создать Z Buffer и заполнить максимальными/минимальными значениями. Далее сравнивать z(x,y) с этими значениями и обновлять Z Buffer             }
                }*/
            }
        }
        public void Weiler_Azerton()
        {
            //смотреть не перекрывает ли Bounds полt класса Bounds
            //Create copy of Bounds to modify
            List<Bound> Bounds = new List<Bound>();
            for (int i = 0; i < this.Bounds.Count; ++i)
            {
                Bounds.Add((Bound)this.Bounds[i].Clone());
            }
            ThreeDPoint nearestPt = new ThreeDPoint();
            nearestPt.z=Points[0].z;
            //defining the nearest plane
            for (int i = 0; i < Points.Count; ++i)
            {
                if (Points[i].z < nearestPt.z)
                {
                    nearestPt.z = Points[i].z;
                }
            }
            List<Edge> tmpEdges=new List<Edge>();
            for (int i = 0; i < Edges.Count; ++i)
            {
                if (Edges[i].Point1 == nearestPt || Edges[i].Point2 == nearestPt)
                {
                    tmpEdges.Add(Edges[i]);
                }
            }
            tmpEdges.Sort(Edge.CompareByZ);
            Bound nearestPlane=new Bound();
            for (int i = 0; i < Bounds.Count; ++i)
            {
                if (Bounds[i].getEdges().Contains(tmpEdges[0]) && Bounds[i].getEdges().Contains(tmpEdges[1]))
                {
                    nearestPlane=Bounds[i];
                    break;
                }
            }
            //Nearest plane is found. Let's find parts of underlaying planes to cut
            //Passes for all bounds. For each looks for edges crossing edges of nearestPlane
            
            bool One_cross,Two_crosses;
            One_cross = Two_crosses = false;
            //Надо написать метод который берёт точку пересечения и ребро грани А.Далее ищется пересечение продолжения пересекающего ребра (грани Б) и какого нить ребра грани А. Т.е. именно ПРОДОЛЖЕНИЕ ребра Б и одно ребро А
            //Нужен метод который будет брать ребро грани Б и по очереди проверять рёбра грани А на пересечение. По рёбрам Б и А строится прямая, далее находится точка пересечения и далее смотрится принадлежит ли данная точка текущему ребру А. (проверять изначальное ребро А не надо, надо передавать ссылку на это ребро чтобы его не проверять (путём сравнения ==))
            for (int j = 0; j < Bounds.Count; ++j)
            {
                if (Bounds[j] == nearestPlane)
                {
                    continue;
                }
                for (int i = 0; i < nearestPlane.getEdges().Count; ++i)
                {
                    //Думать куда впихнуть эти переменные
                    Edge firstCrossedEdge = new Edge(null,null);
                    Edge secondCrossedEdge = new Edge(null, null);
                    ThreeDPoint CrossPoint;
                    for (int k = 0; k < Bounds[j].getEdges().Count; ++k)
                    {
                        if (nearestPlane.getEdge(i) == Bounds[j].getEdge(k))
                        {
                            continue;
                        }
                        if ((CrossPoint = Edge.CrossPointofEdges(nearestPlane.getEdge(i), Bounds[j].getEdge(k))) != null)
                        {

                        }
                    }
                }
            }
            /*for (int j = 0; j < Bounds.Count; ++j)
            {
                if (Bounds[j] == nearestPlane)
                {
                    continue;
                }
                for (int i = 0; i < nearestPlane.getEdges().Count; ++i)
                {
                    for (int k = 0; k < Bounds[j].getEdges().Count; ++k)
                    {
                        CrossPoint = Edge.CrossPointofEdges(nearestPlane.getEdge(i),Bounds[j].getEdge(k));
                        if (CrossPoint == null) { continue; }
                        if (One_cross) { Two_crosses = true; }
                        One_cross = true;
                    }
                }
            }*/
        }
        /*public void EdgeToRastr()
        {
            List<ThreeDPoint[]> EdgesPoints = new List<ThreeDPoint[]>(Edges.Count);
            for (int i = 0; i < Edges.Count; ++i)
            {
                int xmin=(int)Math.Round(Edges[i].Point1.x);
                int xmax = (int)Math.Round(Edges[i].Point2.x);
                int ymin=(int)Math.Round(Edges[i].Point1.y);
                int ymax=(int)Math.Round(Edges[i].Point2.y);
                int tmp;
                int sign=1;
                if (xmin>xmax){
                    tmp=xmin;
                    xmin=xmax;
                    xmax=tmp;
                    sign=-1;
                }
                if (ymin>ymax){
                    tmp=ymin;
                    ymin=ymax;
                    ymax=tmp;
                }
                if (xmin==xmax){
                    EdgesPoints[i]=new ThreeDPoint[ymax-ymin+1];
                    for (int j=ymin;j<ymax;++j){
                        EdgesPoints[i][j].x=xmin;
                        EdgesPoints[i][j].y=j;
                        EdgesPoints[i][j].z=Edges[i].Point2.z-Edges[i].Point1 Edges[i].Point2.y-Edges[i].Point1.y sign*Math.Abs((Edges[i].Point1.y-Edges[i].Point2.y));
                    }

                }
                for (int j=xmin;j)

            }
        }*/
        /*void toRastr()
        {
            f
        }*/
        /*
        //Перенести этот метод в клас какой нить где будут храниться и создаваться объекты находящиеся на форме (для прорисовки)
        public void DrawAxises()
        {
            
        }*/

    }
}
