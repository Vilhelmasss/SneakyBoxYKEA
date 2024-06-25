using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Newtonsoft.Json;

public class FileDataHandler
{
    private string dataDirPath = string.Empty;
    private string dataFileName = string.Empty;

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = new GameData();

        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = string.Empty;
                using StreamReader reader = new StreamReader(fullPath);
                dataToLoad = reader.ReadToEnd();
                loadedData.placedObjects = JsonConvert.DeserializeObject<List<PlacedObject>>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError($"Fail to load at {fullPath} \n{e}");
            }
        }

        return loadedData;
    }

    public void Save(GameData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToStore = JsonConvert.SerializeObject(data.placedObjects);

            using StreamWriter writer = new StreamWriter(fullPath);
            writer.Write(dataToStore);
        }
        catch (Exception e)
        {
            Debug.LogError($"Fail to save at {fullPath} \n{e}");
        }
    }
}
