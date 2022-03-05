namespace Blueprism.Wordlist.Core
{
    public class BruteForceWordRoutePlanner : IWordRoutePlanner
    {
        private int? shortestRoute;
        private void RouteFound(int count)
        {
            if (shortestRoute == null) shortestRoute = count;
            else
            {
                shortestRoute = Math.Min(shortestRoute.GetValueOrDefault(), count);
            }
        }

        public string[] PlanRouteBetweenWords(string[] wordList, string startWord, string endWord)
        {
            var matches = NextStep(wordList, startWord.ToLower(), endWord.ToLower(), new List<string>() { startWord.ToLower() });
            return matches.OrderBy(m => m.Length).First();
        }

        //TODO : Maybe this should just take the WordMatcher?
        private IEnumerable<string[]> NextStep(string[] wordList, string startWord, string endWord, List<string> currentPath)
        {
            if (shortestRoute != null && currentPath.Count > shortestRoute) yield break; 
            var wordMatcher = new WordMatcher();
            var nextSteps = wordMatcher.GetMatchingWords(wordList, startWord, currentPath);
            if (nextSteps.Any(s => s.Equals(endWord)))
            {
                RouteFound(currentPath.Count + 1); //currently this is threadsafe
                //TODO : set shortest route field, terminate any paths that are longer
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