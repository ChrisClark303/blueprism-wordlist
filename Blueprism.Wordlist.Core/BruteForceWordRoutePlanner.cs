namespace Blueprism.Wordlist.Core
{
    public class BruteForceWordRoutePlanner : IWordRoutePlanner
    {
        public string[] PlanRouteBetweenWords(string[] wordList, string startWord, string endWord)
        {
            var matches = NextStep(wordList, startWord, endWord, new List<string>() { startWord });
            return matches.OrderBy(m => m.Length).First();
        }

        //TODO : Maybe this should just take the WordMatcher?
        private IEnumerable<string[]> NextStep(string[] wordList, string startWord, string endWord, List<string> currentPath)
        {
            var wordMatcher = new WordMatcher();
            var nextSteps = wordMatcher.GetMatchingWords(wordList, startWord, currentPath);
            if (nextSteps.Any(s => s.Equals(endWord)))
            {
                yield return new List<string>(currentPath) { endWord }
                    .ToArray();
            }
            else
            {
                //maybe do this as a Select returning a set of Tasks, or WordMatchers?
                foreach (var nextStep in nextSteps)
                {
                    var newPath = new List<string>(currentPath) { nextStep };
                    var path = NextStep(wordList, nextStep, endWord, newPath);
                    foreach (var p in path)
                    {
                        yield return p;
                    }
                }
            }
        }
    }

    public class WordMatcher
    {
        //TODO : Maybe construct this with the word to start with, to make it a state machine

        public string[] GetMatchingWords(string[] wordList, string word, List<string>? currentPath = null)
        {
            return wordList.Where(w => w != word && w.CharacterMatchCount(word) == 3)
                .Where(w => currentPath == null || !currentPath.Contains(w))
                .ToArray();
        }
    }
}