namespace Blueprism.Wordlist.Core
{
    public interface IWordRepository
    {
        string[] GetDictionaryWordList();
        void SaveWordRoute(string[] wordRoute);
    }
}