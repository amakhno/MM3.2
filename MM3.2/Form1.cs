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
        //Создание экземпляра класса
        PLattice PL;
        //Делегат на бесполезный старый метод, без которого ничего не работает
        delegate void SetTextCallback(string s);
        //Вероятность заполнения элемента (от 0 до 100)
        int Count = 65;
        //Размер решетки
        int SizeLattice = 12;

        public Form1()
        {
            InitializeComponent();
            //Инициализация класса
            PL = new PLattice(pictureBox1, SizeLattice);
            //Привязка к событию WriteOutText метода PL_SearcisСomplete
            PL.WriteOutText += PL_SearcisСomplete;
            //Инициализация решетки текущей вероятностью заполнения
            PL.PutLattice(Count);
            //Помечает кластеры методом Хошена-Копельмана
            PL.MarkingСluster();
            //Выводит цветную картинку на экран
            PL.PrintMarkLattice();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Использует старый размер класстера
            int cnt = PL.Count;
            //Инициализация класса с размером, который указан на форме
            PL = new PLattice(pictureBox1, Convert.ToInt32(textBox3.Text));
            //Привязка к событию WriteOutText метода PL_SearcisСomplete
            PL.WriteOutText += PL_SearcisСomplete;
            //Инициализация решетки текущей вероятностью заполнения
            PL.PutLattice(Count);
            //Помечает кластеры методом Хошена-Копельмана
            PL.MarkingСluster();
            //Выводит цветную картинку на экран
            PL.PrintMarkLattice();
            //Вывод результатов на форму
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

        //Старый метод для вывода текущего состояния работы алгоритма, сейчас не работает
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
            //Использует старый размер класстера
            int cnt = PL.Count;
            //Инициализация класса с размером, который указан на форме
            PL = new PLattice(pictureBox1, Convert.ToInt32(textBox3.Text));
            //Привязка к событию WriteOutText метода PL_SearcisСomplete
            PL.WriteOutText += PL_SearcisСomplete;
            //Инициализация решетки текущей вероятностью заполнения
            PL.PutLattice(Count);
            //Помечает кластеры методом Хошена-Копельмана
            PL.MarkingСluster();
            //Выводит цветную картинку на экран
            PL.PrintMarkLattice();
            //Вывод результатов на форму
            String result = "";
            result += "Порог перколяции " + PL.FindP(Convert.ToDouble(textBox1.Text.Replace(".", ",")), Convert.ToDouble(textBox2.Text.Replace(".", ",")));
            result += "\nКоличесво узлов " + PL.Count.ToString();
            result += "\nКоличесво кластеров " + (PL.Clusters.Count - 1).ToString();
            //Подсчет среднего размера кластера
            double Sum = 0;
            foreach(var a in PL.Clusters)
            {
                Sum += a.Key;
            }
            Sum /= (double)PL.Clusters.Count;
            //Продолжаем вывод результатов
            result += "\nСредний размер кластера " + Sum;
            if (PL.PClustNum == -1)
            {
                result += "\nПерколяционный кластер не найден ";
            }
            else
            {
                result += "\nРазмер перколяциооного кластера " + PL.Clusters[PL.PClustNum];
                double Se = -1 / Math.Log(PL.Count / (double)100);
                //Пытаемся по неправильному алгоритму посчитать sigma
                double sigma = -1 / ( (Math.Log(Se, Math.Abs(PL.Count / (double)100))) );
                result += "\nSigma = " + sigma;
            }
            richTextBox1.Text = result;
        }
    }
}
