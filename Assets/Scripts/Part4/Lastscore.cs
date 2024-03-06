using System.Collections;
using System.Collections.Generic;
using Packages.Rider.Editor.UnitTesting;
using UnityEngine.UI;
using UnityEngine;

public class Lastscore : MonoBehaviour
{
    public Text mytext;

    //static: has the value whenever we change the scene
    //int element, score et scorepoints sont devenue des vakeurs independants pour chaque fonction(score Lastescore) 
    //meme si la valeur de score dans la fonction score modifie, elle n'affactera pas la valeur de 
    //scorepoints dans Lastescoore
    public static int scorepoints = 0;

    // Start is called before the first frame update
    void Start(){
        //Lastscore score point (Final score) is the scorepoints of "score" function
        //scorepoints=FindObjectOfType<score>().scorepoints;
        scorepoints=score.scorepoints;
        mytext.text = "Your score is " + scorepoints.ToString("0");
    }

    public void savescore(int score){

    }
}
