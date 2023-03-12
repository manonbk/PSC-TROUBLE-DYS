using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class SaveData : MonoBehaviour
{
    private string data = "";
    private StreamWriter file;

    public void add(string donnee)
    {
        data += Time.time.ToString() +","+ donnee + "\n";
        Debug.Log("Sauvegarde donnees dans data effectuee");
    }

    void OnDestroy()
    {
        file = new(Application.persistentDataPath + "/data.txt", append: true);
        file.Write(data);
        file.Close();
        Debug.Log("Sauvegarde fichier effectuee");
    }

}