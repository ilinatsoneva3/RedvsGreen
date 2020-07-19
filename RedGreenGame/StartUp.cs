namespace RedGreenGame
{
    using System;

    public class StartUp
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;

            //Initiates game engine
            var engine = new GameEngine();
            engine.Run();
        }
    }
}
