using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MazeRunner.GraphExceptions;

namespace MazeRunner
{
    class Graph
    {
        public string Name { get; set; }

        public Vertex[] V { get; private set; }

        public double[,] AdjacencyMatrix { get; private set; }

        public double[,] Heuristics { get; private set; }

        public Graph(string name, Vertex[] v, double[,] adjacency_matrix, double[,] heuristics)
        {
            this.Name = name;
            this.V = (Vertex[])v.Clone();

            if (adjacency_matrix.GetLength(0) != v.Length)
                throw new InvalidAdjancencyMatrixException(adjacency_matrix);
            else
                this.AdjacencyMatrix = (double[,])adjacency_matrix.Clone();

            if (heuristics.GetLength(0) != v.Length)
                throw new InvalidHeuristicsMatrixException(heuristics);
            else
                this.Heuristics = (double[,])heuristics.Clone();
        }
    }
}
