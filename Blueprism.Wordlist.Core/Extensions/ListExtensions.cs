namespace Blueprism.Wordlist.Core
{
    public static class ListExtensions
    {
        public static List<T> CopyAndAdd<T>(this List<T> list, T toAdd)
        {
            return new List<T>(list) { toAdd };
        }
    }
}