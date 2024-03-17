using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.EventSystems;


//avoir un action sur l'objet
public class coupaction : MonoBehaviour
{
    
    //each object has a SpriteRenderer attached to it that change its color
    public SpriteRenderer color;
    //score gagner a chaque fois on touche un coup
    public int scorevalue = 1;
    public AudioClip touchsound;
    public AudioClip refsound;

    public AudioClip failsound;

    // si l'objt a passe la position de ref
    bool hasPassedref = false;

    bool isClicked = false;

    private int value;

    // Start is called before the first frame update
    void Start()
    {
        color.enabled = false;
        value = getnumber();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasPassedref){
            generateref();
        }
    }

    void OnMouseDown(){
        if (hasPassedref){
            if (Input.GetMouseButtonDown(0) && !isClicked){
                if (DrawRefBande.simpos - DrawRefBande.simscale/2 - 0.75f  <= transform.position.x && transform.position.x <= DrawRefBande.simpos + DrawRefBande.simscale/2 + 0.75f){
                        AudioSource.PlayClipAtPoint(touchsound, transform.position);// sound played on the same position
                        ScoreManager.timeplayer[value - 1] = Time.time - spawnereaction.startingTime;
                        color.color = Color.yellow;
                        isClicked = true;

                        //Debug.Log("value:" + value);
                        ScoreManager.writetext.Add(" SUCCESS " + value + " on position x :" + Camera.main.ScreenToWorldPoint(Input.mousePosition).x + " and position y :"+ Camera.main.ScreenToWorldPoint(Input.mousePosition).y + " on time :" + ScoreManager.timeplayer[value - 1]);

                        ScoreManager.failCount--; //ne pas compter comme un fail
                        //Debug.Log(value + "eme coup player realtime: " + ScoreManager.timeplayer[value - 1]);
                        //Debug.Log("in bande: " + EventSystem.current.IsPointerOverGameObject());
                    }
                else{
                    AudioSource.PlayClipAtPoint(failsound, transform.position);
                    float failtime =  Time.time - spawnereaction.startingTime;
                    ScoreManager.failtime.Add(failtime);

                    ScoreManager.writetext.Add(" ERROR on position x :" + Camera.main.ScreenToWorldPoint(Input.mousePosition).x + " and position y :"+ Camera.main.ScreenToWorldPoint(Input.mousePosition).y + " on time :" + failtime);

                    //Debug.Log("not in bande: " + EventSystem.current.IsPointerOverGameObject());
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col){
        if(color.color == Color.yellow){
            Debug.Log("You are fine");
        }
        else if (col.collider.tag == "border"){
            //Debug.Log("game has end");

            //aller a la scene ending
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    void generateref(){
        if(transform.position.x >= DrawRefBande.banderef){
            color.enabled = true;
            hasPassedref = true;
            AudioSource.PlayClipAtPoint(refsound, transform.position);
            
            //Debug.Log("ref sound played: " + transform.position.x + " " + Time.time);

            //sound played on the same position
            float addref = Time.time - spawnereaction.startingTime + (DrawRefBande.dbesoin + DrawRefBande.simscale) / MvtRectiligne.speed;
            ScoreManager.timeref.Add(addref);

            ScoreManager.writetext.Add(" REF temps ref pour coup" + value + " : " + addref);

            ScoreManager.timeplayer.Add(ScoreManager.timeref[ScoreManager.timeref.Count -1] + 2* (DrawRefBande.simscale/2 + 0.75f) * value / MvtRectiligne.speed);

            //Debug.Log("timerefadd: " + ScoreManager.timeref[ScoreManager.timeref.Count - 1]);
            //Debug.Log("timeplayeradd: " + ScoreManager.timeplayer[ScoreManager.timeplayer.Count - 1]);

        }
    }

    int getnumber(){
        GameObject obj = gameObject;
        // Get the name of the GameObject
        string objectName = obj.name;

        //Debug.Log("the n th 1: " + int.Parse(objectName.Split(' ')[1]));
        return int.Parse(objectName.Split(' ')[1]);
        
    }
}
