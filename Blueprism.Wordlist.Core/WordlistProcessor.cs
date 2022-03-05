namespace Blueprism.Wordlist.Core
{
    public class WordlistProcessor : IWordlistProcessor
    {
        private readonly IDictionaryReader _dictionaryReader;
        private readonly IWordRoutePlanner _routePlanner;
        private readonly IWordRepository _repository;

        public WordlistProcessor(IDictionaryReader dictionaryReader, IWordRoutePlanner routePlanner, IWordRepository repository)
        {
            _dictionaryReader = dictionaryReader;
            _routePlanner = routePlanner;
            _repository = repository;
        }

        public string[] CalculatePathFromStartToEndWords(string startWord, string endWord)
        {
            //grab words from list
            //TODO : This should probably use the length of start or end to work out num of characters
            var words = _dictionaryReader.GetFourLetterWords();
            var wordRoute = _routePlanner.PlanRouteBetweenWords(words, startWord, endWord);
            _repository.SavePathFromStartToEndWords(wordRoute);

            return wordRoute;
        }
    }
}