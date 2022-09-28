using System;
namespace Assets.Scripts
{
    public class LevelFileLoader
    {
        public static string[] LoadLevelFromFile(string folderName, int levelNumber)
        {
            var mapText = FileLoader.LoadFile(folderName + "/" + ConvertIntToStringWithDigits(levelNumber, 3));
            string[] seperator = { "\n" };
            return mapText.Split(seperator, StringSplitOptions.RemoveEmptyEntries);
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