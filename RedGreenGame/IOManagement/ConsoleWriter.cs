namespace RedGreenGame.IOManagement
{
    using RedGreenGame.IOMagement.Contracts;
    using System;

    //Writes to the Console
    //Supports string and number only writing
    public class ConsoleWriter : IWriter
    {
        public void Write(string text)
        {
            Console.WriteLine(text);
        }

        public void Write(int number)
        {
            Console.WriteLine(number);
        }
    }
}
