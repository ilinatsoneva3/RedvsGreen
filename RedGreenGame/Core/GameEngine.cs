namespace RedGreenGame
{
    using RedGreenGame.IOManagement;
    using RedGreenGame.Models;
    using RedGreenGame.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class GameEngine
    {
        private Board board;
        private int rowX;
        private int colX;
        private int rotations;
        private int numberOfGreenGenerations;
        private readonly ConsoleReader reader;
        private readonly ConsoleWriter writer;

        //When initialized, the Game engine immediately initializes a new Board
        public GameEngine()
        {
            this.board = new Board();
            this.reader = new ConsoleReader();
            this.writer = new ConsoleWriter();
        }

        //Core logic of the game
        public void Run()
        {
            //Sets width and height
            writer.Write(ConsoleMessages.EnterFieldDimensions);

            var input = this.reader.Read();

            this.board.SetDimensions(input);


            //Fill board with first Generation data
            writer.Write(ConsoleMessages.EnterRedAndGreenPlayersPositions);

            this.board.Populate();

            //Find which cell is to be tracked and how many rotations there are
            writer.Write(ConsoleMessages.EnterStartPositionAndRotations);
            this.ReadNextConditions();

            //Check if the cell to track is green in the first Generation
            var isGreen = this.CheckIfCellIsGreen();

            //Check if the cell to track is green in the initial Generation and increases the number of times the cell is green
            if (isGreen)
            {
                this.numberOfGreenGenerations++;
            }

            //Iterate through field and change generations
            this.Rotate();

            this.writer.Write(string.Format(ConsoleMessages.FinalOutput, numberOfGreenGenerations, this.rotations + 1));
        }

        //Changes the board generations by the number specified
        private void Rotate()
        {
            for (int i = 0; i < this.rotations; i++)
            {
                //Generates next generation data
                this.board.NextGeneration();

                //Check if the cell to track is green in the initial Generation and increases the number of times the cell is green
                var isGreen = this.CheckIfCellIsGreen();

                if (isGreen)
                {
                    this.numberOfGreenGenerations++;
                }
            }
        }

        //Returns true or false depending on whether the tracked cell is green
        private bool CheckIfCellIsGreen()
        {
            return this.board.Field[this.rowX][this.colX] == Helper.Green;
        }

        //The tracked cell row and column number and total number of rotations are given on a single row
        //This method reads the input and assigns the correct values to the fields
        private void ReadNextConditions()
        {
            var input = this.reader
                .Read();

            var result = this.ValidateInput(input);
           
            this.rowX = result[0];
            this.colX = result[1];
            this.rotations = result[2];
        }

        private int[] ValidateInput(string input)
        {
            var result = new int[3];

            //Check if there is any input from user
            if (String.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException("Input cannot be null or empty!");
            }

            try //Check if input can be successfully parsed
            {
                result = input.Split(", ", StringSplitOptions.RemoveEmptyEntries)
               .Select(int.Parse)
               .ToArray();
            }
            catch (FormatException)
            {
                throw new FormatException("Input could not be parsed!");
            }

            if (result.Length != 3) //input must be exactly three numbers
            {
                throw new InvalidOperationException("Input must contain three numbers!");
            }

            //Check if player position is in the field            
            if (result[0] < 0 || result[1] < 0)
            {
                throw new IndexOutOfRangeException("Row and column number cannot be less than zero!");
            }
            else if (result[0] > this.board.Row || result[1] > this.board.Col)
            {
                throw new IndexOutOfRangeException("Player position must be in the field!");
            }

            //Check if number of rotations is less or equal to zero
            if (result[2] <= 0)
            {
                throw new InvalidOperationException("Number of rotations must be positive!");
            }

            return result;
        }
    }
}
