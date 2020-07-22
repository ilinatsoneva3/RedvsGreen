namespace RedGreenGame.Models
{
    using RedGreenGame.IOMagement.Contracts;
    using RedGreenGame.IOManagement;
    using RedGreenGame.Models.Contracts;
    using RedGreenGame.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Linq;


    //Holds information about the game board and methods for populating and updating it 
    public class Board : IBoard
    {
        private readonly IReader reader;
        private readonly IWriter writer;
        private int row;
        private int col;

        //Keeps track of green cells that should become red in this generation
        private IList<Cell> redCells;

        //Keeps track of red cells that should become green in this generation
        private IList<Cell> greenCells;

        public Board()
        {
            this.reader = new ConsoleReader();
            this.writer = new ConsoleWriter();
            this.redCells = new List<Cell>();
            this.greenCells = new List<Cell>();
        }

        //Rows and columns number of the board can be read from outside, but cannot be changed
        public int Row
        {
            get => this.row;

            private set
            {
                if (value <= 0)
                {
                    throw new IndexOutOfRangeException("Row value must be a positive number!");
                }

                this.row = value;
            }
        }
        public int Col
        {
            get => this.col;

            private set
            {
                if (value <= 0)
                {
                    throw new IndexOutOfRangeException("Column value must be a positive number!");
                }

                this.col = value;
            }
        }

        public string[][] Field { get; set; }

        //Initiates field and fills it with values from user's input
        public string PopulateSingleRow(string input, int row)
        {
                var currentRowValue = input
                    .ToCharArray()
                    .Select(s => s.ToString())
                    .ToArray();

                foreach (var value in currentRowValue)
                {
                    if (value!= Helper.Green && value!=Helper.Red)
                    {
                        throw new ArgumentException("Input must consist of green and red values only!");
                    }
                }

                this.Field[row] = currentRowValue;

                return $"Row {row +1} populated successfully";            
        }

        //Iterates through the board, checks which cells should be changed and updates the board
        public void NextGeneration()
        {
            for (int currentRow = 0; currentRow < this.Row; currentRow++)
            {
                for (int currentCol = 0; currentCol < this.Col; currentCol++)
                {
                    if (this.Field[currentRow][currentCol] == Helper.Red) // it's currently red
                    {
                        var shouldTurnToGreen = this.TurnToGreen(currentRow, currentCol);

                        //if a cell meets the criteria to turn green, adds it to the list of red cells to be converted to green
                        if (shouldTurnToGreen)
                        {
                            var cell = new Cell();
                            cell.X = currentRow;
                            cell.Y = currentCol;
                            this.greenCells.Add(cell);
                        }
                    }
                    else //it's currently green
                    {
                        var shouldTurnToRed = TurnToRed(currentRow, currentCol);

                        //if a cell meets the criteria to turn red, adds it to the list of green cell to be converted to red
                        if (shouldTurnToRed)
                        {
                            var cell = new Cell();
                            cell.X = currentRow;
                            cell.Y = currentCol;
                            this.redCells.Add(cell);
                        }
                    }
                }
            }
            this.UpdateField();
        }

        //Prints the board on the console
        public void Print()
        {
            for (int currentRow = 0; currentRow < this.Row; currentRow++)
            {
                writer.Write(string.Join("", this.Field[currentRow]));
            }
        }

        //Sets the board width and height
        public string SetDimensions(string input)
        {
            var dimensions = new int[2];

            try
            {
                dimensions = input
                .Split(", ", StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();
            }
            catch (FormatException)
            {
                throw new FormatException("Input could not be parsed!");
            }

            if (dimensions.Length != 2)
            {
                throw new InvalidOperationException("Input must contain two numbers!");
            }
           

            this.Row = dimensions[0];
            this.Col = dimensions[1];
            this.Field = new string[this.Row][];

            return "Board width and height are set successfully.";
        }


        //Changes all cells values to their new ones and empties the two lists to be ready for the next generation
        private void UpdateField()
        {
            for (int i = 0; i < this.redCells.Count; i++)
            {
                var currentCell = this.redCells[i];
                this.Field[currentCell.X][currentCell.Y] = Helper.Red;
            }

            for (int i = 0; i < this.greenCells.Count; i++)
            {
                var currentCell = this.greenCells[i];
                this.Field[currentCell.X][currentCell.Y] = Helper.Green;
            }

            this.redCells.Clear();
            this.greenCells.Clear();
        }

        //Returns true or false depending on whether a green cell meets the criteria to become red
        private bool TurnToRed(int currentRow, int currentCol)
        {
            //If the cell has 2, 3 or 6 green neighbours, it should stay the same; otherwise, it should become red
            var totalGreenNeighboursCount = FindGreenNeighboursCount(currentRow, currentCol);

            if (totalGreenNeighboursCount == Helper.GreenNeighbourValueOne
                || totalGreenNeighboursCount == Helper.GreenNeighbourValueTwo
                || totalGreenNeighboursCount == Helper.GreenNeighbourValueThree)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        //Returns true or false depending on whether a red cell meets the criteria to become green
        private bool TurnToGreen(int currentRow, int currentCol)
        {
            //If the cell has 3 or 6 green neighbours, it should turn to green
            var totalGreenNeighboursCount = FindGreenNeighboursCount(currentRow, currentCol);

            if (totalGreenNeighboursCount == Helper.RedNeighbourValueOne
                || totalGreenNeighboursCount == Helper.RedNeighbourValueTwo)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private int FindGreenNeighboursCount(int row, int col)
        {
            var neighboursCount = 0;

            //Checks if coordinates of previous and next cells are below zero or greater than board size
            var startRow = row - 1 < 0 ? row : row - 1;
            var startCol = col - 1 < 0 ? col : col - 1;
            var endRow = row + 1 > this.Row ? row : row + 1;
            var endCol = col + 1 > this.Col ? col : col + 1;


            for (int currentRow = startRow; currentRow <= endRow; currentRow++)
            {
                for (int currentCol = startCol; currentCol <= endCol; currentCol++)
                {
                    //Checks if it's not the main cell 
                    if (!((currentRow == row) && (currentCol == col)))
                    {
                        //Checks if neighbour cell is within grid and equal to green
                        if (IsCellInField(currentRow, currentCol) && this.Field[currentRow][currentCol] == Helper.Green)
                        {
                            neighboursCount++;
                        }
                    }
                }
            }

            return neighboursCount;
        }

        private bool IsCellInField(int rowNumber, int colNumber)
        {
            if ((rowNumber < 0) || (colNumber < 0))
            {
                return false;    //false if row or col are negative
            }
            if (rowNumber >= this.Row || colNumber >= this.Col)
            {
                return false;    //false if row or col are more than matrix size
            }
            return true;
        }


    }
}
