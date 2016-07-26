using Newtonsoft.Json;
using System;
using System.IO;

namespace SelfHostedWebApi.HostConfig.Settings
{
    public class FileToJsonHelper
    {
        public static T ReadJsonFile<T>(string fileName)
        {
            return ReadJsonFile<T>(AppDomain.CurrentDomain.BaseDirectory, fileName);
        }

        public static T ReadJsonFile<T>(string path, string fileName)
        {
            if (string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(nameof(path), "A path is needed");
            }

            try
            {
                string jsonString = string.Empty;

                using (StreamReader reader = new StreamReader(path + "\\" + fileName))
                {
                    jsonString = reader.ReadToEnd();
                }

                T result = JsonConvert.DeserializeObject<T>(jsonString);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("error when reading the file", ex);
            }
        }

        public static void WriteJsonToFile<T>(T objectToWrite, string fileName)
        {
            WriteJsonToFile<T>(objectToWrite, AppDomain.CurrentDomain.BaseDirectory, fileName);
        }

        public static void WriteJsonToFile<T>(T objectToWrite, string path, string fileName)
        {
            if (string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(nameof(path), "A path is needed");
            }

            string jsonString = JsonConvert.SerializeObject(objectToWrite);

            using (StreamWriter outputFile = new StreamWriter(path + "\\" + fileName))
            {
                outputFile.WriteLine(jsonString);
            }
        }
    }
}