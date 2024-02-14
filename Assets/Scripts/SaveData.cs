using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class SaveData : MonoBehaviour
{
    public InputField entree;
    private string codeJ;
    public Text showPlayerCode;
    private string data = "";
    private StreamWriter file;
    public string sceneName = "";

    string[] types = new string[] { "%DIST%", "%COORD%", "%EVENT%" };

    DateTime startTime;

    // Start is called before the first frame update
    void Start()
    {
        codeJ = PlayerPrefs.GetString("code");
        PlayerPrefs.SetString("code", codeJ);
        showPlayerCode.text = codeJ;

        startTime = DateTime.Now; 
    }
    public void TaskOnClick()
    {
        codeJ = entree.text;
        PlayerPrefs.SetString("code", codeJ);
        showPlayerCode.text = codeJ;
    }

    public void add(string donnee)
    {
        TimeSpan deltaTime = (DateTime.Now - startTime);
        data +=deltaTime.ToString() +";"+ donnee + "\n";
    }

    void OnDestroy()
    {
        if (sceneName != "")
        {
            save();
        }
    }

    void save()
    {
        file = new(Application.persistentDataPath + "/"+codeJ+ "-V2-ACTI"+sceneName+"-"+ DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") + ".txt", append: true);
        file.Write(data);
        file.Close();
        Debug.Log("Sauvegarde fichier effectuee : "+ Application.persistentDataPath+sceneName);
    }

}