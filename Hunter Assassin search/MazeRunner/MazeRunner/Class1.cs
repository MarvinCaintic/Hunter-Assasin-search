using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
namespace MazeRunner
{
    class node
    {
        public node prev{ get; set; }
        public Point current{ get; set; }

        public node()
        {
            current = new Point(100, 100);
            prev = null;
        }
    }
}
