using System.Collections;
using System.Collections.Generic;
using Packages.Rider.Editor.UnitTesting;
using UnityEngine;
//use UI
using UnityEngine.UI;

public class score : MonoBehaviour
{
    public Text mytext;
    public static int scorepoints;
    public void Start(){
        scorepoints = 0;
    }
    // Start is called before the first frame update
    public void Scoreupdate(int score){
        scorepoints += score;
        mytext.text = scorepoints.ToString("0");
    }
}
