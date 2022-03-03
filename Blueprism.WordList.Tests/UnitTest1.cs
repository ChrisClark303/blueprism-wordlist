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
            var fileRepository = new Mock<IFileRepository>();
            var reader = new DictionaryReader(fileRepository.Object);

            reader.GetFourLetterWords();

            fileRepository.Verify(fr => fr.GetDictionaryContents());
        }

        [Test]
        public void DictionaryReader_ReturnsOnly_FourLetterWordsFromDictionaryContents()
        {
            var fileRepository = new Mock<IFileRepository>();
            fileRepository.Setup(fr => fr.GetDictionaryContents())
                .Returns(new []
                {
                    "Alps", "Alsatian", "Alsop", "Altair", "Alton", "Alva", "Alvarez", "Alvin", "Amadeus", "Amarillo", "Amazon", "Amelia", "Amerada", "America", "American", "Americana", "Americanism", "Ames", "Ameslan", "Amharic", "Amherst", "Amman", "Ammerman", "Amoco", "Amos"
                });
            var reader = new DictionaryReader(fileRepository.Object);

            var fourLetterWords = reader.GetFourLetterWords();

            Assert.AreEqual(4, fourLetterWords.Length);
            Assert.AreEqual(4, fourLetterWords.Intersect(new[] { "Alps", "Alva", "Ames", "Amos" }).Count());
        }

    }
}