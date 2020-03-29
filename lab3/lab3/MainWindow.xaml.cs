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
using System.IO;

namespace lab3_DM
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        int nodes=0;
        int edges=0;
        const int maxnode = 20;
        int[] color = new int[maxnode];
        int[] pred = new int[maxnode];
        int[,] capacity = new int[maxnode, maxnode];
        int[,] flow = new int[maxnode, maxnode];
        int[,] koord = new int[maxnode, 2] { { 30, 140 }, { 100, 80 }, { 100, 140 }, { 100, 200 }, { 200, 80 }, { 200, 200 }, { 240, 140 }, { 260, 20 }, { 260, 260 }, { 310, 100 }, { 310, 175 }, { 360, 20 }, { 360, 260 }, { 420, 80 }, { 420, 140 }, { 420, 200 }, { 520, 80 }, { 520, 140 }, { 520, 200 }, { 600, 140 } };


        class Edge
        {
            public int v1, v2;
            public int weight;          
            public int flow;           

            public Edge(int v1, int v2, int weight)
            {
                this.v1 = v1;
                this.v2 = v2;
                this.weight = weight;
            }
        }
       
        List<Edge> E = new List<Edge>();


        int min(int x, int y)
        {
            return x < y ? x : y; 
        }

        int head, tail;
        int []q=new int[maxnode+2];

        

        void enqueue(int x) {
            q[tail] = x;
            tail++;
            color[x] = 1;
        }

        int dequeue() {
            int x = q[head];
            head++;
            color[x] = 2;
            return x;
        }

        bool bfs(int start, int target) {
            int u, v;
            for (u = 0; u < nodes; u++ )
            {
                color[u] = 0;
            }
            head = tail = 0;
            enqueue(start);
            pred[start] = -1;
            while (head != tail)
            {
                u = dequeue();
                for (v = 0; v < nodes; v++ )
                {
                    if (color[v] == 0 && capacity[u,v] - flow[u,v] > 0) {
                        enqueue(v);
                        pred[v] = u;
                    }
                }
            }

            if (color[target] == 2) return true;
            else return false;            
        }


        int FordFulkerson(int source, int sink) {
            int i, j, u;
            int max_flow = 0;
            for (i = 0; i < nodes; i++) {
                for (j = 0; j < nodes; j++) {
                    flow[i, j] = 0;
                }
            }
            while (bfs(source,sink)) {
                int increment = 10000;
                for (u = nodes - 1; pred[u] >= 0; u = pred[u]) {
                    increment = min(increment, capacity[pred[u], u] - flow[pred[u], u]);
                }
                for (u = nodes - 1; pred[u] >= 0; u = pred[u]) {
                    flow[pred[u], u] += increment;
                    flow[u, pred[u]] -= increment;
                }
                max_flow += increment;
                
            }
            return max_flow;
        }

        void Draw() {

            for (int i = 0; i < edges; i++) {
                DrawLine(koord[E[i].v1, 0], koord[E[i].v1, 1], koord[E[i].v2, 0], koord[E[i].v2, 1], E[i].weight, E[i].flow);
            }
            for (int i = 0; i < nodes; i++) {
                DrawVertex(koord[i,0],koord[i,1],i);
                
            }

        }

        void DrawVertex(int x, int y, int s) {
            Ellipse el = new Ellipse();
           
            el.Width = 20;
            el.Height = 20;
            el.Fill = new SolidColorBrush(Colors.LightBlue);
            Canvas.SetLeft(el, x);
            Canvas.SetTop(el, y);
            canv.Children.Add(el);


            Label l = new Label();
            l.Content = ""+s;
            Canvas.SetLeft(l, x);
            Canvas.SetTop(l, y-3);
            canv.Children.Add(l);
        }

        void DrawLine(int x1, int y1, int x2, int y2, int s1, int s2)
        {
            Line line = new Line();
            line.Stroke = Brushes.LightSteelBlue;

            line.X1 = x1 + 10;
            line.X2 = x2 + 10;
            line.Y1 = y1 + 10;
            line.Y2 = y2 + 10;

            line.StrokeThickness = 2;
            canv.Children.Add(line);

            Label l = new Label();
            l.Content = "" + s1+" ("+s2+")";
            Canvas.SetLeft(l, (x1+x2)/2-20);
            Canvas.SetTop(l, (y1+y2)/2);
            canv.Children.Add(l);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            canv.Children.Clear();
            E.Add(new Edge(0, 1, 5));
            E.Add(new Edge(0, 2, 4));
            E.Add(new Edge(0, 3, 2));
            E.Add(new Edge(1, 2, 1));
            E.Add(new Edge(3, 2, 2));
            E.Add(new Edge(1, 4, 5));
            E.Add(new Edge(1, 5, 3));
            E.Add(new Edge(2, 4, 4));
            E.Add(new Edge(2, 5, 1));
            E.Add(new Edge(3, 5, 1));
            E.Add(new Edge(5, 6, 8));
            E.Add(new Edge(4, 6, 5));
            //E.Add(new Edge(0,1, 16));
            //E.Add(new Edge(0, 2, 13));
            //E.Add(new Edge(1, 2, 10));
            //E.Add(new Edge(2, 1, 4));
            //E.Add(new Edge(1, 3, 12));
            //E.Add(new Edge(3, 2, 9));
            //E.Add(new Edge(4, 3, 7));
            //E.Add(new Edge(2, 4, 14));
            //E.Add(new Edge(3, 5, 20));
            //E.Add(new Edge(4, 5, 4));
          


            nodes = 7;
            edges = 12;

           


            text2.Text = "                                           v1\tv2\tвага\n";
            for (int i = 0; i < edges; i++) {
                capacity[E[i].v1, E[i].v2] = E[i].weight;
                text2.Text += "Додано нове ребро \t" + E[i].v1 + "\t" + E[i].v2 + "\t" + E[i].weight+"\n";
            }
            text2.Text += "Вершин : " + nodes + "   ребер : " + edges + "\n";
            Draw();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            canv.Children.Clear();
            for (int i = 0; i < maxnode; i++) {
                color[i] = 0;
                pred[i] = 0;
                q[i] = 0;
            }
            for (int i = 0; i < nodes; i++)
            {
                for (int j = 0; j < nodes; j++)
                {
                    capacity[i, j] = 0;
                }
            }
            for (int i = 0; i < edges; i++)
            {
                capacity[E[i].v1, E[i].v2] = E[i].weight;
            }

                if (E.Count() != 0)
                {
                    int c = FordFulkerson(Convert.ToInt32(t4.Text), Convert.ToInt32(t5.Text));
                    text1.Text = "Максимальний потік = " + c + "\n\n";

                    text1.Text += "Ребро\tv1\tv2\tвага\tпотік\n";

                    for (int i = 0; i < edges; i++)
                    {
                        text1.Text += "" + i + "\t" + E[i].v1 + "\t" + E[i].v2 + "\t" + E[i].weight + "\t" + flow[E[i].v1, E[i].v2] + "\n";
                        E[i].flow = flow[E[i].v1, E[i].v2];
                    }




                    Draw();
                }
                else text2.Text = "Список ребер пустий.";
            
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            canv.Children.Clear();
            bool a=true;
            int a2 = 0;
            int a3 = 0;
            for (int i = 0; i < edges; i++) {
                if (E[i].v1 == Convert.ToInt32(t1.Text) && E[i].v2 == Convert.ToInt32(t2.Text)) a = false;
                if (E[i].v1 == Convert.ToInt32(t1.Text) || E[i].v2 == Convert.ToInt32(t1.Text)) a2++;
                if (E[i].v2 == Convert.ToInt32(t2.Text) || E[i].v1 == Convert.ToInt32(t2.Text)) a3++;
            }

            if (a)
            {
                text2.Text += "\n";
                E.Add(new Edge(Convert.ToInt32(t1.Text), Convert.ToInt32(t2.Text), Convert.ToInt32(t3.Text)));
                capacity[E[edges].v1, E[edges].v2] = E[edges].weight;
                text2.Text += "Додано нове ребро \t" + E[edges].v1 + "\t" + E[edges].v2 + "\t" + E[edges].weight + "\n";

                edges++;
                if (a2 == 0) nodes++;
                if (a3 == 0) nodes++;

                text2.Text += "Вершин : " + nodes + "   ребер : " + edges + "\n";
            }
            else { text2.Text += "Дане ребро вже додане.\n"; }

            Draw();
           
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            text2.Text = "Список ребер пустий.";
            E.Clear();
            nodes = 0;
            edges = 0;
            canv.Children.Clear();
          
        }
    }
}



