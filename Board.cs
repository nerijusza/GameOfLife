using System;
            
namespace GameOfLife
{
    class Board
    {
        private int SizeX;
        private int SizeY;

        private Cell[] grid;

        // active cell display symbol in console
        private string symbol = "X";

        // last 3 boar status to check if we finished world evolve
        private string[] last3Status;

        // internal class if world evolve is finished
        private bool Finished = false;

        public Board(int sizeX, int sizeY)
        {
            this.SizeX = sizeX;
            this.SizeY = sizeY;

            this.last3Status = new string[3];

            this.Init();
        }

        private void Init()
        {
            this.grid = new Cell[this.SizeX * this.SizeY];

            int index = 0;
            for (int y = 0; y < this.SizeY; y++)
            {
                for (int x = 0; x < this.SizeX; x++)
                {
                    this.grid[index] = new Cell(x, y);
                    index++;
                }
            }

            index = 0;
            for (int y = 0; y < this.SizeY; y++)
            {
                for (int x = 0; x < this.SizeX; x++)
                {
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            int keyX = x + i;
                            int keyY = y + j;

                            if ((x != keyX || y != keyY) && keyX >= 0 && keyX < this.SizeX && keyY >= 0 && keyY < this.SizeY)
                            {
                                this.grid[index].AddNeighbor(this.Get(keyX, keyY));
                            }
                        }
                    }
                    index++;
                }
            }
        }

        public Cell Get(int x, int y)
        {
            int index = this.SizeX * y + x;

            Cell cell = this.grid[index];

            if (cell.X != x || cell.Y != y)
            {
                foreach (Cell cell1 in this.grid)
                {
                    Console.WriteLine(cell1);
                }

                throw new Exception(String.Format("Need ({0},{1}), got ({2},{3}), index, {4}", x, y, cell.X, cell.Y, index));
            }

            return cell;
        }

        // Draw world in console skipping n first lines
        public void Draw(int skipLine = 0)
        {
            Console.CursorLeft = 0;
            Console.CursorTop = skipLine;

            int prewY = 0;
            foreach (Cell cell in this.grid)
            {
                if (prewY != cell.Y) {
                    Console.Write("\n");
                }
                prewY = cell.Y;

                if (cell.Status) {
                    Console.Write(this.symbol);
                }
                else
                {
                    Console.Write(" ");
                }
            }
        }

        public float GetActivePercentage()
        {
            int active = 0;
            foreach (Cell cell in this.grid)
            {
                if (cell.Status) {
                    active++;
                }
            }

            float percentage = (float)active / (float)this.grid.Length * 100;

            return (float) Math.Round(percentage, 2);
        }

        public bool MakeTransition()
        {
            bool changed = false;

            foreach (Cell cell in this.grid) {
                int neigbours = cell.CountActiveNeigbours();

                //Any live cell with fewer than two live neighbors dies, as if by under population.
                //Any live cell with two or three live neighbors lives on to the next generation.
                //Any live cell with more than three live neighbors dies, as if by overpopulation.
                //Any dead cell with exactly three live neighbors becomes a live cell, as if by reproduction.

                if (cell.Status)
                {
                    if (neigbours < 2 || neigbours > 3)
                    {
                        cell.NextStatus = false;
                    }
                    else
                    {
                        cell.NextStatus = true;
                    }
                }
                else
                {
                    if (neigbours == 3) {
                        cell.NextStatus = true;
                    }
                    else
                    {
                        cell.NextStatus = false;
                    }
                }
            }

            foreach (Cell cell in this.grid)
            {
                if (cell.Status != cell.NextStatus)
                {
                    changed = true;
                }
                cell.Status = cell.NextStatus;
            }

            this.updateFinishedStatus();

            return changed;
        }

        private void updateFinishedStatus()
        {
            string status = "";
            int i = 0;
            foreach (Cell cell in this.grid)
            {
                status = status + (cell.Status ? "1" : "0");
                i++;
            }

            if (Array.IndexOf(this.last3Status, status) > -1)
            {
                this.Finished = true;
            }

            this.last3Status[2] = this.last3Status[1];
            this.last3Status[1] = this.last3Status[0];
            this.last3Status[0] = status;
        }

        public bool IsFinished()
        {
            return this.Finished;
        }
    }
}
