using System;
using System.Threading;

namespace GameOfLife
{
    class Game
    {
        // game boad, world of cells
        private Board board;

        // number of world iterations
        private Int64 iteration;

        // default time (ms) wait till next iteration
        private int iterationTime = 100;

        // available time (ms) for iteration period
        private int[] availableIterationTimes = { 10, 20, 50, 100, 200, 350, 500, 1000, 2000 };

        // Iteration type:
        // "time" - next world iteration after automatically after n ms
        // "enter" - next iteartion manually by pressing "ENTER"
        private string iterationCondition = "time";

        // When iteration is "enter" read key for command
        private string manualIterationCommand = "";

        public void Start()
        {
            this.ShowWelcomeScreen();
            this.NewGame();
            Console.Read();
        }

        public void NewGame()
        {
            this.iteration = 0;
            this.Init();
            this.Simulate();
        }

        private void ShowWelcomeScreen()
        {
            Console.WriteLine("Conway's Game of Life simulation in C#");
            Console.WriteLine("\nBy Nerijus Zaniauskas for C# learning purpose");
            Console.WriteLine("\nPick your terminal size for simulation and press ENTER to continue..");
            Console.Read();
        }

        // create new board and generate starting setup
        private void Init()
        {
            int x = int.Parse(Console.BufferWidth.ToString());
            int y = int.Parse(Console.BufferHeight.ToString()) - 4;

            this.board = new Board(x, y);
            Random rnd = new Random();

            int iterationWithoutAdd = 0;
            do
            {
                int randX = rnd.Next(0, x);
                int randY = rnd.Next(0, y);

                if (!this.board.Get(randX, randY).MakeActiveIfAllowed()) {
                    iterationWithoutAdd++;
                }
                else {
                    Console.Clear();
                    Console.WriteLine(String.Format("Board filled: {0}%", board.GetActivePercentage()));
                }
            }
            while (iterationWithoutAdd != 1000);

            Console.Clear();
            Console.CursorTop = 1;
            Console.WriteLine("Initial configuration generated. To start press ENTER..");

            this.board.Draw(3);
            Console.ReadLine();
            Console.Clear();
        }

        private void Simulate()
        {
            bool changed;
            do
            {
                this.ProcessInput();

                changed = this.board.MakeTransition();

                if (!this.board.IsFinished()) {
                    this.iteration++;
                }

                this.Draw();
                this.NextIteration();
            }
            while (changed);
        }

        private void Draw()
        {
            Console.CursorTop = 0;
            Console.CursorLeft = 0;
            Console.WriteLine("M - manual iteration (ENTER key) F - faster speed S - slower speed N - new game");
            Console.WriteLine(String.Format("Iteration time: {0}ms Iteration: {1}", this.iterationTime, this.iteration));

            if (this.board.IsFinished()) {
                Console.WriteLine("Simulation FINISHED! Start new simulation press: \"N\"");
            }

            this.board.Draw(3);
        }

        private void NextIteration()
        {
            if (this.iterationCondition == "time")
            {
                Thread.Sleep(this.iterationTime);
            }
            else if (this.iterationCondition == "enter")
            {
                this.manualIterationCommand = Console.ReadLine();
            }
            else
            {
                throw new Exception("Unknown next iteration condition: " + this.iterationCondition);
            }
        }

        private string getCommand()
        {
            if (this.manualIterationCommand != "")
            {
                string command = this.manualIterationCommand;
                this.manualIterationCommand = "";

                return command;
            }

            if (Console.KeyAvailable)
            {
                return Console.ReadKey(true).Key.ToString();
            }

            return "";
        }

        private void ProcessInput()
        {
            string command = this.getCommand();

            if (command != "")
            {
                int index;

                switch (command)
                {
                    case "s":
                    case "S":
                        index = Array.IndexOf(this.availableIterationTimes, this.iterationTime);
                        if (index + 1 < this.availableIterationTimes.Length)
                        {
                            this.iterationTime = this.availableIterationTimes[index + 1];
                        }

                        this.iterationCondition = "time";

                        break;

                    case "f":
                    case "F":
                        index = Array.IndexOf(this.availableIterationTimes, this.iterationTime);
                        if (index > 0) {
                            this.iterationTime = this.availableIterationTimes[index - 1];
                        }

                        this.iterationCondition = "time";

                        break;

                    case "m":
                    case "M":
                        this.iterationCondition = "enter";

                        break;

                    case "n":
                    case "N":
                        this.NewGame();

                        break;
                }

                Console.Clear();
            }
        }
    }
}
