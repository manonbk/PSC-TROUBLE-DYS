using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaling : MonoBehaviour
//Permet de changer la taille d'un object en utilisant deux doigts
{
    private float initialDistance;
    private float newDistance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount ==2 && (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)) {
            Vector3 touch1 = Input.GetTouch(0).position;
            Vector3 touch2 = Input.GetTouch(1).position;

            newDistance = (touch1 - touch2).sqrMagnitude;
            float changeInDistance = newDistance - initialDistance;
            float percentageChange = changeInDistance / initialDistance;

            Vector3 newScale = transform.localScale;
            newScale += percentageChange * transform.localScale;

            transform.localScale = newScale;
        }
    }

    private void OnMouseDown(){
        //verifie si 2 touches
        if (Input.touchCount == 2 && Input.GetTouch(1).phase == TouchPhase.Began) {
            Vector3 touch1 = Input.GetTouch(0).position;
            Vector3 touch2 = Input.GetTouch(1).position;

            initialDistance = (touch1 - touch2).sqrMagnitude;
        }
    }
}
