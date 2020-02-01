namespace BoggleSolver
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class Program
    {
        static public void Main()
        {
            var testCases = Console.ReadLine();

            for (var tc = 0; tc < int.Parse(testCases); tc++)
            {
                var wordsCount = Console.ReadLine();
                var wordsString = Console.ReadLine();
                var sizeString = Console.ReadLine();
                var boggleLetters = Console.ReadLine();

                var words = wordsString.Split(' ', StringSplitOptions.RemoveEmptyEntries).Distinct().OrderBy(x => x).ToArray();
                var size = sizeString.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var height = int.Parse(size[0]);
                var width = int.Parse(size[1]);
                var chars = boggleLetters.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                var boggle = new Dictionary<(int, int), string>(width * height);

                var index = 0;
                for (var i = 0; i < height; i++)
                    for (var j = 0; j < width; j++)
                    {
                        boggle.Add((i, j), chars[index]);
                        index++;
                    }

                var result = SolveBoggle(boggle, words, width, height);

                if (result.Any())
                {
                    foreach (var w in result)
                        Console.Write(w + " ");
                }
                else
                    Console.Write("-1");

                Console.WriteLine();
            }
        }

        private static List<string> SolveBoggle(Dictionary<(int, int), string> boggle, string[] words, int width, int height)
        {
            var result = new List<string>();

            foreach (var word in words)
            {
                var found = false;

                for (var i = 0; i < height && !found; i++)
                    for (var j = 0; j < width && !found; j++)
                    {
                        var first = word.Substring(0, 1);

                        if (boggle[(i, j)] == first)
                        {
                            if (word.Length == 1)
                            {
                                result.Add(word);
                                found = true;
                            }
                            else
                            {
                                var remaining = new Dictionary<(int, int), string>(boggle);
                                remaining.Remove((i, j));

                                if (CanFormWord(word.Substring(1), remaining, (i, j)))
                                {
                                    result.Add(word);
                                    found = true;
                                }
                            }
                        }
                    }
            }
            return result;
        }

        private static bool CanFormWord(string word, Dictionary<(int, int), string> remaining, (int, int) lastVisited)
        {
            if (word == string.Empty)
                return true;

            var nextChar = word.Substring(0, 1);
            var adjs = GetAdjacents(lastVisited, remaining);

            foreach (var adj in adjs)
            {
                var c = remaining[adj];

                if (c == nextChar)
                {
                    if (word.Length == 1)
                        return true;

                    var newRemaining = new Dictionary<(int, int), string>(remaining);
                    newRemaining.Remove(adj);

                    if (CanFormWord(word.Substring(1), newRemaining, adj))
                        return true;
                }
            }

            return false;
        }

        private static IEnumerable<(int, int)> GetAdjacents((int, int) actual, Dictionary<(int, int), string> remaining)
        {
            for (var i = -1; i < 2; i++)
                for (var j = -1; j < 2; j++)
                {
                    var p = (actual.Item1 + i, actual.Item2 + j);
                    if (remaining.ContainsKey(p))
                        yield return p;
                }
        }
    }
}