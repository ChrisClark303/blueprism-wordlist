namespace Blueprism.Wordlist.Core
{
    public interface IWordRoutePlanner
    {
        //1. Brute force - replace a letter at a time in the source word 
        //with the corresponding letter of the target word
        //need to keep track of which one has changed!

        //2. Move along the dictionary alphabetically and look at each word to check the letters for changes.
        //if startword is not alphabetically prior to endword, reverse them

        //3. Build a word graph of paths of words from the dictionary that at each stage differ by a single character.
        //this could be done for both target and source words
        //Each "step" in the path could be ranked on how close it is to the target; the whole path could then be ranked on 
        string[] PlanRouteBetweenWords(string[] wordList, string startWord, string endWord);
    }
}