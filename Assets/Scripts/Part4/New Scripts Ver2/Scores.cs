using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms.Impl;

public class Scores : MonoBehaviour
{
    public static int failCount = 0;
    public static float alpha = 0.5f;
    public static float beta = 15;
    public static float gamma = 1;

    public static List<float> timeref = new List<float>();
    public static List<float> timeplayer = new List<float>();
    public static List<float> failtime = new List<float>();

    public static List<string> writetext = new List<string>();



    void Update(){
        //Debug.Log("FailCount TRYING");
         if ((Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) || (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject())){
            // Increment the click count
            failCount++;
            //Debug.Log("FailCount: " +failCount);
        }
    }

    public static bool passedtest(){
        return ScoreCalculator() > 0.75f;
    }
    public static double ScoreCalculator(){
        float valA = decalagetemps();
        float valB = decalagetempo();
        return Math.Tanh(1/ (valA * alpha + valB * beta + failCount * gamma));
    }
    
    public static void Reset(){
        timeref.Clear();
        timeplayer.Clear();
        failtime.Clear();
        failCount = 0;
    }
    //save data : tempo, speed, distance

    public static float decalagetemps(){
        float sum = 0;
        for (int i = 0; i < timeref.Count; i++){
            sum += Mathf.Pow(timeref[i] - timeplayer[i], 2);
        }
        return sum;
    }

    public static float decalagetempo(){
        float sum = 0;
        for (int i = 0; i < timeref.Count - 1; i++){
            sum += Mathf.Pow(timeplayer[i+1] - timeplayer[i] - (timeref[i+1] - timeref[i]), 2);
        }
        return sum;
    }
}