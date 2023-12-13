using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaling : MonoBehaviour
//Permet de changer la taille d'un object en utilisant deux doigts
{
    private float initialDistance;
    private float newDistance;
    public float sensibility;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // si deux touches et une bouge 
        if (Input.touchCount ==2 && (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)) {
            Vector2 touch1 = Input.GetTouch(0).position;
            Vector2 touch2 = Input.GetTouch(1).position;

            newDistance = (touch1 - touch2).sqrMagnitude;
            float changeInDistance = newDistance - initialDistance;
            float percentageChange = (changeInDistance / initialDistance) * sensibility; //c'est tres sensible

            Vector3 newScale = transform.localScale;
            newScale += percentageChange * transform.localScale;

            transform.localScale = newScale;
        }
    }

    private void OnMouseDown(){
        //verifie si 2 touches
        if (Input.touchCount == 2 && Input.GetTouch(1).phase == TouchPhase.Began) {
            Vector2 touch1 = Input.GetTouch(0).position;
            Vector2 touch2 = Input.GetTouch(1).position;

            initialDistance = (touch1 - touch2).sqrMagnitude;
        }
    }

    private void OnMouseUp(){
        //si moins de 2 touches, remet la distance initiale 
        if (Input.touchCount<2){
            initialDistance=0;
        }
    }
}
