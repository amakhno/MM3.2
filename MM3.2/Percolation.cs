using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MM3._2
{
    class Percolation
    {
        int size;
        double prob = 0.6;
        int[,] oldArray;
        int[,] newArray;
        int[] labels = new int[800];
        int[,] label;
        List<int> N;

        public Percolation(int size)
        {
            this.size = size;
            label = new int[size, size];
        }

        public int[,] GetArray()
        {
            Random rnd = new Random();
            int[,] result = new int[size, size];
            for (int i = 1; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    if (rnd.Next(0, 100) / (double)100 < prob)
                    {
                        result[i, j] = 1;
                    }
                    else
                    {
                        result[i, j] = 0;
                    }
                }
            }
            return result;
        }

        public void Calculate()
        {
            #region MyVersion
            //this.oldArray = GetArray();
            //int[,] result = (int[,])oldArray.Clone();
            //this.N = new List<int>();
            //N.Add(0);
            //for (int i = 1; i < (size - 1); i++)
            //{
            //    for (int j = 1; j < (size - 1); j++)
            //    {
            //        if (oldArray[i, j] == 1)
            //        {
            //            var good = GetListOfGood(i, j);
            //            var all = GetListOfNeightbor(i, j);
            //            if (all.Count == 0)
            //            {
            //                N.Add(1);
            //                oldArray[i, j] = N.Count;
            //                continue;
            //            }
            //            if (all.Count == 1)
            //            {
            //                if (good.Count > 0)
            //                {
            //                    N[good[0]] += 1;
            //                    oldArray[i, j] = good[0];
            //                }
            //                continue;
            //            }
            //            if (all.Count > 1)
            //            {
            //                if (good.Count > 0)
            //                {
            //                    var minMark = good.Min();
            //                    oldArray[i, j] = minMark;
            //                    N[minMark] = 1 + good.Sum();
            //                    foreach (int a in good)
            //                    {
            //                        if (a != minMark)
            //                        {
            //                            N[a] -= minMark;
            //                        }
            //                    }
            //                }
            //                continue;
            //            }
            //        }
            //    }
            //}
            #endregion
            label = new int[size, size];
            int n_columns = size;
            int n_rows = size;
            int left=0, above;
            int[,] occupied = GetArray();
            int largest_label = 0;
            for (int x = 1; x < n_columns; x++)
            {
                for (int y = 1; y < n_rows; y++)
                {
                    if (occupied[x, y] == 1)
                    {
                        left = occupied[x - 1, y];                    
                        above = occupied[x, y - 1];
                        if ((left == 0) && (above == 0))
                        {
                            largest_label = largest_label + 1;
                            label[x, y] = largest_label;
                        }
                        else
                        {
                            if ((left != 0) && (above == 0))
                            {
                                label[x, y] = find(left);
                            }
                            else
                            {
                                if ((left == 0) && (above != 0))
                                {
                                    label[x, y] = find(above);
                                }
                                else
                                {
                                    union(left, above);
                                    label[x, y] = find(left);
                                }
                            }
                        }
                    }                    
            }
        }
    }

    void union(int x, int y)
    {
        labels[find(x)] = find(y);
    }

    int find(int x)
    {
        int y = x;
        while (labels[y] != y)
            y = labels[y];
        while (labels[x] != x)
        {
            int z = labels[x];
            labels[x] = y;
            x = z;
        }
        return y;
    }

    private List<int> GetListOfGood(int i, int j)
    {
        var a = new List<int>();
        var b = oldArray[i - 1, j];
        if ((b != 0) && (b != 1))
        {
            a.Add(b);
        }
        b = oldArray[i, j - 1];
        if ((b != 0) && (b != 1))
        {
            a.Add(b);
        }
        b = oldArray[i + 1, j];
        if ((b != 0) && (b != 1))
        {
            a.Add(b);
        }
        b = oldArray[i, j + 1];
        if ((b != 0) && (b != 1))
        {
            a.Add(b);
        }
        return a;
    }

    private List<int> GetListOfNeightbor(int i, int j)
    {
        var a = new List<int>();
        var b = oldArray[i - 1, j];
        if (b != 0)
        {
            a.Add(b);
        }
        b = oldArray[i, j - 1];
        if (b != 0)
        {
            a.Add(b);
        }
        b = oldArray[i + 1, j];
        if (b != 0)
        {
            a.Add(b);
        }
        b = oldArray[i, j + 1];
        if (b != 0)
        {
            a.Add(b);
        }
        return a;
    }

    private bool CheckAloneNeightbor(int i, int j)
    {
        if ((oldArray[i - 1, j] == 1) && (oldArray[i, j - 1] == 0) && (oldArray[i + 1, j] == 0) && (oldArray[i, j + 1] == 0))
        {
            oldArray[i, j] = oldArray[i - 1, j];
            N[oldArray[i - 1, j]]++;
            return true;
        }
        if ((oldArray[i - 1, j] == 0) && (oldArray[i, j - 1] == 1) && (oldArray[i + 1, j] == 0) && (oldArray[i, j + 1] == 0))
        {
            oldArray[i, j] = oldArray[i, j - 1];
            N[oldArray[i, j - 1]]++;
            return true;
        }
        if ((oldArray[i - 1, j] == 0) && (oldArray[i, j - 1] == 0) && (oldArray[i + 1, j] == 1) && (oldArray[i, j + 1] == 0))
        {
            oldArray[i, j] = oldArray[i + 1, j];
            N[oldArray[i + 1, j]]++;
            return true;
        }
        if ((oldArray[i - 1, j] == 0) && (oldArray[i, j - 1] == 0) && (oldArray[i + 1, j] == 0) && (oldArray[i, j + 1] == 1))
        {
            oldArray[i, j] = oldArray[i, j + 1];
            N[oldArray[i, j + 1]]++;
            return true;
        }
        return false;
    }

    public void Show(PictureBox box)
    {
        box.Refresh();
        var a = GetArray();
        int Height = (int)((double)box.Height / (double)size);
        int Width = (int)((double)box.Width / (double)size);
        Graphics graph;
        graph = box.CreateGraphics();
        for (int j = 0; j < size; j++)
        {
            for (int i = 0; i < size; i++)
            {
                if (a[i, j] == 1)
                {
                    graph.FillEllipse(Brushes.Black, i * Width, j * Height, Width, Height);
                }
            }
        }
    }
}
}
