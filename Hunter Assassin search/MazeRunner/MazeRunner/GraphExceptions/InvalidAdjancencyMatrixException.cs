using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeRunner.GraphExceptions
{
    class InvalidAdjancencyMatrixException : Exception
    {
        public InvalidAdjancencyMatrixException(double[,] adjacency_matrix)
            : base("Invalid Adjancency Matrix!")
        {
            this.Source = adjacency_matrix.ToString();
        }
    }
}
