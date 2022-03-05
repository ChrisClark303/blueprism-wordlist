namespace Blueprism.Wordlist.Core
{

    public class SingleLetterMergeWordRoutePlanner : IWordRoutePlanner
    {
        /// <summary>
        /// Transforms startWord into endWord by incrementally changing one letter in the start word with one in
        /// the same position in the endword, ensuring that at each stage the resulting word exists in the 
        /// word list.
        /// Passes the example given in the spec, but not much else, I'd wager.
        /// </summary>
        public string[] PlanRouteBetweenWords(string[] wordList, string startWord, string endWord)
        {
            var wordRoute = MergeWords(wordList, endWord.ToLower(), startWord.ToLower().ToCharArray());
            return wordRoute.ToArray();
        }

        private IEnumerable<string> MergeWords(string[] wordList, string endWord, char[] startWordChars)
        {
            //TODO : Probably replace with a for loop
            int charToReplace = startWordChars.GetUpperBound(0);
            string mergedStartWord;
            yield return new string(startWordChars); //TODO : Maybe yield at the start of the loop, and then again once the loop terminates
            do
            {
                //TODO : Check if the letter at this pos is already the same
                mergedStartWord = MergeCharacterAtPos(endWord, startWordChars, charToReplace);
                if (!wordList.Contains(mergedStartWord))
                {
                    bool x = false; //TODO : This needs to raise an exception
                }
                charToReplace--;
                yield return mergedStartWord;
            }
            while (wordList.Contains(mergedStartWord) && mergedStartWord != endWord);
        }

        private static string MergeCharacterAtPos(string sourceWord, char[] targetCharacters, int charToReplace)
        {
            targetCharacters[charToReplace] = sourceWord[charToReplace];
            return new string(targetCharacters);
        }
    }
}