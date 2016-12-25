using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MM3._2
{
    public partial class Form1 : Form
    {
        PLattice PL;
        delegate void SetTextCallback(string s);
        int Count = 65;
        int SizeLattice = 12;

        public Form1()
        {
            InitializeComponent();
            PL = new PLattice(pictureBox1, SizeLattice);
            PL.WriteOutText += PL_SearcisСomplete;
            //PL.SizeRect = 4;
            PL.PutLattice(Count);
            //PL.Print();
            PL.MarkingСluster();
            PL.PrintMarkLattice();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int cnt = PL.Count;
            PL = new PLattice(pictureBox1, Convert.ToInt32(textBox3.Text));
            PL.WriteOutText += PL_SearcisСomplete;
            PL.PutLattice(cnt);
            PL.MarkingСluster();
            PL.PrintMarkLattice();
            String result = "Количесво узлов " + PL.Count.ToString();
            result += "\nКоличесво кластеров " + (PL.Clusters.Count - 1).ToString();
            if (PL.PClustNum == -1)
            {
                result += "\nПерколяционный кластер не найден ";
            }
            else
            {
                result += "\nРазмер перколяциооного кластера " + PL.Clusters[PL.PClustNum];
            }
            richTextBox1.Text = result;
        }

        void PL_SearcisСomplete(string s)
        {

            if (this.label1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(PL_SearcisСomplete);
                this.Invoke(d, new object[] { s });
            }
            else
            {
                this.label1.Text = s;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            int cnt = PL.Count;
            PL = new PLattice(pictureBox1, Convert.ToInt32(textBox3.Text));
            PL.WriteOutText += PL_SearcisСomplete;
            PL.PutLattice(cnt);
            PL.MarkingСluster();
            PL.PrintMarkLattice();
            String result = "";
            result += "Порог перколяции " + PL.FindP(Convert.ToDouble(textBox1.Text.Replace(".", ",")), Convert.ToDouble(textBox2.Text.Replace(".", ",")));
            result += "\nКоличесво узлов " + PL.Count.ToString();
            result += "\nКоличесво кластеров " + (PL.Clusters.Count - 1).ToString();
            double Sum = 0;
            foreach(var a in PL.Clusters)
            {
                Sum += a.Key;
            }
            Sum /= (double)PL.Clusters.Count;
            result += "\nСредний размер кластера " + Sum;
            if (PL.PClustNum == -1)
            {
                result += "\nПерколяционный кластер не найден ";
            }
            else
            {
                result += "\nРазмер перколяциооного кластера " + PL.Clusters[PL.PClustNum];
                double Se = -1 / Math.Log(PL.Count / (double)100);
                double sigma = -1 / ( (Math.Log(Se, Math.Abs(PL.Count / (double)100))) );
                result += "\nSigma = " + sigma;
            }
            richTextBox1.Text = result;
        }
    }
}
