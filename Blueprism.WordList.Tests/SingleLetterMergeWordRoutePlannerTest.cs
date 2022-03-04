using Blueprism.Wordlist.Core;
using NUnit.Framework;

namespace Blueprism.WordList.Tests
{
    public class SingleLetterMergeWordRoutePlannerTest
    {
        [TestCase("Spin", "Spot", new[] { "Spin", "Spit", "Spat", "Spot", "Span" }, new[] { "Spin", "Spit", "Spot" })]
        public void PlanRouteBetweenWords_CorrectlyGeneratesWordList(string startWord, string endWord, string[] source, string[] expected)
        {
            var routePlanner = new SingleLetterMergeWordRoutePlanner();
            var wordRoute = routePlanner.PlanRouteBetweenWords(source, startWord, endWord);
            Assert.AreEqual(expected, wordRoute);
        }
    }
}