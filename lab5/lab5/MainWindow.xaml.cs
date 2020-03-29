using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace lab5_isomorf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //tb1.Text = "0 0 0 0 1 0 1 0 1 0\n0 0 0 0 0 1 1 0 0 1\n0 0 0 0 0 1 0 1 1 0\n0 0 0 0 1 0 0 1 0 1\n1 0 0 1 0 1 0 0 0 0\n0 1 1 0 1 0 0 0 0 0\n1 1 0 0 0 0 0 1 0 0\n0 0 1 1 0 0 1 0 0 0\n1 0 1 0 0 0 0 0 0 1\n0 1 0 1 0 0 0 0 1 0\n";
            //tb2.Text = "0 1 0 0 1 1 0 0 0 0\n1 0 1 0 0 0 1 0 0 0\n0 1 0 1 0 0 0 1 0 0\n0 0 1 0 1 0 0 0 1 0\n1 0 0 1 0 0 0 0 0 1\n1 0 0 0 0 0 0 1 1 0\n0 1 0 0 0 0 0 0 1 1\n0 0 1 0 0 1 0 0 0 1\n0 0 0 1 0 1 1 0 0 0\n0 0 0 0 1 0 1 1 0 0\n";

            tb1.Text = "0 0 1 0 1\n0 0 1 0 1\n1 1 0 1 0\n0 0 1 0 1\n1 1 0 1 0";
            tb2.Text = "0 1 0 1 0\n1 0 1 0 1\n0 1 0 1 0\n1 0 1 0 1\n0 1 0 1 0";
            
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
            numVert = -1;
            canv1.Children.Clear();
            canv2.Children.Clear();
            numVert = Convert.ToInt32(tb3.Text.Trim());                  
            ReadData(numVert);
           
        }

       
            public int numVert;  
            public int[,] data1 = { { 0 } }, data2 = { { 0 } }; //двовимірні масиви з даними про суміжність графів
            public int[] firstSumArray, secondSumArray, conformity; //масиви сум та відповідностей вершин
            
            public String result = "";

            int[,] koord = new int[10, 2] { { 10, 70 }, { 60, 30 }, { 60, 110 }, { 110, 50 }, { 110, 90 }, { 170, 30 }, { 170, 110 }, { 220, 50 }, { 220, 90 }, { 270, 70 } };




            public void ReadData(int numVert)
            {
                data1 = new int[numVert, numVert];
                

                for (int i = 0; i < numVert; i++)
                {
                    String s = tb1.Text;
                    string[] lines = s.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] chars = lines[i].Split(' ');
                    for (int j = 0; j < numVert; j++)
                    {
                        data1[i, j] = Convert.ToInt32(chars[j]);
                        if (Convert.ToInt32(chars[j]) == 1) {
                           
                            DrawLine(koord[i, 0], koord[i, 1], koord[j, 0], koord[j, 1],canv1);
                        }
                    }
                }
                

                data2 = new int[numVert, numVert];
               
                
                for (int i = 0; i < numVert; i++)
                {
                    String s = tb2.Text;
                    string[] lines = s.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] chars = lines[i].Split(' ');
                    for (int j = 0; j < numVert; j++)
                    {
                        data2[i, j] = Convert.ToInt32(chars[j]);
                        if (Convert.ToInt32(chars[j]) == 1)
                        {
                            DrawLine(koord[i, 0], koord[i, 1], koord[j, 0], koord[j, 1], canv2);
                        }
                    }
                }
                
                BrutalForce();
            }

            public void BrutalForce()
            {
               
                firstSumArray = new int[numVert];
                secondSumArray = new int[numVert];
                conformity = new int[numVert];

                for (int i = 0; i < numVert; i++)//перебираємо всі вершини
                    conformity[i] = -1;//ставимо їм у відповідність вершину -1

                Draw(canv1);
                Draw(canv2);
                tb4.Text = Res();
                
            }

            public String Res()//метод вирішення задачі
            {
                for (int i = 0; i < numVert; i++)//перебираємо усі рядки
                {
                    for (int j = 0; j < numVert; j++)//перебираємо усі вершини
                    {
                        firstSumArray[i] += data1[i, j];//додаємо вагу ребра
                        secondSumArray[i] += data2[i, j];//додаємо вагу ребра
                    }
                }

                for (int i = 0; i < numVert; i++)//перебираємо усі вершини
                {
                    for (int j = 0; j < numVert; j++)//перебираємо усі вершини
                    {
                        bool isDone = false;//чи перевірки завершені

                        for (int k = 0; k < numVert; k++)//перебираємо усі вершини
                            if (conformity[k] == j)//чи вершина має відповідну
                                isDone = true;//перевірки завершено

                        if (!isDone && firstSumArray[i] == secondSumArray[j])//якщо перевірки не завершені і вершина має відповідну
                        {
                            conformity[i] = j;//встановлюємо поточне j як відповідну вершину
                            break;//виходимо з циклу
                        }

                    }
                }

                result = "Графи є iзоморфними.";//формуємо стрічку-відповідь
                bool isrouted = true; //чи було знайдено розвязок

                for (int i = 0; i < numVert; i++)//перебираємо усі вершини
                    if (conformity[i] == -1)//якщо для вершини нема відповідної
                    {
                        MessageBox.Show("Графи не ізоморфні!! :(");
                        isrouted = false;//рішення не знайдено
                        result = "Графи не є iзоморфними.";//формуємо стрічку-відповідь
                        break;//виходимо з циклу
                    }

                if (isrouted)//якщо рішення знайдено
                {
                    MessageBox.Show("Графи ізоморфні!! :)");
                    result += "\nВiдповiдностi вершин першого i другого графа: \n";//доповннюємо стрічку-відповідь
                    for (int i = 0; i < numVert; i++)//перебираємо всі вершини
                    {
                        result += (i + 1) + "-->" + (conformity[i] + 1) + " ";//додаємо до стрічки відповіді відповідності вершин
                        result += "\n";
                    }
                }

                return result;//повертаємо отриману стрічку-відповідь
            }

            void Draw(Canvas cc)
            {
               
                
                for (int i = 0; i < numVert; i++)
                {
                    DrawVertex(koord[i, 0], koord[i, 1], i,cc);
                }

            }

            void DrawVertex(int x, int y, int s, Canvas cc)
            {
                Ellipse el = new Ellipse();

                el.Width = 20;
                el.Height = 20;
                el.Fill = new SolidColorBrush(Colors.LightBlue);
                Canvas.SetLeft(el, x);
                Canvas.SetTop(el, y);
                cc.Children.Add(el);

                Label l = new Label();
                l.Content = "" + (s+1);
                Canvas.SetLeft(l, x);
                Canvas.SetTop(l, y - 3);
                cc.Children.Add(l);
            }

            void DrawLine(int x1, int y1, int x2, int y2,Canvas canv)
            {
                Line line = new Line();
                line.Stroke = Brushes.Blue;

                line.X1 = x1 + 10;
                line.X2 = x2 + 10;
                line.Y1 = y1 + 10;
                line.Y2 = y2 + 10;

                line.StrokeThickness = 2;
                canv.Children.Add(line);

                Label l = new Label();
              
                canv.Children.Add(l);
            }

           

           

        }

       

    
}
