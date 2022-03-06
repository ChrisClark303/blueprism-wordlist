using Blueprism.Wordlist.Core;
using Moq;
using NUnit.Framework;
using System.Linq;

namespace Blueprism.WordList.Tests
{
    public class DictionaryReaderTests
    {
        [Test]
        public void DictionaryReader_GetFourLetterWords_CallsFileRepository_GetDictionaryContents()
        {
            var fileRepository = new Mock<IWordRepository>();
            var reader = new DictionaryReader(fileRepository.Object);

            reader.GetFourLetterWords();

            fileRepository.Verify(fr => fr.GetDictionaryWordList());
        }

        [Test]
        public void DictionaryReader_ReturnsOnly_FourLetterWordsFromDictionaryContents()
        {
            var fileRepository = new Mock<IWordRepository>();
            fileRepository.Setup(fr => fr.GetDictionaryWordList())
                .Returns(new []
                {
                    "alps", "alsatian", "alsop", "altair", "alton", "alva", "alvarez", "alvin", "amadeus", "amarillo", "amazon", "amelia", "amerada", "america", "american", "americana", "americanism", "ames", "ameslan", "amharic", "amherst", "amman", "ammerman", "amoco", "amos"
                });
            var reader = new DictionaryReader(fileRepository.Object);

            var fourLetterWords = reader.GetFourLetterWords();

            Assert.AreEqual(4, fourLetterWords.Length);
            Assert.AreEqual(4, fourLetterWords.Intersect(new[] { "alps", "alva", "ames", "amos" }).Count());
        }

        [Test]
        public void DictionaryReader_Returns_FourLetterWords_AsLower()
        {
            var fileRepository = new Mock<IWordRepository>();
            fileRepository.Setup(fr => fr.GetDictionaryWordList())
                .Returns(new[]
                {
                    "Alps", "Alsatian", "Alsop", "Altair", "Alton", "Alva", "Alvarez", "Alvin", "Amadeus", "Amarillo", "Amazon", "Amelia", "Amerada", "America", "American", "Americana", "Americanism", "Ames", "Ameslan", "Amharic", "Amherst", "Amman", "Ammerman", "Amoco", "Amos"
                });
            var reader = new DictionaryReader(fileRepository.Object);

            var fourLetterWords = reader.GetFourLetterWords();

            Assert.AreEqual(4, fourLetterWords.Length);
            Assert.AreEqual(4, fourLetterWords.Intersect(new[] { "alps", "alva", "ames", "amos" }).Count());
        }
    }
}