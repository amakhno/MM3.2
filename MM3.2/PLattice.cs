using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MM3._2
{
    class PLattice
    {
        public int PClustNum;
        public Dictionary<int, int> Clusters;
        public Dictionary<int, Color> ClustersClr;
        public int CountClusters = 0;
        public int SizePerkClust = 0;
        public delegate void Fun(string t);
        public event Fun WriteOutText;
        public int n;
        int[,] Lattice;

        public int[,] СlusterMap;
        Graphics GraphPane;
        Random RndForValue;
        public int SizeRect = 1;
        Bitmap Pane;
        PictureBox PicBox;
        List<Color> ListClr;
        public int Count;


        Random RndForColor;
        List<int> CorrectListNum;
        public PLattice(PictureBox PB, int SizeLattice)
        {

            ListClr = new List<Color>(new Color[1] { Color.Black });
            RndForColor = new Random();

            PicBox = PB;
            Pane = new Bitmap(PicBox.Height, PicBox.Width);
            GraphPane = Graphics.FromImage(Pane);
            RndForValue = new Random();
            n = SizeLattice;
            SizeRect = (int)(PB.Height / (n));
            Lattice = new int[n + 1, n + 1];
        }
        public void PutLattice(double P)
        {


            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= n; j++)
                {

                    if (RndForValue.NextDouble() < P)
                    {
                        Count++;
                        Lattice[i, j] = 1;
                    }
                }
            }




        }
        public void PutLattice(int C)
        {

            int a;
            int b;
            for (int i = 0; i < C; i++)
            {
                a = RndForValue.Next(1, n + 1);
                b = RndForValue.Next(1, n + 1);
                if (Lattice[a, b] != 1)
                {
                    Count++;
                    Lattice[a, b] = 1;
                }
                // GraphPane.FillRectangle(Brushes.Black, SizeRect * Rnd.Next(0, n), SizeRect * Rnd.Next(0, n), SizeRect, SizeRect);
            }
            WriteOutText("Добавлено");

        }
        public void Clear()
        {
            Count = 0;
            Lattice = new int[n + 1, n + 1];
            ClearPicBox();


        }
        public void ClearPicBox()
        {
            Pane = new Bitmap(PicBox.Height, PicBox.Width);
            GraphPane = Graphics.FromImage(Pane);
            PicBox.Image = Pane;
        }
        public void ClearPrint()
        {
            ClearPicBox();
            PrintLattice();
        }
        public void PrintLattice()
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (Lattice[i + 1, j + 1] == 1)
                        GraphPane.FillRectangle(Brushes.Black, SizeRect * i, SizeRect * j, SizeRect, SizeRect);

                }
            }
            PicBox.Refresh();
            PicBox.Image = Pane;
        }
        public void MarkingСluster()
        {


            CorrectListNum = new List<int>(new int[1] { 0 });
            int ClustNum = 1;
            int m1, m2;
            СlusterMap = new int[n + 1, n + 1];

            for (int i = 0; i <= n; i++)
            {
                for (int j = 0; j <= n; j++)
                {
                    if (Lattice[i, j] == 0) continue;

                    if (Lattice[i, j - 1] == 0 && Lattice[i - 1, j] == 0)
                    {
                        СlusterMap[i, j] = ClustNum;
                        CorrectListNum.Add(ClustNum);
                        ClustNum++;
                        //ListClr.Add(Color.FromArgb(RndForColor.Next(255), RndForColor.Next(255), RndForColor.Next(255)));
                    }
                    if (Lattice[i, j - 1] != 0 && Lattice[i - 1, j] == 0)
                    {
                        СlusterMap[i, j] = СlusterMap[i, j - 1];

                    }
                    if (Lattice[i, j - 1] == 0 && Lattice[i - 1, j] != 0)
                    {

                        СlusterMap[i, j] = CorrectListNum[getTN(СlusterMap[i - 1, j])];
                    }

                    if (Lattice[i, j - 1] != 0 && Lattice[i - 1, j] != 0)
                    {

                        m1 = СlusterMap[i, j - 1];
                        m2 = СlusterMap[i - 1, j];
                        if (m1 < m2)
                        {
                            СlusterMap[i, j] = m1;
                            CorrectListNum[m2] = m1;
                        }
                        else
                        {
                            СlusterMap[i, j] = m2;
                            CorrectListNum[m1] = m2;
                        }

                    }
                }
            }

            CorrectClusterMap();
            CalculateCoutCluster();
            SizeClust();
            CheckClusters();
        }
        public int getTN(int i)
        {
            if (CorrectListNum[i] == i)
            {
                return i;
            }
            else
            {
                return getTN(CorrectListNum[i]);
            }


        }
        public void PrintMarkLattice()
        {


            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {


                    if (СlusterMap[i + 1, j + 1] != 0)
                    {
                        GraphPane.FillRectangle(new SolidBrush(ClustersClr[СlusterMap[i + 1, j + 1]]), SizeRect * i, SizeRect * j, SizeRect, SizeRect);
                    }

                }
            }
            PicBox.Refresh();
            PicBox.Image = Pane;

        }
        public void CorrectClusterMap()
        {
            for (int i = 0; i <= n; i++)
            {
                for (int j = 0; j <= n; j++)
                {
                    if (CorrectListNum[СlusterMap[i, j]] != СlusterMap[i, j])
                    {
                        СlusterMap[i, j] = CorrectListNum[getTN(СlusterMap[i, j])];
                    }

                }
            }
        }
        public int CheckClusters()
        {

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= n; j++)
                {
                    if (СlusterMap[1, i] == СlusterMap[n, j] && СlusterMap[n, j] != 0)
                    {

                        PClustNum = СlusterMap[1, i];
                        ClustersClr[PClustNum] = Color.Black;
                        return СlusterMap[1, i];
                    }
                }
            }
            PClustNum = -1;
            return -1;



        }
        public int CalculateCoutCluster()
        {
            ListClr = new List<Color>(new Color[1] { Color.Black });
            CountClusters = 0;
            Clusters = new Dictionary<int, int>();
            ClustersClr = new Dictionary<int, Color>();

            for (int i = 1; i < CorrectListNum.Count; i++)
            {

                if (Clusters.Keys.ToList().IndexOf(CorrectListNum[i]) == -1)
                {

                    CountClusters++;
                    Clusters.Add(CorrectListNum[i], 0);
                    ClustersClr.Add(CorrectListNum[i], Color.FromArgb(RndForColor.Next(255), RndForColor.Next(255), RndForColor.Next(255)));
                }

            }
            return CountClusters;
        }
        public double FindP(double sP, double h)
        {
            Count = 0;
            Lattice = new int[n + 1, n + 1];
            double p;
            for (p = sP; sP < 1; p += h)
            {
                PutLattice(p);
                MarkingСluster();
                if (PClustNum != -1)
                {
                    ClearPicBox();
                    PrintMarkLattice();
                    break;
                }
                Count = 0;
                Lattice = new int[n + 1, n + 1];
            }
            return p;
        }
        public void SizeClust()
        {

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= n; j++)
                {
                    if (СlusterMap[i, j] == 0)
                    {
                        continue;
                    }

                    Clusters[СlusterMap[i, j]]++;
                }
            }
        }
        static T[,] Copy<T>(T[,] array)
        {
            T[,] newArray = new T[array.GetLength(0), array.GetLength(1)];
            for (int i = 0; i < array.GetLength(0); i++)
                for (int j = 0; j < array.GetLength(1); j++)
                    newArray[i, j] = array[i, j];
            return newArray;
        }


    }
}
