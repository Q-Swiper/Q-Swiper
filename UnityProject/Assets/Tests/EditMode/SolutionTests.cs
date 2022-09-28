using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assets.Scripts;

namespace Tests
{
    public class SolutionTests
    {
        private bool LevelCanBeSolvedWithItsSolution(int levelNumber)
        {
            var solution = SolutionFileLoader.LoadSolutionFromFile(Configurations.solutionsFolder, levelNumber);
            if(solution.Count == 0)
            {
                return false;
            }
            var level = new LevelController(Configurations.levelMapsFolder, levelNumber);
            for (var i = 0; i < solution.Count; i++)
            {
                level.ReactOnUserMove(solution[i]);
            }
            if (level.AreAllCollectablesCollected())
            {
                return true;
            }
            return false;
        }

        [Test]
        public void SolutionForLevel001()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(1));
        }
        [Test]
        public void SolutionForLevel002()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(2));
        }
        [Test]
        public void SolutionForLevel003()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(3));
        }
        [Test]
        public void SolutionForLevel004()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(4));
        }
        [Test]
        public void SolutionForLevel005()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(5));
        }
        [Test]
        public void SolutionForLevel006()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(6));
        }
        [Test]
        public void SolutionForLevel007()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(7));
        }
        [Test]
        public void SolutionForLevel008()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(8));
        }
        [Test]
        public void SolutionForLevel009()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(9));
        }
        [Test]
        public void SolutionForLevel010()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(10));
        }
        [Test]
        public void SolutionForLevel011()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(11));
        }
        [Test]
        public void SolutionForLevel012()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(12));
        }
        [Test]
        public void SolutionForLevel013()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(13));
        }
        [Test]
        public void SolutionForLevel014()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(14));
        }
        [Test]
        public void SolutionForLevel015()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(15));
        }
        [Test]
        public void SolutionForLevel016()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(16));
        }
        [Test]
        public void SolutionForLevel017()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(17));
        }
        [Test]
        public void SolutionForLevel018()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(18));
        }
        [Test]
        public void SolutionForLevel019()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(19));
        }
        [Test]
        public void SolutionForLevel020()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(20));
        }
        [Test]
        public void SolutionForLevel021()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(21));
        }
        [Test]
        public void SolutionForLevel022()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(22));
        }
        [Test]
        public void SolutionForLevel023()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(23));
        }
        [Test]
        public void SolutionForLevel024()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(24));
        }
        [Test]
        public void SolutionForLevel025()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(25));
        }
        [Test]
        public void SolutionForLevel026()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(26));
        }
        [Test]
        public void SolutionForLevel027()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(27));
        }
        [Test]
        public void SolutionForLevel028()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(28));
        }
        [Test]
        public void SolutionForLevel029()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(29));
        }
        [Test]
        public void SolutionForLevel030()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(30));
        }
        [Test]
        public void SolutionForLevel031()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(31));
        }
        [Test]
        public void SolutionForLevel032()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(32));
        }
        [Test]
        public void SolutionForLevel033()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(33));
        }
        [Test]
        public void SolutionForLevel034()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(34));
        }
        [Test]
        public void SolutionForLevel035()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(35));
        }
        [Test]
        public void SolutionForLevel036()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(36));
        }
        [Test]
        public void SolutionForLevel037()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(37));
        }
        [Test]
        public void SolutionForLevel038()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(38));
        }
        [Test]
        public void SolutionForLevel039()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(39));
        }
        [Test]
        public void SolutionForLevel040()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(40));
        }
        [Test]
        public void SolutionForLevel041()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(41));
        }
        [Test]
        public void SolutionForLevel042()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(42));
        }
        [Test]
        public void SolutionForLevel043()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(43));
        }
        [Test]
        public void SolutionForLevel044()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(44));
        }
        [Test]
        public void SolutionForLevel045()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(45));
        }
        [Test]
        public void SolutionForLevel046()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(46));
        }
        [Test]
        public void SolutionForLevel047()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(47));
        }
        [Test]
        public void SolutionForLevel048()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(48));
        }
        [Test]
        public void SolutionForLevel049()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(49));
        }
        [Test]
        public void SolutionForLevel050()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(50));
        }/*
        [Test]
        public void SolutionForLevel051()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(51));
        }
        [Test]
        public void SolutionForLevel052()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(52));
        }
        [Test]
        public void SolutionForLevel053()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(53));
        }
        [Test]
        public void SolutionForLevel054()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(54));
        }
        [Test]
        public void SolutionForLevel055()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(55));
        }
        [Test]
        public void SolutionForLevel056()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(56));
        }
        [Test]
        public void SolutionForLevel057()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(57));
        }
        [Test]
        public void SolutionForLevel058()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(58));
        }
        [Test]
        public void SolutionForLevel059()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(59));
        }
        [Test]
        public void SolutionForLevel060()
        {
            Assert.IsTrue(LevelCanBeSolvedWithItsSolution(60));
        }*/
    }
}