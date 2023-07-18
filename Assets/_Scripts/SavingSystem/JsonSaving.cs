using System.IO;
using UnityEngine;

namespace ProjectUtils.SavingSystem
{
    public static class JsonSaving 
    {
        public static void SaveData(object data, string fileName = "gameData")
        {
            string persistentPath = Application.persistentDataPath + $"/{fileName}.json";
            string jsonData = JsonUtility.ToJson(data);

            using StreamWriter streamWriter = new StreamWriter(persistentPath);
            streamWriter.Write(jsonData);
            streamWriter.Close();
        }

        public static object LoadData(string fileName = "gameData")
        {
            string persistentPath = Application.persistentDataPath + $"/{fileName}.json";
            if (File.Exists(persistentPath))
            {
                using StreamReader streamReader = new StreamReader(persistentPath);
                string jsonData = streamReader.ReadToEnd();
                streamReader.Close();

                object gameData = JsonUtility.FromJson<object>(jsonData);
                return gameData;
            }
            
            Debug.Log("File can't be found");
            return null;
        }
    }
}
