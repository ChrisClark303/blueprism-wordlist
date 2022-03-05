namespace Blueprism.Wordlist.Core
{
    public static class CharArrayExtensions
    {
        public static int CharacterMatchCount(this IEnumerable<char> source, IEnumerable<char> target)
        {
            if (source.Count() != target.Count()) return 0;
            int matches = 0;
            for (int i = 0; i < source.Count(); i++)
            {
                if (source.ElementAt(i) == target.ElementAt(i)) matches++;
            }

            return matches;
        }
    }
}