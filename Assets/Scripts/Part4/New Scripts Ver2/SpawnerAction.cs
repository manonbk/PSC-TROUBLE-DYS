using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Video;
using System.Buffers;
using Mono.Cecil.Cil;
using UnityEngine.UIElements.Experimental;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class SpawnerAction : MonoBehaviour
{
    int testnb = 0; // nb e fois le test present soit teste
    public float width; 
    public float height;

    //les coups delivrer
    public GameObject Generatecoup;
    public float tempo;

    public static float startingTime;

    //Declaring an array of integers
    public static List<string> sequences = new List<string>();
    
    //Variable global
    public static int niveau = 0;
    
    public static SaveData sd;


    // Start is called before the first frame update
    void Start()
    {
        //Add elements in list
        sequences.Add("11");
        sequences.Add("101");
        //sequences.Add("111");
        sequences.Add("11011");
        //sequences.Add("1011");
        //sequences.Add("10101");
        sequences.Add("1111");
        //sequences.Add("10111");
        //sequences.Add("110101");
        sequences.Add("11011011");
        //sequences.Add("110111");
        //sequences.Add("1010101");
        sequences.Add("101111");
        sequences.Add("11111");
        sequences.Add("1101011");
        //sequences.Add("1111011");
        //sequences.Add("10101011");
        sequences.Add("11011101");
        //sequences.Add("101111011");
        sequences.Add("110101011");
        //sequences.Add("1110101101");
        //sequences.Add("10110111011");
        sequences.Add("101101101011");

        spawnseq(niveau);
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));
    }

    // Update is called once per frame
    void Update()
    {
        if(checkforempty()){//si il n'y a plus de coup
            //Debug.Log("alpha: " + Scores.alpha);
            //Debug.Log("beta: " + Scores.beta);
            //Debug.Log("gamma: " + Scores.gamma);
            //Debug.Log("ValA: " + Scores.decalagetemps());
            //Debug.Log("ValB: " + Scores.decalagetempo());
            //Debug.Log("FailCount: " + Scores.failCount);
            //Debug.Log("Score: " + Scores.ScoreCalculator());
            sd.add("ValA: " + Scores.decalagetemps());
            sd.add("ValB: " + Scores.decalagetempo());
            sd.add("FailCount: " + Scores.failCount);
            sd.add("Score: " + Scores.ScoreCalculator());
            sd.add("Fin d'essai");

            if (niveau <=5){
                if (Scores.passedtest()){
                    Scores.Reset();
                    DrawGates.Reset();
                    niveau++;
                    spawnseq(niveau);
                }
                else{
                    Scores.Reset();
                    DrawGates.Reset();
                    spawnseq(niveau);
                }
            }
            else if (niveau >= sequences.Count){
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else{
                if (Scores.passedtest()){
                    testnb = 0;
                    Scores.Reset();
                    DrawGates.Reset();
                    niveau++;
                    spawnseq(niveau);
                    }
                else{
                    if (testnb < 2){
                        testnb++;
                        Scores.Reset();
                        DrawGates.Reset();
                        spawnseq(niveau);
                    }
                    else{
                        Debug.Log("game has end");
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                    }
                }
            }
        }
    }

    void spawnseq(int niveau){
        //Debug.Log("test du niveau: " + niveau);
        sd.add("Niveau" + niveau + ", essai" + testnb + ", sequence" + sequences[niveau]);
        Transform position = freeposition();
        if (position != null){ //une position sans coup
            ResetTimer();
            string seq = sequences[niveau];
            int nbones = 1;
            Vector3 pos = position.transform.position;

            //Debug.Log(MvtRectiligne.speed * tempo);
            for (int k = 0; k < seq.Length; k++){
                //Debug.Log(seq[k]);
                string val = seq[k].ToString();
                //Debug.Log(val.Equals("1"));
                if (val.Equals("1")){
                    //Debug.Log("for a 1:" + pos.x);
                    GameObject newObject = Instantiate(Generatecoup, pos, Quaternion.identity);
                    newObject.transform.parent = position;
                    newObject.transform.localPosition = pos - position.transform.position;
                    newObject.name = "coup " + nbones;
                    nbones++;
                }
                pos.x = pos.x - MvtRectiligne.speed * tempo;

                //Debug.Log("speed:" + MvtRectiligne.speed);
                //Debug.Log("for a 0:" + pos.x);
            }
            //Debug.Log("nbones: " + nbones);

            //Scores.timeplayer = new List<float>(nbones);
            //Debug.Log("length: " + Scores.timeplayer.Count);

        }

        //Debug.Log(niveau + " success");
        /*if (freeposition() != null){
            Invoke("spawnseq", delay);// invoke the fonction "spawnuntil" with a delay of delay
        }*/
    }

    //Condition generation de coup
    bool checkforempty(){
        foreach (Transform child in transform){
            if(child.childCount > 0){
                return false;
            }
        }
        return true;
    }

    //Si il y a une position libre sans coup
    Transform freeposition(){
        foreach(Transform child in transform){
            if (child.childCount == 0){
                return child; //retourne la "position" ou il n'y a plus d'enfant
            }
        }
        return null;
    }

        void ResetTimer(){
        startingTime = Time.time;
        //Debug.Log(startingTime);
        
    }
}
