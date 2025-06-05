namespace Agent
{
    public class DefaultAgent
    {
        private string _filePath { get; set; }
        private string[] _fileToString { get; set; }    
        private Dictionary<string, int> _fileContent { get; set; }
        public DefaultAgent(string filePath)
        {
            this._filePath = filePath;
            this._fileContent = new Dictionary<string, int>();
        }

        public void ChangeFilePath(string newPath)
        {
            this._filePath = newPath;
        }
        public async Task ParseFileAsync()
        {
            string buffer = string.Empty;
            using (StreamReader fileStream = new StreamReader(_filePath))
            {
                buffer = await fileStream.ReadToEndAsync();
                _fileToString = buffer.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                fileStream.Close();
            }
        }
        public void CalculateWordFrequency(string searchedWord) //MUST be Tasked
        {
            if (string.IsNullOrEmpty(searchedWord))
            {
                throw new ArgumentException("Searched word cannot be null or empty.");
            }
            else
            {
                int count = _fileToString.Count(x => x == searchedWord);
                if(_fileContent.ContainsKey(searchedWord))
                {
                    _fileContent[searchedWord] += count;
                }
                else
                {
                    _fileContent.Add(searchedWord, count);
                }
            }
                
        }
        public (string, int) GetContentByToken(string token)
        {
            string returnTok = string.Empty;
            int amount = 0;
            if (_fileContent.ContainsKey(token))
            {
                returnTok = token;
                amount = _fileContent[token];
            }
            else
            {
                amount = 0;
            }
            return (returnTok, amount);
        }
    }
}
