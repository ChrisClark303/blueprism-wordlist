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
            //TODO : This could be changed to use the length of start or end to work out num of characters
            var words = _dictionaryReader.GetFourLetterWords();
            if (!words.Contains(startWord.ToLower()))
            {
                throw new ArgumentOutOfRangeException(nameof(startWord), "StartWord must exist in the word list.");
            }
            if (!words.Contains(endWord.ToLower()))
            {
                throw new ArgumentOutOfRangeException(nameof(endWord), "EndWord must exist in the word list.");
            }
            var wordRoute = _routePlanner.PlanRouteBetweenWords(words, startWord, endWord);
            _repository.SaveWordRoute(wordRoute);

            return wordRoute;
        }
    }
}