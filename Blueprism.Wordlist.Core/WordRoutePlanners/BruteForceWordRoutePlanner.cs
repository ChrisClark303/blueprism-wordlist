namespace Blueprism.Wordlist.Core
{
    /// <summary>
    /// A slow, bruteforce approach that explores all routes from the start word by recursively finding words in the dictionary that differ
    /// by a single letter. 
    /// There is not a massive amount of intelligence here, if there is a route between start and end words then it will arrive at it 
    /// purely through a "thousand monkeys at a thousand typewriters" style approach.
    /// Unpromising avenues are discouraged in two ways: firstly, paths that share the most number of characters with the end word are prioritised.
    /// Secondly, once a path is found, any path currently being explored that is longer is terminated, since, even if it succeeds, it won't produce a 
    /// more efficient path.
    /// The benefit of this approach is that will find ANY path to the target, no matter how indirect.
    /// </summary>
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
            var matches = ProcessNextSetOfWords(wordList, startWord.ToLower(), endWord.ToLower(), new List<string>() { startWord.ToLower() });
            return matches.OrderBy(m => m.Length).First();
        }

        private IEnumerable<string[]> ProcessNextSetOfWords(string[] wordList, string startWord, string endWord, List<string> currentPath)
        {
            if (PathIsLongerThanShortestRoute(currentPath))
            {
                yield break;
            }
            var nextWords = wordList.GetWordsDifferingBySingleCharacter(startWord, currentPath);
            if (nextWords.Any(s => s.Equals(endWord)))
            {
                RouteFound(currentPath.Count + 1);
                yield return currentPath.CopyAndAdd(endWord)
                    .ToArray();
            }
            else
            {
                var orderedNextWords = OrderWordsByCommonalityWithTarget(nextWords, endWord);
                var paths = orderedNextWords.SelectMany(word =>
                {
                    var newPath = currentPath.CopyAndAdd(word);
                    return ProcessNextSetOfWords(wordList, word, endWord, newPath);
                });
                foreach (var path in paths)
                {
                    yield return path;
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
}