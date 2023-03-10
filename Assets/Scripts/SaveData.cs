using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class SaveData : MonoBehaviour
{
    public Button button;
    public InputField entree;
    private string codeJ;
    public Text showPlayerCode;

    string[] types = new string[] { "%DIST%", "%COORD%", "%EVENT%" };

    // Start is called before the first frame update
    void Start()
    {
        codeJ = PlayerPrefs.GetString("code");
        PlayerPrefs.SetString("code", codeJ);
        showPlayerCode.text = codeJ;
    }
    public void TaskOnClick()
    {
        codeJ = entree.text;
        PlayerPrefs.SetString("code", codeJ);
        showPlayerCode.text = codeJ;

        StreamWriter file = new(Application.persistentDataPath + "/data-"+codeJ+".txt", append: true);
        Debug.Log("Fichier créé");
        file.WriteLineAsync(codeJ);
        file.Close();
    }

    public void save(string donnee, int type)
    {
        Debug.Log("Sauvegarde effectuée");

    }

}