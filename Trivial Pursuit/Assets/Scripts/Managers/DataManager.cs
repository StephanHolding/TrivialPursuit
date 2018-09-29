using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class DataManager : MonoBehaviour {

    public static DataManager instance;

    public string directory;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public string[] LoadStringFile(string fileName)
    {
        FileStream fs = new FileStream(directory + "/" + fileName, FileMode.Open);

        string wholeFile = null;

        using (StreamReader reader = new StreamReader(fs, true))
        {
            wholeFile = reader.ReadToEnd();
        }

        string[] toReturn = wholeFile.Split('\n');

        return toReturn;
    }
}
