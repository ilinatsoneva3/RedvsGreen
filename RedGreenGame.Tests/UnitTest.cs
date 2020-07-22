namespace RedGreenGame.Tests
{
    using NUnit.Framework;
    using RedGreenGame.Models;
    using System;

    public class Tests
    {
        public Board board;
        public GameEngine gameEngine;

        [SetUp]
        public void Setup()
        {
            this.board = new Board();
            this.gameEngine = new GameEngine();
        }

        [Test]
        public void TestDimensionsAreValid()
        {
            var result = this.board.SetDimensions("2, 2");
            Assert.AreEqual(2, this.board.Row);
            Assert.AreEqual(2, this.board.Col);
            Assert.AreEqual("Board width and height are set successfully.", result);
        }

        [Test]
        public void TestWithWrongInput()
        {
            Assert.Throws<FormatException>(() =>
            {
                this.board.SetDimensions("2, s");
            });
        }

        [Test]
        public void TestDimensionsInputLength()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                this.board.SetDimensions("2, 2, 2");
            });
        }

        [Test]
        public void TestZeroAndNegativeDimensionsInput()
        {
            Assert.Throws<IndexOutOfRangeException>(() =>
            {
                this.board.SetDimensions("0, 1");
            });

            Assert.Throws<IndexOutOfRangeException>(() =>
            {
                this.board.SetDimensions("-1, 1");
            });

            Assert.Throws<IndexOutOfRangeException>(() =>
            {
                this.board.SetDimensions("1, 0");
            });

            Assert.Throws<IndexOutOfRangeException>(() =>
            {
                this.board.SetDimensions("1, -1");
            });
        }

        [Test]
        public void TestPopulateWorksCorrectly()
        {
            this.board.SetDimensions("2, 2");

            for (int i = 0; i < 2; i++)
            {
                var result = this.board.PopulateSingleRow("11", i);
                Assert.AreEqual($"Row {i + 1} populated successfully", result);
                Assert.AreEqual("11", this.board.Field[i]);
            }
        }

        [Test]
        public void TestPopulateThrowsErrorForIncorrectValues()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                this.board.SetDimensions("1, 1");
                this.board.PopulateSingleRow("2, 1", 0);
            });

            Assert.Throws<ArgumentException>(() =>
            {
                this.board.SetDimensions("1, 1");
                this.board.PopulateSingleRow("1, 2", 0);
            });
        }

        [Test]
        public void TestNextGenerationWorksCorrectlyForChangeOfGreenPlayerToRed()
        {
            this.board.SetDimensions("2, 2");
            this.board.PopulateSingleRow("11", 0);
            this.board.PopulateSingleRow("00", 1);
            this.gameEngine.ReadPlayerPositionAndNumberOfRotations("0, 0, 1");
            this.board.NextGeneration();

            var result = string.Join("", this.board.Field[0]);

            Assert.AreEqual("00", result);
        }

        [Test]
        public void TestNextGenerationWorksCorrectlyForChangeOfRedPlayerToGreen()
        {
            this.board.SetDimensions("2, 2");
            this.board.PopulateSingleRow("01", 0);
            this.board.PopulateSingleRow("11", 1);
            this.gameEngine.ReadPlayerPositionAndNumberOfRotations("0, 0, 1");
            this.board.NextGeneration();

            var result = string.Join("", this.board.Field[0]);

            Assert.AreEqual("11", result);
        }
    }
}