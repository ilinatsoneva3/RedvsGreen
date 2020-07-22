namespace RedGreenGame.Models.Contracts
{
    //Interface for the game board

    public interface IBoard
    {
        public int Row { get; }

        public int Col { get; }

        public string[][] Field { get; set; }

        void Populate();

        public void NextGeneration();

        public void Print();

        public void SetDimensions(string input);
    }
}
