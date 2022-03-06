namespace Blueprism.Wordlist.Core
{
    public class BruteForceWordRoutePlanner : IWordRoutePlanner
    {
        private int? _shortestRoute;

        //Currently this is threadsafe, since all operations in this class occur on a single thread.        
        //If extra threads were introduced to allow some processing to occur in parallel, some kind of
        //synchronization mechanism would be required here.
        private void RouteFound(int routeLength)
        {
            if (_shortestRoute == null) _shortestRoute = routeLength;
            else
            {
                _shortestRoute = Math.Min(_shortestRoute.GetValueOrDefault(), routeLength);
            }
        }

        public string[] PlanRouteBetweenWords(string[] wordList, string startWord, string endWord)
        {
            var matches = NextStep(wordList, startWord.ToLower(), endWord.ToLower(), new List<string>() { startWord.ToLower() });
            return matches.OrderBy(m => m.Length).First();
        }

        private IEnumerable<string[]> NextStep(string[] wordList, string startWord, string endWord, List<string> currentPath)
        {
            if (PathIsLongerThanShortestRoute(currentPath))
            {
                yield break;
            }
            var nextSteps = WordMatcher.GetMatchingWords(wordList, startWord, currentPath);
            if (nextSteps.Any(s => s.Equals(endWord)))
            {
                RouteFound(currentPath.Count + 1);
                yield return currentPath.CopyAndAdd(endWord)
                    .ToArray();
            }
            else
            {
                //TODO : maybe do this as a Select returning a set of Tasks, or WordMatchers?
                //probably a Select just returns P
                var orderedNextSteps = OrderWordsByCommonalityWithTarget(nextSteps, endWord);
                foreach (var nextStep in orderedNextSteps)
                {
                    var newPath = currentPath.CopyAndAdd(nextStep);
                    var path = NextStep(wordList, nextStep, endWord, newPath);
                    foreach (var p in path)
                    {
                        yield return p;
                    }
                }
            }
        }

        private bool PathIsLongerThanShortestRoute(List<string> path)
        {
            return _shortestRoute != null && path.Count > _shortestRoute;
        }

        private string[] OrderWordsByCommonalityWithTarget(string[] words, string target)
        {
            return words.OrderByDescending(m => m.CharacterMatchCount(target)).ToArray();
        }
    }

    public static class WordMatcher
    {
        public static string[] GetMatchingWords(string[] wordList, string word, List<string>? currentPath = null)
        {
            return wordList.Where(w => w != word && w.CharacterMatchCount(word) == 3)
                .Where(w => currentPath == null || !currentPath.Contains(w))
                .ToArray();
        }
    }

    public static class ListExtensions
    {
        public static List<T> CopyAndAdd<T>(this List<T> list, T toAdd)
        {
            return new List<T>(list) { toAdd };
        }
    }
}