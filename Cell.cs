using System;
using System.Collections.Generic;

namespace GameOfLife
{
    class Cell
    {
        public int X;
        public int Y;

        public bool Status;
        public bool NextStatus;

        public List<Cell> Neighbors = new List<Cell>();

        public Cell(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public void AddNeighbor(Cell cell)
        {
            this.Neighbors.Add(cell);
        }

        public int GetNeighborCount()
        {
            return this.Neighbors.Count;
        }

        public bool MakeActiveIfAllowed()
        {
            if (this.Status)
            {
                return false;
            }

            this.Status = true;

            if (this.getActiveNeighbors().Count > 3) {
                this.Status = false;
                return false;
            }

            foreach (Cell neighbour in this.Neighbors)
            {
                if (neighbour.Status && neighbour.getActiveNeighbors().Count > 3)
                {
                    this.Status = false;
                    return false;
                }
            }

            return true;
        }

        public List<Cell> getActiveNeighbors()
        {
            List<Cell> activeNeighbours = new List<Cell>();

            foreach (Cell neighbour in this.Neighbors) {
                if (neighbour.Status) {
                    activeNeighbours.Add(neighbour);
                }
            }

            return activeNeighbours;
        }

        public int CountActiveNeigbours()
        {
            int count = 0;

            foreach (Cell neighbour in this.Neighbors)
            {
                if (neighbour.Status)
                {
                    count++;
                }
            }

            return count;
        }

        public override string ToString()
        {
            string neighbors = "";

            foreach (Cell cell in this.Neighbors)
            {
                neighbors = neighbors + String.Format(" ({0} {1} {2})", cell.X, cell.Y, cell.Status);
            }

            return String.Format("{0} {1} {2} {3} >{4} {5}< {6}", this.X, this.Y, this.Status, this.NextStatus, this.GetNeighborCount(), this.getActiveNeighbors().Count, neighbors);
        }
    }

}
