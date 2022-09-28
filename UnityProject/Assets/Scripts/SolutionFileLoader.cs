using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Assets.Scripts
{
    public class SolutionFileLoader
    {
        public static List<Direction> LoadSolutionFromFile(string folderName, int levelNumber)
        {
            var solutionAsString = FileLoader.LoadFile(folderName + "/" + ConvertIntToStringWithDigits(levelNumber, 3));
            string[] seperator = { ";" };
            string[] solutionAsStringList = solutionAsString.Split(seperator, StringSplitOptions.RemoveEmptyEntries);
            var solutionAsDirections = new List<Direction>();
            foreach(var solutionString in solutionAsStringList)
            {
                switch (solutionString)
                {
                    case "r":
                        solutionAsDirections.Add(new Direction(Direction.DirectionLetter.Right));
                        break;
                    case "l":
                        solutionAsDirections.Add(new Direction(Direction.DirectionLetter.Left));
                        break;
                    case "u":
                        solutionAsDirections.Add(new Direction(Direction.DirectionLetter.Up));
                        break;
                    case "d":
                        solutionAsDirections.Add(new Direction(Direction.DirectionLetter.Down));
                        break;
                    default:
                        Debug.Write("Not valid letter in SolutionString: " + solutionString);
                        break;
                }
            }
            return solutionAsDirections;
        }

        private static string ConvertIntToStringWithDigits(int number, int amountOfdigits)
        {
            var numberAsString = number.ToString();
            while (numberAsString.Length < amountOfdigits)
            {
                numberAsString = numberAsString.Insert(0, "0");
            }
            return numberAsString;
        }
    }
}
