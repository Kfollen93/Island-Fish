using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = string.Empty, dataFileName = string.Empty;

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        
        if (File.Exists(fullPath))
        {
            try
            {
                using FileStream fs = new FileStream(fullPath, FileMode.Open);
                using StreamReader sr = new StreamReader(fs);
                string dataToLoad = sr.ReadToEnd();

                // Deserialize JSON data back into C# object.
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error occurred when trying to load data from file: " + fullPath + "\n" + ex);
            }
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            // Create the directory the file will be written to if it doesn't already exist.
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // Serialize the C# game data object into JSON.
            string dataToStore = JsonUtility.ToJson(data, true);

            // Write the serialized data to the file.
            using FileStream fs = new FileStream(fullPath, FileMode.Create);
            using StreamWriter sw = new StreamWriter(fs);
            sw.Write(dataToStore);
        }
        catch (Exception ex)
        {
            Debug.LogError("Error occurred when trying to save data to file: " + fullPath + "\n" + ex);
        }
    }
}
