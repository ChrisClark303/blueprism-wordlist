using Blueprism.Wordlist.Core;
using Moq;
using NUnit.Framework;
using System;

namespace Blueprism.WordList.Tests
{
    public class WordListProcessorTests
    {
        private static WordlistProcessor CreateWordProcessor(Mock<IDictionaryReader>? dictionaryReader = null, 
            Mock<IWordRoutePlanner>? routePlanner = null,
            Mock<IWordRepository>? repository = null)
        {
            return new WordlistProcessor((dictionaryReader ?? CreateDictionaryReader()).Object, 
                (routePlanner ?? new Mock<IWordRoutePlanner>()).Object, 
                (repository ?? new Mock<IWordRepository>()).Object);
        }

        private static Mock<IDictionaryReader> CreateDictionaryReader()
        {
            var mockReader = new Mock<IDictionaryReader>();
            mockReader.Setup(x => x.GetFourLetterWords()).Returns(new[] { "start", "end" });
            return mockReader;
        }

        [Test]
        public void CalculatePathFromStartToEndWords_Calls_DictionaryReader_GetFourLetterWords()
        {
            var reader = CreateDictionaryReader();
            var processor = CreateWordProcessor(reader);

            processor.CalculatePathFromStartToEndWords("start", "end");

            reader.Verify(r => r.GetFourLetterWords());
        }

        [Test]
        public void CalculatePathFromStartToEndWords_StartWordNotInWordList_ThrowsArgumentOutOfRangeException()
        {
            var reader = new Mock<IDictionaryReader>();
            var routePlanner = new Mock<IWordRoutePlanner>();
            var processor = CreateWordProcessor(reader, routePlanner);

            reader.Setup(r => r.GetFourLetterWords())
                .Returns(new[] { "spin", "spit", "spat", "spot", "span" });

            Assert.Throws<ArgumentOutOfRangeException>(() => processor.CalculatePathFromStartToEndWords("spun", "span"));
        }

        [Test]
        public void CalculatePathFromStartToEndWords_EndWordNotInWordList_ThrowsArgumentOutOfRangeException()
        {
            var reader = new Mock<IDictionaryReader>();
            var routePlanner = new Mock<IWordRoutePlanner>();
            var processor = CreateWordProcessor(reader, routePlanner);

            reader.Setup(r => r.GetFourLetterWords())
                .Returns(new[] { "spin", "spit", "spat", "spot", "span" });

            Assert.Throws<ArgumentOutOfRangeException>( () => processor.CalculatePathFromStartToEndWords("spin", "spun"));
        }

        [Test]
        public void CalculatePathFromStartToEndWords_Calls_RoutePlanner_PlanRouteBetweenWords()
        {
            var reader = new Mock<IDictionaryReader>();
            var routePlanner = new Mock<IWordRoutePlanner>();
            var processor = CreateWordProcessor(reader, routePlanner);

            string[] words = new[] { "start", "end" };
            reader.Setup(r => r.GetFourLetterWords())
                .Returns(words);

            processor.CalculatePathFromStartToEndWords("start", "end");

            routePlanner.Verify(rp => rp.PlanRouteBetweenWords(words, "start", "end"));
        }

        [Test]
        public void CalculatePathFromStartToEndWords_Calls_WordRepository_SomethingOrOther()
        {
            var routePlanner = new Mock<IWordRoutePlanner>();
            var repository = new Mock<IWordRepository>();
            var processor = CreateWordProcessor(routePlanner: routePlanner, repository:repository );

            string[] words = new[] { "TEST" };
            routePlanner.Setup(r => r.PlanRouteBetweenWords(It.IsAny<string[]>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(words);

            processor.CalculatePathFromStartToEndWords("start", "end");

            repository.Verify(repo => repo.SavePathFromStartToEndWords(words));
        }
    }
}