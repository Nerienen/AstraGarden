using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace ProjectUtils.SavingSystem
{
    public static class BinarySaving 
    {
        public static void SaveData(object data, string fileName = "gameData")
        {
            string persistentPath = Application.persistentDataPath + $"/{fileName}.save";
            FileStream fileStream = new FileStream(persistentPath, FileMode.Create);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(fileStream, data);
            fileStream.Close();
        }
        
        public static object LoadData(string fileName = "gameData")
        {
            string persistentPath = Application.persistentDataPath + $"/{fileName}.save";
            if (File.Exists(persistentPath))
            {
                FileStream fileStream = new FileStream(persistentPath, FileMode.Open);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                object data = binaryFormatter.Deserialize(fileStream);
                fileStream.Close();
                return data;
            }

            Debug.Log("File can't be found");
            return null;
        }
    }
}
