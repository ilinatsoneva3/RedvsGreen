namespace RedGreenGame.Models.Contracts
{
    //Interface for the game board

    public interface IBoard
    {
        public int Row { get; }

        public int Col { get; }

        public string[][] Field { get; set; }

        public string PopulateSingleRow(string input, int row);

        public void NextGeneration();

        public void Print();

        public string SetDimensions(string input);
    }
}
