using BoggleLib;
using BoggleLibResources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace BoggleLibTests
{
    [TestClass]
    public class BoardTests
    {
        private IEnumerable<string> _words;
        private Random _random;

        [TestInitialize()]
        public void Initialize()
        {
            _words = new List<string>(
                ResourceProxy.GetDictionary().Split(
                new string[] { "\n" },
                StringSplitOptions.RemoveEmptyEntries));

            _random = new Random((int)DateTime.Now.Ticks);
        }

        [TestMethod]
        public void Board_Constructor_Test()
        {
            // Make sure it doesn't throw exceptions
            new Board(new char[0, 0], _words);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException),
            "Cannot be null.")]
        public void Board_Constructor_Null_Letters_Test()
        {
            new Board(null, new List<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException),
            "Cannot be null.")]
        public void Board_Constructor_Null_Words_Test()
        {
            new Board(new char[0,0], null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException),
            "Length must equal width.")]
        public void Board_Constructor_Invalid_Letters_Size_Test()
        {
            new Board(new char[1, 2], new List<string>());
        }

        public void Board_Solve_Empty_Dictionary_Test()
        {
            char[,] letters =
            {
                {'A', 'B'},
                {'C', 'D'}
            };

            var b = new Board(letters, new List<string>());
            var results = b.Solve();

            Assert.IsTrue(results.Count == 0);
        }

        [TestMethod]
        public void Board_Solve_Empty_Board_Test()
        {
            var b = new Board(new char[0, 0], _words);
            var results = b.Solve();

            Assert.IsTrue(results.Count == 0);
        }

        [TestMethod]
        public void Board_Solve_One_By_One_Board_Test()
        {
            char[,] letters = { { 'A' } };
            var b = new Board(letters, _words);
            var results = b.Solve();

            Assert.IsTrue(results.Count == 0);
        }

        [TestMethod]
        public void Board_Solve_Non_Alphabetic_Board_Test()
        {
            char[,] letters =
            {
                {'1', '*'},
                {'@', '{'}
            };

            var b = new Board(letters, new List<string>());
            var results = b.Solve();

            Assert.IsTrue(results.Count == 0);
        }

        [TestMethod]
        public void Board_Solve_Longest_Word_Test()
        {
            char[,] letters =
            {
                {'C', 'M', 'O', 'N', 'S'},
                {'E', 'O', 'O', 'N', '2'},
                {'U', 'D', 'S', 'I', '!'},
                {'R', 'N', 'T', 'T', '%'},
                {'T', 'E', 'R', 'A', '^'}
            };

            var b = new Board(letters, _words);
            var results = b.Solve();

            Assert.IsTrue(results.Contains("COUNTERDEMONSTRATIONS"));
        }

        [TestMethod]
        public void Board_Solve_Case_Insensitivity_Test()
        {
            char[,] letters =
            {
                {'y', 'O'},
                {'r', 'B'}
            };

            var expected = new HashSet<string>()
            {
                "BOy", "OrB", "OrBy", "rOB", "yOB"
            };

            var b = new Board(letters, _words);
            var results = b.Solve();

            Assert.IsTrue(results.Count == expected.Count);

            foreach (var word in results)
                Assert.IsTrue(expected.Contains(word));
        }

        [TestMethod]
        public void Board_Solve_Large_Board_Speed_Test()
        {
            var w = new System.Diagnostics.Stopwatch();
            w.Start();

            var b = new Board(GenerateRandomLetters(50), _words);
            b.Solve();

            w.Stop();

            Assert.IsTrue(w.ElapsedMilliseconds < 2000);
        }
        private char[,] GenerateRandomLetters(int size)
        {
            char[,] result = new char[size, size];

            for (int x = 0; x < result.GetLength(0); x++)
            {
                for (int y = 0; y < result.GetLength(1); y++)
                {
                    result[x, y] = Convert.ToChar(
                                   Convert.ToInt32(
                                   Math.Floor(26 * _random.NextDouble() + 65)));
                }
            }

            return result;
        }

        [TestMethod]
        public void Board_Solve_Known_Board_Test()
        {
            char[,] letters =
            {
                {'Y', 'O', 'X'},
                {'R', 'B', 'A'},
                {'V', 'E', 'D'}
            };

            var expected = new HashSet<string>()
            {
                "ABED", "ABO", "ABY", "AERO", "AERY",
                "BAD", "BADE", "BEAD", "BED", "BOA",
                "BORE", "BORED", "BOX", "BOY", "BREAD",
                "BRED", "BROAD", "BYRE", "BYROAD", "DAB",
                "DEB", "DERBY", "DEV", "OBE", "ORB",
                "ORBED", "ORBY", "ORE", "OREAD", "READ",
                "REB", "RED", "REV","ROAD", "ROB",
                "ROBE", "ROBED", "VERB", "VERY", "YOB", "YORE"
            };

            var b = new Board(letters, _words);
            var results = b.Solve();
 
            Assert.IsTrue(results.Count == expected.Count);

            foreach (var word in results)
                Assert.IsTrue(expected.Contains(word));
        }
    }
}
