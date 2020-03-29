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

namespace Lab2_DM_maloid
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }


        private List<Edge> edges = new List<Edge>();
        private List<int> vert = new List<int>();
        private List<Vertex> vertices = new List<Vertex>();
        private List<String> Comp_edges = new List<String>();
        private List<List<Edge>> AllEdges = new List<List<Edge>>();
        private List<Edge> Result = new List<Edge>();
      
        private int INDEX = 0;
        private bool isOK;
        private bool Enough = false;

        private class Vertex
        {
            private string name;
            private int id;
            private int component = int.MaxValue;
            private int neighboors;
            private bool off = false;
            public string NAME
            {
                get { return name; }
                set { name = value; }
            }
            public int ID
            {
                get { return id; }
                set { id = value; }
            }
            public int COMP
            {
                get { return component; }
                set { component = value; }
            }
            public int NEIGHBOOR
            {
                get { return neighboors; }
                set { neighboors = value; }
            }
            public bool OFF
            {
                get { return off; }
                set { off = value; }
            }
            public Vertex() { }
            public Vertex(string s, int a1)
            {
                name = s;
                id = a1;
            }
        }
        //ребро
        private class Edge
        {
            private int vertex1_id;
            private int vertex2_id;
            private int Weight;
            private int Comp;
            public int VERTEX1
            {
                get { return vertex1_id; }
                set { vertex1_id = value; }
            }
            public int VERTEX2
            {
                get { return vertex2_id; }
                set { vertex2_id = value; }
            }
            public int WEIGHT
            {
                get { return Weight; }
                set { Weight = value; }
            }
            public int COMP
            {
                get { return Comp; }
                set { Comp = value; }
            }
            public Edge() { }
            public Edge(int a1, int a2, int a3)
            {
                vertex1_id = a1;
                vertex2_id = a2;
                Weight = a3;
            }
        }
        //зчитати ребра з файлу
        private void ReadEdges(string path, List<Edge> input, List<int> input2)
        {
            String[] str = File.ReadAllLines(path);
            int[,] matrix = new int[str.Length, 3];
            for (int i = 0; i < str.Length; i++)
            {
                int[] arr = str[i].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToArray();
                for (int j = 0; j < arr.Length; j++)
                {
                    matrix[i, j] = arr[j];
                }
            }
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                input.Add(new Edge(matrix[i, 0], matrix[i, 1], matrix[i, 2]));
                input2.Add(matrix[i, 0]);
                input2.Add(matrix[i, 1]);
            }
        }
        //Список вершин
        private List<Vertex> GetUniquesVer(List<int> input)
        {
            Dictionary<int, bool> found = new Dictionary<int, bool>();
            List<Vertex> uniques = new List<Vertex>();
            foreach (int value in input)
            {
                if (!found.ContainsKey(value))
                {
                    found[value] = true;
                    char ch = Convert.ToChar(value + 64);
                    uniques.Add(new Vertex(ch.ToString(), value));
                }
            }
            return uniques;
        }
        //Належність до підграфа
        private void MarkEdge(Edge ed, List<Edge> Elist, int num)
        {
            for (int i = 0; i < Elist.Count; i++)
            {
                if (Elist[i].VERTEX1 == ed.VERTEX1 && Elist[i].VERTEX2 == ed.VERTEX2)
                {
                    ed.COMP = num;
                    break;
                }
            }
        }
        //Дістаю перелік унік. ел.
        private List<int> GetUnique(List<int> input)
        {
            List<int> output = new List<int>();
            Dictionary<int, bool> dict = new Dictionary<int, bool>();
            for (int i = 0; i < input.Count; i++)
            {
                if (!dict.ContainsKey(input[i]))
                {
                    dict[input[i]] = true;
                    output.Add(input[i]);
                }
            }
            return output;
        }
        private Point[] ReadCoords(string path)
        {
            String[] str = File.ReadAllLines(path);
            Point[] points = new Point[str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                int[] arr = str[i].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToArray();
                points[i].X = arr[0];
                points[i].Y = arr[1];
            }
            return points;
        }



        private List<int> CountUniques<T>(List<T> list)
        {
            List<int> result = new List<int>();
            Dictionary<T, int> counts = new Dictionary<T, int>();
            List<T> uniques = new List<T>();
            foreach (T val in list)
            {
                if (counts.ContainsKey(val))
                    counts[val]++;
                else
                {
                    counts[val] = 1;
                    uniques.Add(val);
                }
            }
            foreach (T val in uniques)
            {
                result.Add(counts[val]);
            }
            return result;
        }
        //Словник унікальних
        private Dictionary<T, int> UniquesDict<T>(List<T> list)
        {
            List<int> result = new List<int>();
            Dictionary<T, int> counts = new Dictionary<T, int>();
            List<T> uniques = new List<T>();
            foreach (T val in list)
            {
                if (counts.ContainsKey(val))
                    counts[val]++;
                else
                {
                    counts[val] = 1;
                    uniques.Add(val);
                }
            }

            return counts;
        }
        //Визначення наявності Ейлерового циклу в графі
        private bool IsEilerCycle(List<Vertex> iVer, List<Edge> iEdg)
        {
            bool cycle = true;
            List<int> list = new List<int>();
            for (int i = 0; i < iEdg.Count; i++)
            {
                list.Add(iEdg[i].VERTEX1);
                list.Add(iEdg[i].VERTEX2);
            }
            list = CountUniques<int>(list);
            foreach (int item in list)
            {
                if (item % 2 != 0)
                {
                    cycle = false;
                    break;
                }
            }

            return cycle;
        }
        //Вершини непарного степеня
        private int OddDegree(List<Vertex> Vist, List<Edge> Elist, List<Vertex> res)
        {
            int count = 0;
            List<int> list = new List<int>();
            for (int i = 0; i < Elist.Count; i++)
            {
                list.Add(Elist[i].VERTEX1);
                list.Add(Elist[i].VERTEX2);
            }
            Dictionary<int, int> dict = UniquesDict<int>(list);
            foreach (KeyValuePair<int, int> entry in dict)
            {
                if (entry.Value % 2 != 0)
                {
                    res.Add(FindVertex(entry.Key, Vist));
                    count += 1;
                }
            }
            return count;
        }
        //Чи задіяні всі ребра
        private bool CheckEdges(List<Edge> Elist)
        {
            foreach (Edge e in Elist)
            {
                if (e.COMP == 0)
                    return false;
            }
            return true;
        }
        //Знайти інцидентне ребро(вільне)
        private Edge FindGoodEdge(Vertex iVer, List<Edge> Elist)
        {
            Edge edge = new Edge();
            foreach (Edge e in Elist)
            {
                if ((e.VERTEX1 == iVer.ID || e.VERTEX2 == iVer.ID) && e.COMP == 0)
                {
                    edge = e;
                    break;
                }
            }
            return edge;
        }
        //Знайти вершину за ID
        private Vertex FindVertex(int id, List<Vertex> Vlist)
        {
            foreach (Vertex v in Vlist)
            {
                if (v.ID == id)
                {
                    return v;
                }
            }
            return new Vertex();
        }
        //Чи закінчене злиття компонент
        private bool IsEnd(List<List<Edge>> Elist)
        {
            if (Elist.Count == 1)
                return true;
            else
                return false;
            //foreach (Edge e in Elist)
            //{
            //    if (e.COMP != 1)
            //        return false;
            //}
            //return true;
        }
        //Обхід
        private void PostmanWalk(Vertex beginV, List<Edge> Elist, List<Vertex> Vlist, List<List<Edge>> sBuild, int Num)
        {
            Vertex ver = new Vertex();
            Vertex v = beginV;
            List<Edge> sb = new List<Edge>();
            while (ver.ID != beginV.ID)
            {
                ver = v;
                Edge e = FindGoodEdge(ver, Elist);
                MarkEdge(e, Elist, Num);
                sb.Add(e);
                if (ver.ID != e.VERTEX1)
                    ver = v = FindVertex(e.VERTEX1, Vlist);
                else
                    ver = v = FindVertex(e.VERTEX2, Vlist);
            }
            sBuild.Add(sb);

        }
        //Знайти підходящу вершину
        private Vertex FindGoodVertex(List<Vertex> Vlist, List<Edge> Elist)
        {
            Vertex ver = new Vertex();
            foreach (Vertex v in Vlist)
            {
                if (FindGoodEdge(v, Elist).VERTEX1 != 0)
                {
                    return v;
                }
            }
            return ver;
        }
        //Всі вершини,що в ходять в компоненту
        private List<int> ComponentVertices(List<Edge> Elist, int comp_n)
        {
            List<int> vert = new List<int>();
            for (int i = 0; i < Elist.Count; i++)
            {
                if (Elist[i].COMP == comp_n)
                {
                    vert.Add(Elist[i].VERTEX1);
                    vert.Add(Elist[i].VERTEX2);
                }
            }
            vert = GetUnique(vert);
            return vert;
        }
        //Знайти спільну для компонент вершину
        private Vertex FindCommonVertex(List<Edge> Elist, int comp, List<Vertex> Vlist)
        {
            Vertex vert = new Vertex();
            List<int> list1 = ComponentVertices(Elist, 1);
            List<int> list2 = ComponentVertices(Elist, comp);
            bool breaked = false;
            for (int i = 0; i < list1.Count; i++)
            {
                for (int j = 0; j < list2.Count; j++)
                {
                    if (list1[i] == list2[j])
                    {
                        vert = FindVertex(list1[i], Vlist);
                        breaked = true;
                        break;
                    }
                }
                if (breaked)
                    break;
            }
            return vert;
        }
        //З'єднання компонент
        private List<List<Edge>> SplitAndMerge(Vertex v, List<List<Edge>> e, int ind)
        {
            bool needToSplit = false;
            bool splitted = false;
            List<List<Edge>> result = new List<List<Edge>>();
            for (int i = 0; i < e.Count; i++)
            {
                result.Add(new List<Edge>());
            }
            for (int i = 0; i < e[0].Count; i++)
            {
                if (!splitted)
                {
                    if (v.ID == e[0][i].VERTEX1 || v.ID == e[0][i].VERTEX2)
                    {
                        needToSplit = true;
                        result[0].Add(e[0][i]);
                    }
                    else
                    {
                        result[0].Add(e[0][i]);
                    }
                }
                else
                {
                    result[0].Add(e[0][i]);
                }
                if (needToSplit)
                {
                    result[0].AddRange(e[1]);
                    needToSplit = false;
                    splitted = true;
                }
            }
            for (int i = 0; i < e.Count; i++)
            {
                if (i != 0 && i != 1)
                    result[i].AddRange(e[i]);
            }
            for (int i = 0; i < result.Count; i++)
            {
                if (result[i].Count == 0)
                    result.RemoveAt(i);
            }
            return result;
        }
        //Вивід шляху(текстовий)
        private void ShowWay(TextBox txt1, List<Edge> Elist, List<Vertex> Vlist)
        {
            txt1.Text = "";
            int Price = 0;
            for (int i = 0; i < Elist.Count; i++)
            {
                String s1 = FindVertex(Elist[i].VERTEX1, Vlist).NAME;
                String s2 = FindVertex(Elist[i].VERTEX2, Vlist).NAME;
                String str = String.Format("({0},{1}) - {2}\r\n", s1, s2, Elist[i].WEIGHT);
                Price += Elist[i].WEIGHT;
                txt1.Text += str;
            }
            txt1.Text += "\r\nВага шляху = " + Price.ToString();
        }
        //Чи закінчення алгоритму Дейкстри
        private bool isEnd(List<Vertex> Vlist)
        {
            for (int i = 0; i < Vlist.Count; i++)
            {
                if (Vlist[i].OFF == false)
                    return false;
            }
            return true;
        }
        //Знайти вершину з мін шляхом
        private int FindMinVertexId(List<Vertex> Vlist)
        {
            int index = 0;
            int Min = int.MaxValue;
            for (int i = 0; i < Vlist.Count; i++)
            {
                if (Vlist[i].COMP < Min && !Vlist[i].OFF)
                {
                    Min = Vlist[i].COMP;
                    index = i + 1;
                }
            }
            return index;
        }
        //Прокласти шлях до сусідів
        private void RoadToNeighbors(int id, List<Vertex> Vlist, List<Edge> Elist)
        {
            Vertex vert1 = FindVertex(id, Vlist);
            for (int i = 0; i < Elist.Count; i++)
            {
                if (id == Elist[i].VERTEX1)
                {

                    Vertex vert2 = FindVertex(Elist[i].VERTEX2, Vlist);

                    int way = vert1.COMP + Elist[i].WEIGHT;
                    if (way < vert2.COMP)
                    {
                        vert2.COMP = way;
                        vert2.NEIGHBOOR = vert1.ID;
                    }
                }
                else if (id == Elist[i].VERTEX2)
                {
                    Vertex vert2 = FindVertex(Elist[i].VERTEX1, Vlist);

                    int way = vert1.COMP + Elist[i].WEIGHT;
                    if (way < vert2.COMP)
                    {
                        vert2.COMP = way;
                        vert2.NEIGHBOOR = vert1.ID;
                    }
                }
            }
            for (int i = 0; i < Vlist.Count; i++)
            {
                if (Vlist[i].ID == id)
                {
                    Vlist[i].OFF = true;
                    break;
                }
            }
        }
        //Сортувати по зростанню ваги
        private void SortVert(List<Vertex> input)
        {
            for (int i = input.Count - 1; i > 0; i--)
                for (int j = 0; j < i; j++)
                    if (input[j].NAME.ToCharArray()[0] > input[j + 1].NAME.ToCharArray()[0])
                        Swap(input, j, j + 1);
        }
        private void Swap(List<Vertex> inp, int i, int j)
        {
            Vertex temp = inp[i];
            inp[i] = inp[j];
            inp[j] = temp;
        }
        private Edge FindEdge(Vertex ver, Vertex ver2, List<Edge> Elist)
        {
            Edge e = new Edge();
            for (int i = 0; i < Elist.Count; i++)
            {
                if ((ver.ID == Elist[i].VERTEX1 && ver2.ID == Elist[i].VERTEX2) || (ver2.ID == Elist[i].VERTEX1 && ver.ID == Elist[i].VERTEX2))
                {
                    e = Elist[i];
                }
            }
            return e;
        }
        //шлях в ребрах
        private List<Edge> ShortWay(Vertex vert1, Vertex vert2, List<Edge> Elist, List<Vertex> Vlist)
        {
            List<Edge> result = new List<Edge>();
            Vertex VR = vert2;
            bool isEnd = false;
            while (!isEnd)
            {
                result.Add(FindEdge(VR, FindVertex(VR.NEIGHBOOR, Vlist), Elist));
                VR = FindVertex(VR.NEIGHBOOR, Vlist);
                if (vert1.ID == VR.ID)
                    isEnd = true;
            }
            return result;
        }
        //Відповідна вершина у повному списку(для непарного степеня)
        private int FindEquivalent(Vertex vr, List<Vertex> Vlist)
        {
            int ind = -1;
            for (int i = 0; i < Vlist.Count; i++)
            {
                if (Vlist[i].ID == vr.ID)
                {
                    ind = i;
                    break;
                }
            }
            return ind;
        }
        //Головна функція
        private void MAINACTION()
        {
            int index = 1;
            Vertex vert1 = vertices[0];
            while (!CheckEdges(edges))
            {
                PostmanWalk(vert1, edges, vertices, AllEdges, index);
                index += 1;
                vert1 = FindGoodVertex(vertices, edges);
            }
            index = 2;
            while (!IsEnd(AllEdges))
            {
                Vertex com = FindCommonVertex(edges, index, vertices);
                AllEdges = SplitAndMerge(com, AllEdges, index);

                index += 1;
            }
            edges = AllEdges[0];
            ShowWay(txt1, edges, vertices);
        }



        void Draw()
        {

            Point[] points = ReadCoords("Coords.txt");
            for (int i = 0; i < edges.Count; i++)
            {
                DrawLine((int)points[edges[i].VERTEX1 - 1].X, (int)points[edges[i].VERTEX1 - 1].Y, (int)points[edges[i].VERTEX2 - 1].X, (int)points[edges[i].VERTEX2 - 1].Y, edges[i].WEIGHT.ToString());
            }
            for (int i = 0; i < vertices.Count; i++)
            {
                DrawVertex((int)points[i].X, (int)points[i].Y, vertices[i].NAME);            
            }
           

        }

        void DrawVertex(int x, int y, String s)
        {
            Ellipse el = new Ellipse();

            el.Width = 20;
            el.Height = 20;
            el.Fill = new SolidColorBrush(Colors.LightBlue);
            Canvas.SetLeft(el, x);
            Canvas.SetTop(el, y);
            canv.Children.Add(el);


            Label l = new Label();
            l.Content = "" + s;
            Canvas.SetLeft(l, x);
            Canvas.SetTop(l, y - 3);
            canv.Children.Add(l);
        }

        void DrawLine(int x1, int y1, int x2, int y2, String s1)
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
            l.Content = "" + s1;
            Canvas.SetLeft(l, (x1 + x2) / 2);
            Canvas.SetTop(l, (y1 + y2) / 2);
            canv.Children.Add(l);
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            edges.Clear();
            vert.Clear();
            vertices.Clear();
            AllEdges.Clear();
            INDEX = 0;
            ReadEdges("Data.txt", edges, vert);
            vertices = GetUniquesVer(vert);

            //Парна степінь
            if (IsEilerCycle(vertices, edges))
            {
                MAINACTION();
            }
            //непарна степінь
            else
            {
                List<Vertex> vr = new List<Vertex>();
                int Count = OddDegree(vertices, edges, vr);
                SortVert(vertices);

                vertices[FindEquivalent(vr[0], vertices)].COMP = 0;
                while (!isEnd(vertices))
                {
                    int id = FindMinVertexId(vertices);
                    RoadToNeighbors(id, vertices, edges);
                }
                switch (Count)
                {
                    case 2:
                        List<Edge> shortWay = ShortWay(vertices[FindEquivalent(vr[0], vertices)], vertices[FindEquivalent(vr[1], vertices)], edges, vertices);
                        for (int i = 0; i < shortWay.Count; i++)
                        {
                            Edge ed = new Edge(shortWay[i].VERTEX1, shortWay[i].VERTEX2, shortWay[i].WEIGHT);
                            edges.Add(ed);
                        }
                        break;
                    default:
                        MessageBox.Show("Помилка, забагато непарних вершин)");
                        Enough = true;
                        break;
                }

                MAINACTION();

            }
            if (!Enough)
            {
                Draw();
                
                isOK = true;
                
            }
        }
    }
}
