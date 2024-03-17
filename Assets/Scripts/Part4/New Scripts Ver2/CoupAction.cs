using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.EventSystems;


//avoir un action sur l'objet
public class CoupAction : MonoBehaviour
{
    
    //each object has a SpriteRenderer attached to it that change its color
    public SpriteRenderer color;
    //score gagner a chaque fois on touche un coup
    public int scorevalue = 1;
    public AudioClip touchsound;
    public AudioClip refsound;

    public AudioClip failsound;
    
    float refpos;
    float refscale;
    float simpos;
    float simscale;
    
    // si l'objt a passe la position de ref
    bool hasPassedref = false;

    bool isClicked = false;

    private int value;

    // Start is called before the first frame update
    void Start()
    {
        value = getnumber();
        refpos = DrawGates.refpos;
        refscale = DrawGates.refscale;
        simpos = DrawGates.simpos;
        simscale = DrawGates.simscale;
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
                //Debug.Log("bandeinf: " + (simpos - simscale/2 - 0.75f));
                //Debug.Log("bandesup: " + (simpos + simscale/2 + 0.75f));
                if (simpos - simscale/2 - 0.75f  <= transform.position.x && transform.position.x <= simpos + simscale/2 + 0.75f){
                        AudioSource.PlayClipAtPoint(touchsound, transform.position);// sound played on the same position
                        Scores.timeplayer[value - 1] = Time.time - SpawnerAction.startingTime;
                        color.color = Color.yellow;
                        isClicked = true;
                        
                        Scores.writetext.Add(" SUCCESS " + value + " on position x :" + Camera.main.ScreenToWorldPoint(Input.mousePosition).x + " and position y :"+ Camera.main.ScreenToWorldPoint(Input.mousePosition).y + " on time :" + Scores.timeplayer[value - 1]);

                        Scores.failCount--; //ne pas compter comme un fail
                        //Debug.Log(value + "eme coup player realtime: " + Scores.timeplayer[value - 1]);
                        //Debug.Log("in bande: " + EventSystem.current.IsPointerOverGameObject());

                    }
                else{
                    AudioSource.PlayClipAtPoint(failsound, transform.position);
                    float failtime =  Time.time - SpawnerAction.startingTime;
                    Scores.failtime.Add(failtime);
                    
                    Scores.writetext.Add(" ERROR on position x :" + Camera.main.ScreenToWorldPoint(Input.mousePosition).x + " and position y :"+ Camera.main.ScreenToWorldPoint(Input.mousePosition).y + " on time :" + failtime);
                    
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
        if(transform.position.x >= refpos ){
            //Debug.Log("detectionX" + refpos);
            hasPassedref = true;
            AudioSource.PlayClipAtPoint(refsound, transform.position);
            
            //Debug.Log("ref sound played: " + transform.position.x + " " + Time.time);
            // sound played on the same position

            float addref = Time.time - SpawnerAction.startingTime + (DrawGates.dbesoin + refscale/2 + simscale/2) / MvtRectiligne.speed;
            Scores.timeref.Add(addref);
            
            Scores.writetext.Add(" REF Temp ref pour coup" + value + " : " + addref);

            Scores.timeplayer.Add(Scores.timeref[Scores.timeref.Count -1] + 2* (simscale/2 + 0.75f) * value / MvtRectiligne.speed);  //ajout d'un tempo de valeur aleatoire de l'ordre de 2* (simscale/2 + 0.75f) [pour que lorsqu'on fait rien il y a une grand ecart de 2* (simscale/2 + 0.75f) entre deux coups succcessives loupes]

            //Debug.Log("timerefadd: " + Scores.timeref[Scores.timeref.Count - 1]);
            //Debug.Log("timeplayeradd: " + Scores.timeplayer[Scores.timeplayer.Count - 1]);

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
