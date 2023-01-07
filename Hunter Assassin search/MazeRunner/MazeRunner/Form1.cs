using System; 
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;



namespace MazeRunner
{
    public partial class Form1 : Form
    {
        static string proj_folder = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
        static string sprite_folder = proj_folder + "\\bin\\";
        System.Media.SoundPlayer player = new System.Media.SoundPlayer();
        
        //walking animation
        string dir = "down";
        int walker = 0;

        private const int APPCOMMAND_VOLUME_DOWN = 0x90000;

        private int[,] map;
        Graph graph;
        Point mouse;
        Point cheese;
        List<Point> searched;
        TreeView maintree;

        //simulation
        List<Point> drawing;
        List<Point> mainPath;
        int transferred;

        public Form1()
        {
            InitializeComponent();            
            map = new int[16, 16] { {0,0,1,1,0,0,0,0,0,0,0,0,0,0,1,1},
                                    {0,0,1,3,1,1,1,0,0,0,0,0,0,0,1,1},
                                    {0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,1},
                                    {0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,1},
                                    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                    {1,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0},
                                    {1,0,0,0,0,0,0,0,1,5,1,1,1,1,0,0},
                                    {5,1,1,1,1,0,0,0,1,1,1,1,1,1,1,0},
                                    {1,1,1,1,1,0,0,0,0,1,1,1,1,1,1,0},
                                    {1,1,1,1,1,0,0,0,0,0,0,0,0,1,1,0},
                                    {1,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0},
                                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                    {7,1,1,1,1,1,1,0,0,7,1,1,1,1,1,1},
                                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}};


            mouse = new Point(1,1);
            cheese = new Point(2,3);
            
            searched = new List<Point>();
            drawing = new List<Point>();
            canvas.BackColor = Color.FromArgb(88,88,88);
            player.SoundLocation = sprite_folder+"footsteps-3.wav";
            setGraph();
        }


        public void setGraph()
        {
            List<Vertex> vertices = new List<Vertex>();

            ///Creation of vertices
            for (int r = 0; r < map.GetLength(0); r++)
            {
                for (int c = 0; c < map.GetLength(1); c++)
                {
                    if (map[r, c] == 0)
                    {
                        Vertex v = new Vertex(r + "," + c);
                        v.Location = new Point(c, r);
                        vertices.Add(v);
                        v = null;
                    }
                }
            }
            int i = 3;
            for (int r = cheese.Y; r < map.GetLength(0) && r < cheese.Y + i; r++)
            {
                if (r < 0) r = 0;
                else if (r > 15) r = 15;
                for (int c = cheese.X - i; c < map.GetLength(1) && c < cheese.X + 1 + i; c++)
                {
                    if (c < 0) c = 0;
                    else if (c > 15) c = 15;
                    if (map[r, c] == 0 && new Point(c, r) != new Point(cheese.X, cheese.Y))
                    {
                        map[r, c] = 2;
                    }
                }
            }
            
            double[,] adj_mtx = new double[vertices.Count, vertices.Count];

            ///Connection of vertices
            for (int r = 0; r < map.GetLength(0); r++)
            {
                for (int c = 0; c < map.GetLength(1); c++)
                {
                    if (map[r, c] == 0)
                    {
                        //Up
                        if (r > 0 && map[r - 1, c] == 0)
                        {
                            adj_mtx[vertices.IndexOf(new Vertex(r + "," + c)), vertices.IndexOf(new Vertex((r - 1) + "," + c))] = 1;
                            adj_mtx[vertices.IndexOf(new Vertex((r - 1) + "," + c)), vertices.IndexOf(new Vertex(r + "," + c))] = 1;
                        }
                        //Right
                        if (c < map.GetLength(1) - 1 && map[r, c + 1] == 0)
                        {
                            adj_mtx[vertices.IndexOf(new Vertex(r + "," + c)), vertices.IndexOf(new Vertex(r + "," + (c + 1)))] = 1;
                            adj_mtx[vertices.IndexOf(new Vertex(r + "," + (c + 1))), vertices.IndexOf(new Vertex(r + "," + c))] = 1;
                        }

                        double[,] heuristics = new double[vertices.Count, vertices.Count];
                        /*
                        //heuristics
                        foreach (Vertex v in vertices)
                        {
                            heuristics[vertices.IndexOf(new Vertex(v.Location.Y + "," + v.Location.X)), vertices.IndexOf(new Vertex(r + "," + c))] = heuristics[vertices.IndexOf(new Vertex(r + "," + c)), vertices.IndexOf(new Vertex(v.Location.Y + "," + v.Location.X))] = ComputeDistance(c, r, v.Location.X, v.Location.Y);
                        }*/
                        heuristics[vertices.IndexOf(new Vertex(cheese.Y + "," + cheese.X)), vertices.IndexOf(new Vertex(r + "," + c))] = heuristics[vertices.IndexOf(new Vertex(r + "," + c)), vertices.IndexOf(new Vertex(cheese.Y + "," + cheese.X))] = ComputeDistance(c, r, cheese.X, cheese.Y);
                        graph = new Graph("Map1", vertices.ToArray(), adj_mtx, heuristics);
                    }
                }
            }
        }

        public Bitmap generateImage()
        {
            canvas.Controls.Clear();
            int pixelsize = 50;
            Bitmap b = new Bitmap(800,800);
            Graphics g = Graphics.FromImage(b);
            ///boxes
            for (int r = 0; r < map.GetLength(0); r++)
            {
                for (int c = 0; c < map.GetLength(1); c++)
                {
                    if (map[r, c] == 1)
                    {
                        g.DrawImage(Image.FromFile(sprite_folder+"box.jpg"), c * pixelsize, r * pixelsize);
                    }
                }
            }
            for (int r = 0; r < map.GetLength(0); r++)
            {
                for (int c = 0; c < map.GetLength(1); c++)
                {
                    if (map[r, c] == 3)
                    {
                        g.DrawImage(Image.FromFile(sprite_folder+"box3.jpg"), c * pixelsize, r * pixelsize);
                    }
                }
            }
            for (int r = 0; r < map.GetLength(0); r++)
            {
                for (int c = 0; c < map.GetLength(1); c++)
                {
                    if (map[r, c] == 5)
                    {
                        g.DrawImage(Image.FromFile(sprite_folder+"box5.jpg"), c * pixelsize, r * pixelsize);
                    }
                }
            }
            for (int r = 0; r < map.GetLength(0); r++)
            {
                for (int c = 0; c < map.GetLength(1); c++)
                {
                    if (map[r, c] == 7)
                    {
                        g.DrawImage(Image.FromFile(sprite_folder+"box7.jpg"), c * pixelsize, r * pixelsize);
                    }
                }
            }
            g.DrawImage(Image.FromFile(sprite_folder+"downenemystand.png"), cheese.X * pixelsize, cheese.Y * pixelsize);
           
            PictureBox pic = new PictureBox();
            pic.Location = new Point(mouse.X * pixelsize, mouse.Y * pixelsize);
            pic.BackColor = Color.Transparent;
            if (dir == "up")
                pic.Image = Image.FromFile(sprite_folder+"upstand.jpg");
            else if (dir == "left")
                pic.Image = Image.FromFile(sprite_folder+"leftstand.jpg");
            else if (dir == "down")
                pic.Image = Image.FromFile(sprite_folder+"downstand.jpg");
            else if (dir == "right")
                pic.Image = Image.FromFile(sprite_folder+"rightstand.jpg");
            
            canvas.Controls.Add(pic);
            //searched
            foreach (Point p in drawing)
            {
                g.DrawLine(Pens.White, new Point(p.X * pixelsize, p.Y * pixelsize), new Point((p.X + 1) * pixelsize, (p.Y + 1) * pixelsize));
                g.DrawLine(Pens.White, new Point((p.X + 1) * pixelsize, p.Y * pixelsize), new Point(p.X * pixelsize, (p.Y + 1) * pixelsize));
            }

            ///main path
           
            if (drawing.Count == searched.Count && mainPath != null)
            {
                foreach (Point p in mainPath)
                {
                    g.FillEllipse(Brushes.Black, new Rectangle(p.X * pixelsize, p.Y * pixelsize, pixelsize, pixelsize));
                }
            }
            
            return b;
        }
        private double ComputeDistance(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        private void DFS(Vertex start, Vertex goal)
        {
            foreach (Vertex v in graph.V)
                v.Visited = false;
            mainPath = null;
            searched = null;
            searched = new List<Point>();
            Vertex current = start;
            current.Visited = true;
            Stack<Vertex> stack = new Stack<Vertex>();

            while (!current.Equals(goal))
            {
                int cur_idx = graph.V.ToList().IndexOf(current);
                for (int i = 0; i < graph.AdjacencyMatrix.GetLength(0); i++)
                {
                    if (graph.AdjacencyMatrix[cur_idx, i] > 0 && !graph.V[i].Visited)
                    {
                        graph.V[i].Visited = true;
                        stack.Push(graph.V[i]);
                    }
                }
                current = stack.Pop();
                searched.Add(current.Location);
            }
        }

        private void BFS(Vertex start, Vertex goal)
        {
            foreach (Vertex v in graph.V)
                v.Visited = false;
            mainPath = null;
            searched = null;
            searched = new List<Point>();
            Vertex current = start;
            current.Visited = true;
            Queue<Vertex> queue = new Queue<Vertex>();

            while (!current.Equals(goal))
            {
                int cur_idx = graph.V.ToList().IndexOf(current);
                for (int i = 0; i < graph.AdjacencyMatrix.GetLength(0); i++)
                {
                    if (graph.AdjacencyMatrix[cur_idx, i] > 0 && !graph.V[i].Visited)
                    {
                        graph.V[i].Visited = true;
                        queue.Enqueue(graph.V[i]);
                    }
                }
                current = queue.Dequeue();
                searched.Add(current.Location);
            }
        }

        /// <summary>
        /// Helper function find ancestry
        /// </summary>
        /// <param name="current">Current Tree Node</param>
        /// <param name="ancestor">The Vertex ancestor to be matched</param>
        /// <returns>Returns true if vertex is ancestor to the current tree node, false otherwise.</returns>
        private bool hasAncestor(TreeNode current, Vertex ancestor)
        {
            bool hasAncestor = false;

            while (current != null)
            {
                if (((Vertex)current.Tag).Equals(ancestor))
                {
                    hasAncestor = true;
                    break;
                }
                current = current.Parent;
            }

            return hasAncestor;
        }

        private void UCS(Vertex start, Vertex goal)
        {
            mainPath = null;
            maintree = null;
            maintree = new TreeView();
            searched = null;
            searched = new List<Point>();

            ///Prepping he start vertex node
            Vertex temp = new Vertex(start.Name);
            temp.Location = start.Location;
            temp.Cost = 0;
            TreeNode currNode = new TreeNode(temp.Name);
            currNode.Tag = temp;
            maintree.Nodes.Add(currNode);

            ///Creating the priority queue (list so it can be sorted) and enqueueing the start node
            List<TreeNode> pq = new List<TreeNode>();
            pq.Add(currNode); ///Enqueue for list is Add

            ///UCS iteration algo
            while (pq.Count != 0)
            {
                pq = pq.OrderBy(z => z.Tag).ToList(); //sorting
                currNode = pq.ElementAt(0); //getting the first element in the priority queue
                pq.RemoveAt(0); //Dequeue the first element of the priority queue

                int cur_idx = graph.V.ToList().IndexOf((Vertex)currNode.Tag);
                Vertex currV = (Vertex)currNode.Tag;
                Console.WriteLine(currV.Name + ": " + currV.Cost);
                ///Updating the searched vertex points if it does not exist in the list
                if (!searched.Contains(currV.Location))
                {
                    searched.Add(currV.Location);
                }

                ///Stop if the goal is found
                if (currV.Equals(goal))
                {
                    mainPath = new List<Point>();
                    TreeNode tempNode = currNode;
                    while (tempNode != null)
                    {
                        mainPath.Add(((Vertex)tempNode.Tag).Location);
                        tempNode = tempNode.Parent;
                    }
                    break;
                }

                //Propagating the children of current node
                for (int i = 0; i < graph.V.Length; i++)
                {
                    if (graph.AdjacencyMatrix[cur_idx, i] > 0)
                    {
                        temp = new Vertex(graph.V[i].Name);
                        temp.Location = graph.V[i].Location;
                        temp.Cost = currV.Cost + graph.AdjacencyMatrix[cur_idx, i];
                        TreeNode tempNode = new TreeNode(temp.Name);
                        tempNode.Tag = temp;

                        if (!hasAncestor(currNode, temp))
                        {
                            currNode.Nodes.Add(tempNode); //adding each connecting vertex as children node to that current node if not ancestor
                            pq.Add(tempNode); //adding each tree node to the priority queue
                        }
                    }
                }
            }

        }

        private void GBFS(Vertex start, Vertex goal)
        {
            mainPath = null;
            maintree = null;
            maintree = new TreeView();
            searched = null;
            searched = new List<Point>();

            ///Prepping he start vertex node
            Vertex temp = new Vertex(start.Name);
            temp.Location = start.Location;
            temp.Heuristics = graph.Heuristics[graph.V.ToList().IndexOf(start), graph.V.ToList().IndexOf(goal)];
            TreeNode currNode = new TreeNode(temp.Name);
            currNode.Tag = temp;
            maintree.Nodes.Add(currNode);

            ///Creating the priority queue (list so it can be sorted) and enqueueing the start node
            List<TreeNode> pq = new List<TreeNode>();
            pq.Add(currNode); ///Enqueue for list is Add

            ///GBFS iteration algo
            while (pq.Count != 0)
            {
                pq = pq.OrderBy(z => z.Tag).ToList(); //sorting
                currNode = pq.ElementAt(0); //getting the first element in the priority queue
                pq.RemoveAt(0); //Dequeue the first element of the priority queue

                int cur_idx = graph.V.ToList().IndexOf((Vertex)currNode.Tag);
                Vertex currV = (Vertex)currNode.Tag;
                Console.WriteLine(currV.Name + ": " + currV.Cost);
                ///Updating the searched vertex points if it does not exist in the list
                if (!searched.Contains(currV.Location))
                {
                    searched.Add(currV.Location);
                }

                ///Stop if the goal is found
                if (currV.Equals(goal))
                {
                    mainPath = new List<Point>();
                    TreeNode tempNode = currNode;
                    while (tempNode != null)
                    {
                        mainPath.Add(((Vertex)tempNode.Tag).Location);
                        tempNode = tempNode.Parent;
                    }
                    break;
                }

                //Propagating the children of current node
                for (int i = 0; i < graph.V.Length; i++)
                {
                    if (graph.AdjacencyMatrix[cur_idx, i] > 0)
                    {
                        temp = new Vertex(graph.V[i].Name);
                        temp.Location = graph.V[i].Location;
                        temp.Heuristics = graph.Heuristics[graph.V.ToList().IndexOf(temp), graph.V.ToList().IndexOf(goal)];
                        TreeNode tempNode = new TreeNode(temp.Name);
                        tempNode.Tag = temp;

                        if (!hasAncestor(currNode, temp))
                        {
                            currNode.Nodes.Add(tempNode); //adding each connecting vertex as children node to that current node if not ancestor
                            pq.Add(tempNode); //adding each tree node to the priority queue
                        }
                    }
                }
            }

        }

        private void AStar(Vertex start, Vertex goal)
        {
            mainPath = null;
            maintree = null;
            maintree = new TreeView();
            searched = null;
            searched = new List<Point>();

            ///Prepping he start vertex node
            Vertex temp = new Vertex(start.Name);
            temp.Location = start.Location;
            temp.Cost = 0;
            temp.Heuristics = graph.Heuristics[graph.V.ToList().IndexOf(start), graph.V.ToList().IndexOf(goal)];
            TreeNode currNode = new TreeNode(temp.Name);
            currNode.Tag = temp;
            maintree.Nodes.Add(currNode);

            ///Creating the priority queue (list so it can be sorted) and enqueueing the start node
            List<TreeNode> pq = new List<TreeNode>();
            pq.Add(currNode); ///Enqueue for list is Add

            ///GBFS iteration algo
            while (pq.Count != 0)
            {
                pq = pq.OrderBy(z => z.Tag).ToList(); //sorting
                currNode = pq.ElementAt(0); //getting the first element in the priority queue
                pq.RemoveAt(0); //Dequeue the first element of the priority queue

                int cur_idx = graph.V.ToList().IndexOf((Vertex)currNode.Tag);
                Vertex currV = (Vertex)currNode.Tag;
                Console.WriteLine(currV.Name + ": " + currV.Cost);
                ///Updating the searched vertex points if it does not exist in the list
                if (!searched.Contains(currV.Location))
                {
                    searched.Add(currV.Location);
                }

                ///Stop if the goal is found
                if (currV.Equals(goal))
                {
                    mainPath = new List<Point>();
                    TreeNode tempNode = currNode;
                    while (tempNode != null)
                    {
                        mainPath.Add(((Vertex)tempNode.Tag).Location);
                        tempNode = tempNode.Parent;
                    }
                    break;
                }

                //Propagating the children of current node
                for (int i = 0; i < graph.V.Length; i++)
                {
                    if (graph.AdjacencyMatrix[cur_idx, i] > 0)
                    {
                        temp = new Vertex(graph.V[i].Name);
                        temp.Location = graph.V[i].Location;
                        temp.Cost = currV.Cost + graph.AdjacencyMatrix[cur_idx, i];
                        temp.Heuristics = graph.Heuristics[graph.V.ToList().IndexOf(temp), graph.V.ToList().IndexOf(goal)];
                        TreeNode tempNode = new TreeNode(temp.Name);
                        tempNode.Tag = temp;

                        if (!hasAncestor(currNode, temp))
                        {
                            currNode.Nodes.Add(tempNode); //adding each connecting vertex as children node to that current node if not ancestor
                            pq.Add(tempNode); //adding each tree node to the priority queue
                        }
                    }
                }
            }

        }

        private void simulator_Tick(object sender, EventArgs e)
        {
            if (transferred < searched.Count)
            {
                drawing.Add(searched[transferred++]);
                canvas.Image = generateImage();
            }
            else
            {
                simulator.Enabled = false;
                MessageBox.Show("Finished!");
            }
        }
        private void setter_Tick(object sender, EventArgs e)
        {
            canvas.Image = generateImage();
            setter.Enabled = false;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (walk.Enabled == false && e.KeyCode != Keys.Enter && simulator.Enabled==false)
            {
                if (e.KeyCode == Keys.Left)
                {
                    dir = "left";
                    walk.Enabled = true;
                }
                else if (e.KeyCode == Keys.Down)
                {
                    dir = "down";
                    walk.Enabled = true;
                }
                else if (e.KeyCode == Keys.Right)
                {
                    dir = "right";
                    walk.Enabled = true;
                }
                else if (e.KeyCode == Keys.Up)
                {
                    dir = "up";
                    walk.Enabled = true;
                }
            }
            PictureBox pics = new PictureBox();
            pics.Location = new Point(cheese.X * 50, cheese.Y * 50);
            pics.BackColor = Color.Transparent;
            pics.Image = Image.FromFile(sprite_folder+"downenemystand.png");
            canvas.Controls.Add(pics);
            
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            player.Stop();
            if (simulator.Enabled == false && (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode == Keys.Down || e.KeyCode == Keys.Up))
            {
                canvas.Controls.Clear();
                int pixelsize = 50;
                if (e.KeyCode == Keys.Left)
                {
                    PictureBox pic = new PictureBox();
                    pic.Location = new Point(mouse.X * pixelsize, mouse.Y * pixelsize);
                    pic.BackColor = Color.Transparent;
                    pic.Image = Image.FromFile(sprite_folder+"leftstand.jpg");
                    canvas.Controls.Add(pic);
                    dir = "left";
                }
                else if (e.KeyCode == Keys.Down)
                {
                    PictureBox pic = new PictureBox();
                    pic.Location = new Point(mouse.X * pixelsize, mouse.Y * pixelsize);
                    pic.BackColor = Color.Transparent;
                    pic.Image = Image.FromFile(sprite_folder+"downstand.jpg");
                    canvas.Controls.Add(pic);
                    dir = "down";
                }
                else if (e.KeyCode == Keys.Right)
                {
                    PictureBox pic = new PictureBox();
                    pic.Location = new Point(mouse.X * pixelsize, mouse.Y * pixelsize);
                    pic.BackColor = Color.Transparent;
                    pic.Image = Image.FromFile(sprite_folder+"rightstand.jpg");
                    canvas.Controls.Add(pic);
                    dir = "right";
                }
                else if (e.KeyCode == Keys.Up)
                {
                    PictureBox pic = new PictureBox();
                    pic.Location = new Point(mouse.X * pixelsize, mouse.Y * pixelsize);
                    pic.BackColor = Color.Transparent;
                    pic.Image = Image.FromFile(sprite_folder+"upstand.jpg");
                    canvas.Controls.Add(pic);
                    dir = "up";
                }
                walker = 0;
                walk.Enabled = false;
                PictureBox pics = new PictureBox();
                pics.Location = new Point(cheese.X * 50, cheese.Y * 50);
                pics.BackColor = Color.Transparent;
                pics.Image = Image.FromFile(sprite_folder+"downenemystand.png");
                canvas.Controls.Add(pics);
                
            }
        }


        private void walk_Tick(object sender, EventArgs e)
        {
            int pixelsize = 50;
            if (walker < 4 && mouse.X > -1 && mouse.Y > -1 && mouse.X < 16 && mouse.Y < 16 && simulator.Enabled == false)
            {
                
                if (dir == "up" && mouse.Y > 0 && map[mouse.Y - 1, mouse.X] == 0)
                {
                    player.Play();
                    canvas.Controls.Clear();
                    if (walker == 0)
                    {
                        PictureBox pic = new PictureBox();
                        pic.Location = new Point(mouse.X * pixelsize, (mouse.Y-- * pixelsize) - 25);
                        pic.Size = new System.Drawing.Size(pixelsize,pixelsize+25);
                        pic.BackColor = Color.Transparent;
                        pic.Image = Image.FromFile(sprite_folder+"upr.png");
                        canvas.Controls.Add(pic);
                        dir = "up";
                    }
                    else if (walker == 1)
                    {
                        PictureBox pic = new PictureBox();
                        pic.Location = new Point(mouse.X * pixelsize, --mouse.Y * pixelsize);
                        pic.BackColor = Color.Transparent;
                        pic.Image = Image.FromFile(sprite_folder + "upstand.jpg");
                        canvas.Controls.Add(pic);
                        dir = "up";
                    }
                    else if (walker == 2)
                    {
                        PictureBox pic = new PictureBox();
                        pic.Location = new Point(mouse.X * pixelsize, (mouse.Y-- * pixelsize) - 25);
                        pic.Size = new System.Drawing.Size(pixelsize,pixelsize+25);
                        pic.BackColor = Color.Transparent;
                        pic.Image = Image.FromFile(sprite_folder + "upl.jpg");
                        canvas.Controls.Add(pic);
                        dir = "up";
                    }
                    else if (walker == 3)
                    {
                        PictureBox pic = new PictureBox();
                        pic.Location = new Point(mouse.X * pixelsize, --mouse.Y * pixelsize);
                        pic.BackColor = Color.Transparent;
                        pic.Image = Image.FromFile(sprite_folder + "upstand.jpg");
                        canvas.Controls.Add(pic);
                        dir = "up";
                    }

                }
                else if (dir == "left" && mouse.X > 0 && map[mouse.Y, mouse.X - 1] == 0)
                {
                    player.Play();
                    canvas.Controls.Clear();
                    if (walker == 0)
                    {
                        PictureBox pic = new PictureBox();
                        pic.Location = new Point((mouse.X-- * pixelsize)-25, mouse.Y * pixelsize);
                        pic.BackColor = Color.Transparent;
                        pic.Image = Image.FromFile(sprite_folder + "leftr.png");
                        canvas.Controls.Add(pic);
                        dir = "left";
                    }
                    else if (walker == 1)
                    {
                        PictureBox pic = new PictureBox();
                        pic.Location = new Point(--mouse.X * pixelsize, mouse.Y * pixelsize);
                        pic.BackColor = Color.Transparent;
                        pic.Image = Image.FromFile(sprite_folder + "leftstand.jpg");
                        canvas.Controls.Add(pic);
                        dir = "left";
                    }
                    else if (walker == 2)
                    {
                        PictureBox pic = new PictureBox();
                        pic.Location = new Point((mouse.X-- * pixelsize)-25, mouse.Y * pixelsize);
                        pic.BackColor = Color.Transparent;
                        pic.Image = Image.FromFile(sprite_folder + "leftl.jpg");
                        canvas.Controls.Add(pic);
                        dir = "left";
                    }
                    else if (walker == 3)
                    {
                        PictureBox pic = new PictureBox();
                        pic.Location = new Point(--mouse.X * pixelsize, mouse.Y * pixelsize);
                        pic.BackColor = Color.Transparent;
                        pic.Image = Image.FromFile(sprite_folder + "leftstand.jpg");
                        canvas.Controls.Add(pic);
                        dir = "left";
                    }
                }
                else if (dir == "down" && mouse.Y < 15 && map[mouse.Y + 1, mouse.X] == 0)
                {
                    player.Play();
                    canvas.Controls.Clear();
                    if (walker == 0)
                    {
                        PictureBox pic = new PictureBox();
                        pic.Location = new Point(mouse.X * pixelsize, (mouse.Y++ * pixelsize) + 25);
                        pic.Size = new System.Drawing.Size(pixelsize,pixelsize+25);
                        pic.BackColor = Color.Transparent;
                        pic.Image = Image.FromFile(sprite_folder + "downr.png");
                        canvas.Controls.Add(pic);
                        dir = "down";
                    }
                    else if (walker == 1)
                    {
                        PictureBox pic = new PictureBox();
                        pic.Location = new Point(mouse.X * pixelsize, ++mouse.Y * pixelsize);
                        pic.BackColor = Color.Transparent;
                        pic.Image = Image.FromFile(sprite_folder + "downstand.jpg");
                        canvas.Controls.Add(pic);
                        dir = "down";
                    }
                    else if (walker == 2)
                    {
                        PictureBox pic = new PictureBox();
                        pic.Location = new Point(mouse.X * pixelsize, (mouse.Y++ * pixelsize) + 25);
                        pic.Size = new System.Drawing.Size(pixelsize,pixelsize+25);
                        pic.BackColor = Color.Transparent;
                        pic.Image = Image.FromFile(sprite_folder + "downl.jpg");
                        canvas.Controls.Add(pic);
                        dir = "down";
                    }
                    else if (walker == 3)
                    {
                        PictureBox pic = new PictureBox();
                        pic.Location = new Point(mouse.X * pixelsize, ++mouse.Y * pixelsize);
                        pic.BackColor = Color.Transparent;
                        pic.Image = Image.FromFile(sprite_folder + "downstand.jpg");
                        canvas.Controls.Add(pic);
                        dir = "down";
                    }
                }
                else if (dir == "right" && mouse.X < 15 && map[mouse.Y, mouse.X + 1] == 0)
                {
                    player.Play();
                    canvas.Controls.Clear();
                    if (walker == 0)
                    {
                        PictureBox pic = new PictureBox();
                        pic.Location = new Point((mouse.X++ * pixelsize) + 25, mouse.Y * pixelsize);
                        pic.BackColor = Color.Transparent;
                        pic.Image = Image.FromFile(sprite_folder + "rightr.png");
                        canvas.Controls.Add(pic);
                        dir = "right";
                    }
                    else if (walker == 1)
                    {
                        PictureBox pic = new PictureBox();
                        pic.Location = new Point(++mouse.X * pixelsize, mouse.Y * pixelsize);
                        pic.BackColor = Color.Transparent;
                        pic.Image = Image.FromFile(sprite_folder + "rightstand.jpg");
                        canvas.Controls.Add(pic);
                        dir = "right";
                    }
                    else if (walker == 2)
                    {
                        PictureBox pic = new PictureBox();
                        pic.Location = new Point((mouse.X++ * pixelsize) + 25, mouse.Y * pixelsize);
                        pic.BackColor = Color.Transparent;
                        pic.Image = Image.FromFile(sprite_folder + "rightl.jpg");
                        canvas.Controls.Add(pic);
                        dir = "right";
                    }
                    else if (walker == 3)
                    {
                        PictureBox pic = new PictureBox();
                        pic.Location = new Point(++mouse.X * pixelsize, mouse.Y * pixelsize);
                        pic.BackColor = Color.Transparent;
                        pic.Image = Image.FromFile(sprite_folder + "rightstand.jpg");
                        canvas.Controls.Add(pic);
                        dir = "right";
                    }
                }
                else {
                    player.Stop();
                    canvas.Controls.Clear();
            if (dir == "left")
            {
                PictureBox pic = new PictureBox();
                pic.Location = new Point(mouse.X * pixelsize, mouse.Y * pixelsize);
                pic.BackColor = Color.Transparent;
                pic.Image = Image.FromFile(sprite_folder + "leftstand.jpg");
                canvas.Controls.Add(pic);
                dir = "left";
            }
            else if (dir == "down")
            {
                PictureBox pic = new PictureBox();
                pic.Location = new Point(mouse.X * pixelsize, mouse.Y * pixelsize);
                pic.BackColor = Color.Transparent;
                pic.Image = Image.FromFile(sprite_folder + "downstand.jpg");
                canvas.Controls.Add(pic);
                dir = "down";
            }
            else if (dir == "right")
            {
                PictureBox pic = new PictureBox();
                pic.Location = new Point(mouse.X * pixelsize, mouse.Y * pixelsize);
                pic.BackColor = Color.Transparent;
                pic.Image = Image.FromFile(sprite_folder + "rightstand.jpg");
                canvas.Controls.Add(pic);
                dir = "right";
            }
            else if (dir == "up")
            {
                PictureBox pic = new PictureBox();
                pic.Location = new Point(mouse.X * pixelsize, mouse.Y * pixelsize);
                pic.BackColor = Color.Transparent;
                pic.Image = Image.FromFile(sprite_folder + "upstand.jpg");
                canvas.Controls.Add(pic);
                dir = "up";
            }
            walker = 0;
            
                }
                
                walker = (walker+2)%4;

            }
            if (walker == 2)
                walk.Enabled = false;

            
            PictureBox pics = new PictureBox();
            pics.Location = new Point(cheese.X * 50, cheese.Y * 50);
            pics.BackColor = Color.Transparent;
            pics.Image = Image.FromFile(sprite_folder + "downenemystand.png");
            canvas.Controls.Add(pics);
            if (mouse == cheese)
            {
                System.Media.SoundPlayer player2 = new System.Media.SoundPlayer();
                System.Media.SoundPlayer player1 = new System.Media.SoundPlayer();
                player1.SoundLocation = sprite_folder + "stab.wav";
                player1.Play();
                player2.SoundLocation = sprite_folder + "pain.wav";
                player2.Play();
                canvas.Image = generateImage();


                
            }


        }

        private void uCSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UCS(graph.V[graph.V.ToList().IndexOf(new Vertex(mouse.Y + "," + mouse.X))], graph.V[graph.V.ToList().IndexOf(new Vertex(cheese.Y + "," + cheese.X))]);
            canvas.Image = generateImage();
            simulator.Enabled = true;
            transferred = 0;
            drawing = new List<Point>();
        }

        private void gBFSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GBFS(graph.V[graph.V.ToList().IndexOf(new Vertex(mouse.Y + "," + mouse.X))], graph.V[graph.V.ToList().IndexOf(new Vertex(cheese.Y + "," + cheese.X))]);
            canvas.Image = generateImage();
            simulator.Enabled = true;
            transferred = 0;
            drawing = new List<Point>();
        }

        private void aToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AStar(graph.V[graph.V.ToList().IndexOf(new Vertex(mouse.Y + "," + mouse.X))], graph.V[graph.V.ToList().IndexOf(new Vertex(cheese.Y + "," + cheese.X))]);
            canvas.Image = generateImage();
            simulator.Enabled = true;
            transferred = 0;
            drawing = new List<Point>();
        }
        private void bFSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BFS(graph.V[graph.V.ToList().IndexOf(new Vertex(mouse.Y + "," + mouse.X))], graph.V[graph.V.ToList().IndexOf(new Vertex(cheese.Y + "," + cheese.X))]);
            canvas.Image = generateImage();
            simulator.Enabled = true;
            transferred = 0;
            drawing = new List<Point>();
        }

        private void dFSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DFS(graph.V[graph.V.ToList().IndexOf(new Vertex(mouse.Y + "," + mouse.X))], graph.V[graph.V.ToList().IndexOf(new Vertex(cheese.Y + "," + cheese.X))]);
            canvas.Image = generateImage();
            simulator.Enabled = true;
            transferred = 0;
            drawing = new List<Point>();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawing = new List<Point>();
            mainPath = new List<Point>();
            canvas.Image = generateImage();
        }



    }
    
}
