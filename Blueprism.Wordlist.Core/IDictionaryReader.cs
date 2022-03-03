namespace Blueprism.Wordlist.Core
{
    public interface IDictionaryReader
    {
        string[] GetFourLetterWords();
    }

    public class DictionaryReader : IDictionaryReader
    {
        private readonly IFileRepository repository;

        public DictionaryReader(IFileRepository repository)
        {
            this.repository = repository;
        }                

        public string[] GetFourLetterWords()
        {
            var dictContents = repository.GetDictionaryContents();

            return dictContents.Where(w => w.Length == 4)
                .ToArray();
        }
    }
}