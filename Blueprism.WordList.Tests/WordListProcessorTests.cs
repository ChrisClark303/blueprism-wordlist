using Blueprism.Wordlist.Core;
using Moq;
using NUnit.Framework;

namespace Blueprism.WordList.Tests
{
    public class WordListProcessorTests
    {
        private static WordlistProcessor CreateWordProcessor(Mock<IDictionaryReader>? dictionaryReader = null, 
            Mock<IWordRoutePlanner>? routePlanner = null,
            Mock<IWordRepository>? repository = null)
        {
            return new WordlistProcessor((dictionaryReader ?? new Mock<IDictionaryReader>()).Object, 
                (routePlanner ?? new Mock<IWordRoutePlanner>()).Object, 
                (repository ?? new Mock<IWordRepository>()).Object);
        }

        [Test]
        public void CalculatePathFromStartToEndWords_Calls_DictionaryReader_GetFourLetterWords()
        {
            var reader = new Mock<IDictionaryReader>();
            var processor = CreateWordProcessor(reader);

            processor.CalculatePathFromStartToEndWords("", "");

            reader.Verify(r => r.GetFourLetterWords());
        }

        [Test]
        public void CalculatePathFromStartToEndWords_Calls_RoutePlanner_PlanRouteBetweenWords()
        {
            var reader = new Mock<IDictionaryReader>();
            var routePlanner = new Mock<IWordRoutePlanner>();
            var processor = CreateWordProcessor(reader, routePlanner);

            string[] words = new[] { "TEST" };
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