namespace Blueprism.Wordlist.Core
{
    public interface IWordRoutePlanner
    {
        string[] PlanRouteBetweenWords(string[] wordList, string startWord, string endWord);
    }

    public class WordRoutePlanner : IWordRoutePlanner
    {
        public string[] PlanRouteBetweenWords(string[] wordList, string startWord, string endWord)
        {
            //1. Brute force - replace a letter at a time in the source word 
            //with the corresponding letter of the target word
            //need to keep track of which one has changed!

            throw new NotImplementedException();
        }
    }

}