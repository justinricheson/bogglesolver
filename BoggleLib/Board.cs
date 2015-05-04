using System;
using System.Collections.Generic;
using System.Linq;

namespace BoggleLib
{
    public class Board
    {
        private const int MIN_WORD_LENGTH = 3;
        private char[,] _letters;
        private HashSet<string> _words;
        private HashSet<string> _results;
        private HashSet<Tuple<int, int>> _xyPairs;
        private PrefixTrie _wordTree;

        public Board(char[,] letters, IEnumerable<string> words)
        {
            if (words == null)
                throw new ArgumentException("Cannot be null.", "words");

            if (letters == null)
                throw new ArgumentException("Cannot be null.", "letters");

            if (letters.GetLength(0) != letters.GetLength(1))
                throw new ArgumentException("Length must equal width.", "letters");

            _words = new HashSet<string>(words, StringComparer.OrdinalIgnoreCase);
            _results = new HashSet<string>();
            _xyPairs = new HashSet<Tuple<int, int>>();
            _letters = letters;

            _wordTree = new PrefixTrie();
            foreach (string word in _words)
                _wordTree.AddWord(word);
        }

        public List<string> Solve()
        {
            _results.Clear();

            for (int x = 0; x < _letters.GetLength(0); x++)
            {
                for (int y = 0; y < _letters.GetLength(1); y++)
                {
                    SearchBranch(x, y, null);
                }
            }

            var result = _results.ToList();
            result.Sort();
            return result;
        }

        private void SearchBranch(int x, int y, string prevLetters)
        {
            string currLetters = string.Format("{0}{1}",
                prevLetters ?? string.Empty, // prevLetters will be null when called non-recursively
                _letters[x, y]);

            if (!AnyWordsStartWith(currLetters))
                return;

            TryAddWord(currLetters); // Add the word if it's a word

            int xUp = x - 1;
            int xDown = x + 1;
            int yLeft = y - 1;
            int yRight = y + 1;
            bool canGoUp = xUp > -1;
            bool canGoDown = xDown < _letters.GetLength(0);
            bool canGoLeft = yLeft > -1;
            bool canGoRight = yRight < _letters.GetLength(1);

            SetTravelled(x, y, true); // Mark this space so it's not repeated

            if (canGoUp)
            {
                TrySearchBranch(xUp, y, currLetters);

                if (canGoLeft)
                    TrySearchBranch(xUp, yLeft, currLetters);

                if (canGoRight)
                    TrySearchBranch(xUp, yRight, currLetters);
            }

            if (canGoDown)
            {
                TrySearchBranch(xDown, y, currLetters);

                if (canGoLeft)
                    TrySearchBranch(xDown, yLeft, currLetters);

                if (canGoRight)
                    TrySearchBranch(xDown, yRight, currLetters);
            }

            if (canGoLeft)
                TrySearchBranch(x, yLeft, currLetters);

            if (canGoRight)
                TrySearchBranch(x, yRight, currLetters);

            SetTravelled(x, y, false);
        }

        private bool AnyWordsStartWith(string currLetters)
        {
            return _wordTree.WordStartsWith(currLetters);
        }

        private void TrySearchBranch(int x, int y, string prevLetters)
        {
            if (!HaveTravelled(x, y))
                SearchBranch(x, y, prevLetters);
        }

        private void SetTravelled(int x, int y, bool travelled)
        {
            var t = new Tuple<int, int>(x, y);

            if (travelled)
                _xyPairs.Add(t);
            else
                _xyPairs.Remove(t);
        }

        private bool HaveTravelled(int x, int y)
        {
            return _xyPairs.Contains(new Tuple<int, int>(x, y));
        }

        private void TryAddWord(string word)
        {
            if (word.Length >= MIN_WORD_LENGTH && _words.Contains(word))
                _results.Add(word);
        }
    }
}
