namespace Blueprism.Wordlist.Core
{
    //Todo : Exception handling!
    public interface IWordRepository
    {
        string[] GetDictionaryWordList();
        void SavePathFromStartToEndWords(string[] contents);
    }

    public class WordRepository : IWordRepository
    {
        private readonly string _dictionaryFilePath;
        private readonly string _resultsFilePath;

        public WordRepository(string dictionaryFilePath, string resultsFilePath)
        {
            _dictionaryFilePath = dictionaryFilePath;
            _resultsFilePath = resultsFilePath;
        }

        public string[] GetDictionaryWordList()
        {
            return File.ReadAllLines(_dictionaryFilePath);
        }

        public void SavePathFromStartToEndWords(string[] contents)
        {
            File.WriteAllLines(_resultsFilePath, contents);
        }
    }
}