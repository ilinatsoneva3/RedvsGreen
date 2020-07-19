using System;

namespace RedGreenGame.Utilities
{
    public static class ConsoleMessages
    {
        public static string EnterFieldDimensions = "Please enter field width and height, separated by comma:";

        public static string EnterRedAndGreenPlayersPositions = "Please enter red and green players positions on the field."
            + Environment.NewLine + "Greens are represented by 1 and reds are represented by 0.";

        public static string EnterStartPositionAndRotations 
            = "Please enter your player's corodinates and the number of rotations, separated by comma:";

        public static string FinalOutput = "Your player was {0} times green during {1} generations.";
    }
}
