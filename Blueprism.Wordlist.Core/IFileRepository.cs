namespace Blueprism.Wordlist.Core
{
    //Todo : should be named to avoid references to File
    public interface IFileRepository
    {
        string[] GetDictionaryContents();
        void SaveFile(string[] contents);
    }

    public class FileRepository : IFileRepository
    {
        private readonly string _dictionaryFilePath;
        private readonly string _resultsFilePath;

        public FileRepository(string dictionaryFilePath, string resultsFilePath)
        {
            _dictionaryFilePath = dictionaryFilePath;
            _resultsFilePath = resultsFilePath;
        }

        public string[] GetDictionaryContents()
        {
            return File.ReadAllLines(_dictionaryFilePath);
        }

        public void SaveFile(string[] contents)
        {
            File.WriteAllLines(_resultsFilePath, contents);
        }
    }
}