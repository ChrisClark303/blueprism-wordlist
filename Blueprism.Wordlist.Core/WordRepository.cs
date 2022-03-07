namespace Blueprism.Wordlist.Core
{
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

        public void SaveWordRoute(string[] contents)
        {
            File.WriteAllLines(_resultsFilePath, contents);
        }
    }
}