using Blueprism.Wordlist.Core;
using NUnit.Framework;

namespace Blueprism.WordList.Tests
{
    public class BruteForceWordRoutePlannerTests
    {
        [TestCase("spin", "spot", new[] { "spin", "spit", "spat", "spot", "span" }, new[] { "spin", "spit", "spot" })]
        [TestCase("spin", "slur", new[] { "spin", "spit", "spat", "spot", "span", "spun", "spur", "slur" }, new[] { "spin", "spun", "spur", "slur" })]
        public void PlanRouteBetweenWords_CorrectlyGeneratesWordList(string startWord, string endWord, string[] source, string[] expected)
        {
            var routePlanner = new BruteForceWordRoutePlanner();
            var wordRoute = routePlanner.PlanRouteBetweenWords(source, startWord, endWord);
            Assert.AreEqual(expected, wordRoute);
        }
    }
}