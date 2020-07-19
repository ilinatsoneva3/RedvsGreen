namespace RedGreenGame.Models
{
    using RedGreenGame.Models.Contracts;

    //Holds information about cell's coordinates
    public class Cell : ICell
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
