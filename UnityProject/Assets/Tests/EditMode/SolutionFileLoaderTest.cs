using NUnit.Framework;
using System.Collections.Generic;
using Assets.Scripts;
using System.Linq;
using UnityEngine.TestTools;

namespace Tests
{
    public class SolutionFileLoaderTest
    {
        [Test]
        public void CanLoadSolutionForLevel2()
        {
            //assing
            var expectedResult = new List<Direction>()
            {
                new Direction(Direction.DirectionLetter.Right),
                new Direction(Direction.DirectionLetter.Down),
                new Direction(Direction.DirectionLetter.Left)
            };
            //act
            var actualResult = SolutionFileLoader.LoadSolutionFromFile(Configurations.solutionsFolder, 2);
            //assert
            Assert.AreEqual(expectedResult.Count, actualResult.Count);
            Assert.IsTrue(expectedResult.Except(actualResult).Any());
            Assert.IsTrue(actualResult.Except(expectedResult).Any());
        }
    }
}