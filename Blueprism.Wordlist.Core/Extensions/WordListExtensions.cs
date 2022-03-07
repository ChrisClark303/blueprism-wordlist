namespace Blueprism.Wordlist.Core
{
    public static class WordListExtensions
    {
        public static string[] GetWordsDifferingBySingleCharacter(this string[] wordList, string word, List<string>? currentPath = null)
        {
            return wordList.Where(w => w != word && w.CharacterMatchCount(word) == 3)
                .Where(w => currentPath == null || !currentPath.Contains(w))
                .ToArray();
        }
    }
}