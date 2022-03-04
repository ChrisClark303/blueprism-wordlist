namespace Blueprism.Wordlist.Core
{
    public interface IDictionaryReader
    {
        string[] GetFourLetterWords();
    }

    public class DictionaryReader : IDictionaryReader
    {
        private readonly IWordRepository repository;

        public DictionaryReader(IWordRepository repository)
        {
            this.repository = repository;
        }                

        public string[] GetFourLetterWords()
        {
            var dictContents = repository.GetDictionaryWordList();

            return dictContents.Where(w => w.Length == 4)
                .Select(w => w.ToLower())
                .ToArray();
        }
    }
}