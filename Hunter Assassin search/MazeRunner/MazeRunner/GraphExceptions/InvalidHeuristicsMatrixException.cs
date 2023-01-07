using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeRunner.GraphExceptions
{
    class InvalidHeuristicsMatrixException : Exception
    {
        public InvalidHeuristicsMatrixException(double[,] heuristics)
            : base("Invalid Heuristics Matrix!")
        {
            this.Source = heuristics.ToString();
        }
    }
}
