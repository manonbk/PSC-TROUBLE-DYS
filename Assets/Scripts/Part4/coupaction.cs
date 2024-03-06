using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;


//avoir un action sur l'objet
public class coupaction : MonoBehaviour
{
    
    //each object has a SpriteRenderer attached to it that change its color
    public SpriteRenderer color;
    //score gagner a chaque fois on touche un coup
    public int scorevalue=1;
    public AudioClip touchsound;
    public AudioClip refsound;
    
    // position a detecter pour lancer son de reference
    public float detectionX = -6.5f;

    // si l'objt a passe la position de ref
    public bool hasPassedref = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasPassedref){
            generateref();
        }

        testposition();
    }

    void testposition(){
        if (Input.GetMouseButtonDown(0)){
            Vector3 mousepos = Input.mousePosition;
            Vector3 worldpos = Camera.main.ScreenToWorldPoint(mousepos);
            float worldposx = worldpos.x;
            float worldposy = worldpos.y;
            
            if (5.5f <= worldposx && worldposx <= 7.5f && -1.67f <= worldposy && worldposy <= 1.67f && 4.5f <= transform.position.x && transform.position.x <=8.5f){
                //&& 4.5f <= transform.position.x && transform.position.x <=8.5f
                if (color.color != Color.yellow){
                    AudioSource.PlayClipAtPoint(touchsound, transform.position);// sound played on the same position
                    color.color = Color.yellow;
                    // appel a la fonction Scoreupdate de la class score
                    FindObjectOfType<score>().Scoreupdate(scorevalue);
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        }
    }

    void generateref(){
        if(transform.position.x >= detectionX){
            hasPassedref = true;
            AudioSource.PlayClipAtPoint(refsound, transform.position);
            // sound played on the same position
        }
    }
}
