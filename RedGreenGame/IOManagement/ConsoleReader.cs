namespace RedGreenGame.IOManagement
{
    using RedGreenGame.IOMagement.Contracts;
    using System;

    //Reads input from the Console
    public class ConsoleReader : IReader
    {
        public string Read()
        {
            return Console.ReadLine();
        }
    }
}
