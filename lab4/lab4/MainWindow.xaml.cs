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

namespace lab4_komi
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            tb1.Text = "0 2 0 5 0 0\n0 0 1 0 2 0\n2 0 0 0 0 5\n4 0 0 0 0 2\n0 9 0 2 0 0\n0 0 2 0 2 0";
        }

        private int n, sum, minSum, passedVertex, end_way;
       
        private int[,] matrix_for_computations;			
        private int[] current_way, minWay;					
        int[,] koord = new int[10, 2] { { 10, 10 }, { 60, 60 }, { 110, 10 }, { 10, 160 }, { 60, 110 }, { 110, 160 }, { 160, 110 }, { 160, 60 }, { 210, 10 }, { 210, 110 } };

        

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int[,] matrix2;

            int points = 0;
             string alphabet = "abcdefghijklmnoprst";
            points = tb1.LineCount;
           
            
            matrix2 = new int[points, points];
            canv1.Children.Clear();
            tb4.Text = "";
            try
            {
                for (int i = 0; i < points; ++i)
                {
                    String s = tb1.Text;
                    string[] lines = s.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] chars = lines[i].Split(' ');
                    for (int j = 0; j < points; j++)
                    {

                        matrix2[i, j] = Convert.ToInt32(chars[j]);
                        if (Convert.ToInt32(chars[j]) !=0)
                        {
                            DrawLine(koord[i, 0], koord[i, 1], koord[j, 0], koord[j, 1], canv1, Convert.ToInt32(chars[j]),0);
                            tb4.Text += alphabet[i] + " --> " + alphabet[j] + "  =  " + chars[j] + "\n";
                        }
                    }
                }

               
                Draw(points, alphabet);

          
            current_way = new int[points + 1];
            minWay = new int[points + 1];
            new_matrix(matrix2, points);
            string str = route(alphabet);
            tb3.Text = str;
            }
            catch (FormatException)
            {
                MessageBox.Show("Помилка в заданій матриці суміжності!");
            }
        }

        public void new_matrix(int[,] matrix2, int number_of_points) //метод, що заповнює масиви даними, отриманими через конструктор
        {
            n = number_of_points;
            matrix_for_computations = new int[number_of_points + 1, number_of_points + 1];

            for (int i = 1; i <= number_of_points; i++)
                for (int j = 1; j <= number_of_points; j++)
                    matrix_for_computations[i, j] = matrix2[i - 1, j - 1];
        }


        public String route(string alphabet)
        {
            String res = "";
            for (int j = 1; j <= n; j++)
                current_way[j] = 0;
            sum = 0;//занулюємо суму
            end_way = 0;//шлях вважаємо не знайденим
            passedVertex = 1;
            current_way[1] = passedVertex;                  
            minSum = Int32.MaxValue / 2;
            recursSearch(1);                        	//шукаємо починаючи з першої вершини

            if (end_way > 0)                        		//якщо знайдено шлях
            {
                res += "Оптимальний маршрут: ";            //починаємо формувати результуючу стрічку

                int c = 1;        
                //номер в порядку обходу вершин
                int z = 1;

                for (int i = 1; i <= n; i++)      		//проходимо по всіх вершинах
                {
                    int j = 1;
                    while ((j <= n) && (minWay[j] != c)) 	//шукаємо наступну вершину в порядку обходу    
                        j++;

                    res += alphabet[j - 1] + "-";
                    
                    DrawLine(koord[z, 0], koord[z, 1], koord[j-1, 0], koord[j-1, 1], canv1, alphabet[j - 1],1);//додаємо вершину до результуючої стрічки
                    z = j-1;
                    c++;
                }
                DrawLine(koord[z, 0], koord[z, 1], koord[0, 0], koord[0, 1], canv1, 2, 1);
                res += alphabet[0];				//до результуючої стрічки додаємо першу вершину, якою завершується обхід
                res += "\nДовжина прокладеного шляху = " + minSum;//до результуючої стрічки додаємо суму ваг пройдених ребер

            }
            else
                res = "Не вдалося знайти шлях.";


            return res;
        }

        private void recursSearch(int x)
        {
            //якщо всі вершини переглянуті, 
            //і з останньої вершини є шлях в першу,
            //і мнова сума відстаней менша мінімальної
            if ((passedVertex == n) && (matrix_for_computations[x, 1] != 0) && (sum + matrix_for_computations[x, 1] < minSum))
            {
                end_way = 1;                    							//шлях вважається знайденим
                minSum = sum + matrix_for_computations[x, 1];               //вводимо нову мінімальну суму відстаней
                for (int i = 1; i <= n; i++)
                    minWay[i] = current_way[i];								//вводимо новий мінімальний шлях
            }
            else
            {

                for (int i = 1; i <= n; i++)     		//переглядаємо всі вершини з поточної

                    //нова вершина не співпадає з біжучою, є прямий шлях з біжучої вершини в нову, 
                    //нова вершина ще не переглянута, нова сума є меншою за мінімальну
                    if ((i != x) && (matrix_for_computations[x, i] != 0) && (current_way[i] == 0) && (sum + matrix_for_computations[x, i] < minSum))
                    {
                        sum += matrix_for_computations[x, i];                			//збільшуємо суму
                        passedVertex++;                						//збільшуємо кількість переглянутих вершин
                        current_way[i] = passedVertex;                				//відмічаємо у нової вершини новий номер у порядку обходу
                        recursSearch(i);                				//пошук нової вершини починаючи з і (вершина, в яку перейшли)
                        current_way[i] = 0;                    				//повертаємо все назад
                        passedVertex--;               							
                        sum -= matrix_for_computations[x, i];                			
                    }
            }
        }


        void DrawLine(int x1, int y1, int x2, int y2, Canvas canv, int ii, int m)
        {
            Line line = new Line();
            if (m == 1) line.Stroke = Brushes.Red;
            else
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
        void Draw(int points, string alf)
        {

            for (int i = 0; i < points; i++)
            {
                DrawVertex(koord[i, 0], koord[i, 1], i, alf);
            }

        }

        void DrawVertex(int x, int y, int s, string alf)
        {
            Ellipse el = new Ellipse();

            el.Width = 20;
            el.Height = 20;
            el.Fill = new SolidColorBrush(Colors.LightBlue);
            Canvas.SetLeft(el, x);
            Canvas.SetTop(el, y);
            canv1.Children.Add(el);

            Label l = new Label();
            l.Content = "" + alf[s];
            Canvas.SetLeft(l, x);
            Canvas.SetTop(l, y - 3);
            canv1.Children.Add(l);
        }


    }
}
