using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SerializationManager
{
    public static bool Save(string saveName, object saveData)
    {
        BinaryFormatter binaryFormatter = Get_Binary_Formatter();

        if(!Directory.Exists(Application.persistentDataPath + "/save"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/save");
        }

        string path = Application.persistentDataPath + "/saves/" + saveName + ".save";

        FileStream file = File.Create(path);
        binaryFormatter.Serialize(file, saveData);
        file.Close();

        return true;
    }

    public static object Load(string path)
    {
        if (!File.Exists(path))
        {
            return null;
        }

        BinaryFormatter binaryFormatter = Get_Binary_Formatter();
        FileStream file = File.Open(path, FileMode.Open);

        try
        {
            object save = binaryFormatter.Deserialize(file);
            file.Close();
            return save;
        }
        catch
        {
            Debug.LogWarning("Error to Load Save File at " + path);
            file.Close();
            return null;
        }
    }

    public static BinaryFormatter Get_Binary_Formatter()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        return binaryFormatter;
    }
}
