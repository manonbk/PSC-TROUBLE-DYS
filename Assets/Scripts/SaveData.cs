using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class SaveData : MonoBehaviour
{
    public Button button;
    public InputField entrée;
    public string codeJ;
    string[] types = new string[] { "%DIST%", "%COORD%", "%EVENT%" };

    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick()
    {
        codeJ = entrée.text;
        StreamWriter file = new(Application.dataPath + "/data.txt", append: true);
        Debug.Log("Fichier créé");
        file.WriteLineAsync(codeJ);
        file.Close();
    }

    public void save(string donnee, int type)
    {

        string time = Time.time.ToString();
        StreamWriter file = new(Application.dataPath + "/data.txt", append: true);
        file.WriteLineAsync(time + types[type] + donnee);
        file.Close();
        Debug.Log("Sauvegarde effectuée");

    }

}