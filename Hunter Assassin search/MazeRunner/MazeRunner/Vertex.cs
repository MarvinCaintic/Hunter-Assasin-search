using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MazeRunner
{
    class Vertex : IComparable
    {
        public string Name { get; set; }
        public Point Location { get; set; }
        public bool Visited { get; set; }
        public double Cost { get; set; }
        public double Heuristics { get; set; }

        /// <summary>
        /// Vertex Constructor
        /// </summary>
        /// <param name="name">Name of the vertex</param>
        public Vertex(string name)
        {
            this.Name = name;
            this.Location = Point.Empty;
            this.Visited = false;
            this.Cost = 0;
            this.Heuristics = 0;
        }

        public int CompareTo(object o)
        {
            int result = 0;
            if (o is Vertex)
            {
                Vertex v = (Vertex)o;

                if ((this.Cost + this.Heuristics) < (v.Cost + v.Heuristics))
                    result = -1;
                else if ((this.Cost + this.Heuristics) > (v.Cost + v.Heuristics))
                    result = 1;
                else
                    result = 0;

                if (result == 0)
                {
                    result = this.Name.CompareTo(v.Name);
                }
            }
            else
                throw new Exception("Object must be a Vertex!");

            return result;
        }

        public string ToVertexDisplay()
        {
            return this.Name + "(Cost:" + this.Cost + ", Heuristics: " + this.Heuristics + ", Total: " + (this.Cost + this.Heuristics) + ")";
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Vertex))
                return false;
            else
                return this.Name == ((Vertex)obj).Name;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
