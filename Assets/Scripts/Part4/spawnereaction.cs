using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Video;
using System.Collections.Generic;
using System.Buffers;
using Mono.Cecil.Cil;

public class spawnereaction : MonoBehaviour
{
    // Start is called before the first frame update
    public float width = 2f; 
    public float height = 2f;
    //les coups delivrer
    public GameObject Generatecoup;
    public GameObject Generatecoupsuc;
    
    //next fonction be delayed in somme sort of value
    public float delay;

    //Declaring an array of integers
    List<string> sequences = new List<string>();
    
    //Variable global
    int niveau = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Add elements in list
        sequences.Add("3");
        sequences.Add("22");
        sequences.Add("12");
        sequences.Add("111");
        sequences.Add("4");
        sequences.Add("13");
        sequences.Add("211");
        sequences.Add("222");
        sequences.Add("23");
        sequences.Add("1111");
        sequences.Add("14");
        sequences.Add("5");
        sequences.Add("212");
        sequences.Add("42");
        sequences.Add("1112");
        sequences.Add("231");
        sequences.Add("142");
        sequences.Add("2112");
        sequences.Add("3121");
        sequences.Add("1232");
        sequences.Add("12212");

        //spawner()
        //spawnuntil();
        spawnseq(niveau);
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));
    }

    // Update is called once per frame
    void Update()
    {
        if(checkforempty()){
            //spawner();
            //spawnuntil();
            if(niveau < 19){
            niveau++;
            spawnseq(niveau);
            }
        }
    }

    /*void spawnuntil(){//une chance sur deux avoir cercle que square
        Transform position = freeposition();
        if (position != null){ //une position sans coup
            int val = Random.Range(0,10);
            GameObject coup;
            if(val<=4){ 
            coup = Instantiate(Generatecoup, position.transform.position, Quaternion.identity);
            }
            else{
                coup = Instantiate(Generatecoupsuc, position.transform.position, Quaternion.identity);
            }
            coup.transform.parent = position;
        }
        if (freeposition() != null){
            Invoke("spawnuntil", delay);// invoke the fonction "spawnuntil" with a delay of delay
        }
    }*/

    void spawnseq(int niveau){
        Transform position = freeposition();
        if (position != null){ //une position sans coup
            string seq = sequences[niveau];
            /*GameObject coup;
            if(val<=4){ 
            coup = Instantiate(Generatecoup, position.transform.position, Quaternion.identity);
            }
            else{
                coup = Instantiate(Generatecoupsuc, position.transform.position, Quaternion.identity);
            }
            coup.transform.parent = position;
        }
        if (freeposition() != null){
            Invoke("spawnuntil", delay);// invoke the fonction "spawnuntil" with a delay of delay*/
            Debug.Log(seq);
        }
    }

    //Generer des coups sur tous positions infiniement
    void spawner(){
        //child: position transform: Spawner
        foreach (Transform child in transform){
            GameObject coup = Instantiate(Generatecoup, child.position, Quaternion.identity);//no iteration on the gameobject
            coup.transform.parent = child;//parent-child link
        }
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

    /*// Start is called before the first frame update
    public float width = 2f; 
    public float height = 2f;
    //les coups delivrer
    public GameObject Generatecoup;
    public GameObject Generatecoupsuc;
    
    //next fonction be delayed in somme sort of value
    public float delay;

    // Start is called before the first frame update
    void Start()
    {
        //spawner();
        spawnuntil();
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));
    }

    // Update is called once per frame
    void Update()
    {
        if(checkforempty()){
            //spawner();
            spawnuntil();
        }
    }

    void spawnuntil(){//une chance sur deux avoir cercle que square
        Transform position = freeposition();
        if (position != null){ //une position sans coup
            int val = Random.Range(0,10);
            GameObject coup;
            if(val<=4){ 
            coup = Instantiate(Generatecoup, position.transform.position, Quaternion.identity);
            }
            else{
                coup = Instantiate(Generatecoupsuc, position.transform.position, Quaternion.identity);
            }
            coup.transform.parent = position;
        }
        if (freeposition() != null){
            Invoke("spawnuntil", delay);// invoke the fonction "spawnuntil" with a delay of delay
        }
    }

    //Generer des coups sur tous positions infiniement
    void spawner(){
        //child: position transform: Spawner
        foreach (Transform child in transform){
            GameObject coup = Instantiate(Generatecoup, child.position, Quaternion.identity);//no iteration on the gameobject
            coup.transform.parent = child;//parent-child link
        }
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
    }*/
}
