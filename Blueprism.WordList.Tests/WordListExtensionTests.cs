using Blueprism.Wordlist.Core;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Blueprism.WordList.Tests
{
    public class WordListExtensionTests
    {
        [Test]
        public void GetWordsDifferingBySingleCharacter_FindsAllWords_DifferingByOneLetter()
        {
            var wordList = new[]
                {
                    "Spin", "Spit", "Spot", "Span", "Spat", "Spar", "Slap", "Slip", "Skin"
                };

            var matching = wordList.GetWordsDifferingBySingleCharacter("Spin");

            Assert.AreEqual(3, matching.Count());
            Assert.AreEqual(3, matching.Intersect(new[] { "Spit", "Span", "Skin" }).Count());
        }

        [Test]
        public void GetWordsDifferingBySingleCharacter_OnlyReturnsWords_NotInPathAlready()
        {
            var wordList = new[]
                {
                    "Spin", "Spit", "Spot", "Span", "Spat", "Spar", "Slap", "Slip", "Skin"
                };

            var matching = wordList.GetWordsDifferingBySingleCharacter("Spin", new List<string>(new[] { "Skin" }));

            Assert.AreEqual(2, matching.Count());
            Assert.AreEqual(2, matching.Intersect(new[] { "Spit", "Span"}).Count());
        }
    }
}