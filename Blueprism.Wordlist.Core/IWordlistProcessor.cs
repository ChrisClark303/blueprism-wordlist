namespace Blueprism.Wordlist.Core
{
    public interface IWordlistProcessor
    {
        string[] CalculatePathFromStartToEndWords(string startWord, string endWord);
    }
}