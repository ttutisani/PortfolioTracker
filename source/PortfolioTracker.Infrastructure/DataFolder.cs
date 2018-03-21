using Newtonsoft.Json;
using System;
using System.IO;

namespace PortfolioTracker.Infrastructure
{
    internal static class DataFolder
    {
        private static readonly string _dataFolderPath;

        static DataFolder()
        {
            var myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            _dataFolderPath = Path.Combine(myDocuments, "Portfolios");

            if (!Directory.Exists(_dataFolderPath))
                Directory.CreateDirectory(_dataFolderPath);
        }

        private static string GetFilePath(string fileName)
        {
            return Path.Combine(_dataFolderPath, fileName);
        }

        public static TContent DeserializeFileContent<TContent>(string jsonFileName)
        {
            var filePath = GetFilePath(jsonFileName);
            if (!File.Exists(filePath))
                return default(TContent);

            var fileContent = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<TContent>(fileContent);
        }

        public static void SerializeContentInfoFile(string jsonFileName, object content)
        {
            var filePath = GetFilePath(jsonFileName);
            var fileContent = JsonConvert.SerializeObject(content, Formatting.Indented);
            File.WriteAllText(filePath, fileContent);
        }
    }
}
