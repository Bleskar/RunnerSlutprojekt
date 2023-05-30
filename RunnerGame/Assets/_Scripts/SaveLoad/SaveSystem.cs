using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

//converts the savefile to a binary file and saves it to the computer
public static class SaveSystem
{
    static string Dir => "/MainSave.dnc"; //the standard relative directory for the savefile

    //Method for saving
    public static void Save(SaveFile data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + Dir; //path of the file
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data); //format the data into the file stream
        stream.Close(); //close stream
    }

    //Method for loading a save
    public static SaveFile Load()
    {
        string path = Application.persistentDataPath + Dir; //path of the file
        if (!File.Exists(path)) //return null if the path doens't exist
            return null;

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Open);

        SaveFile data = (SaveFile)formatter.Deserialize(stream); //deserialize the file and cast it into a SaveFile
        stream.Close(); //close stream

        return data; //return the file
    }
}
