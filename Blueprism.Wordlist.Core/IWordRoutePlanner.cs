namespace Blueprism.Wordlist.Core
{
    public interface IWordRoutePlanner
    {
        string[] PlanRouteBetweenWords(string[] wordList, string startWord, string endWord);
    }
}