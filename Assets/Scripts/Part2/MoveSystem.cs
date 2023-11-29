using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSystem : MonoBehaviour
//ce script permet de bouger l'objet et de le bloquer sur la bonne forme
{
    public GameObject correctForm; //cible
    private bool moving;
    private bool finish; //est-ce vraiment la bonne forme ?
    private float startPosX;
    private float startPosY;

    private Vector3 resetPosition; //position de départ si erreur 


    void Start()
    {
        resetPosition = this.transform.localPosition;
    }

    void Update()
    {
        if (finish == false) { //on ne peut plus bouger l'objet s'il est sur la bonne forme
            if (moving) {
                Vector3 mousePos;
                mousePos = Input.mousePosition;
                mousePos = Camera.main.ScreenToWorldPoint(mousePos);

                transform.localPosition = new Vector3(mousePos.x - startPosX, mousePos.y - startPosY, transform.localPosition.z);
            }
        }
    }
    
    private void OnMouseDown() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            startPosX = mousePos.x - transform.localPosition.x;
            startPosY = mousePos.y - transform.localPosition.y;

            moving = true;
        }
    }

    private void OnMouseUp() {
        moving = false;

        if (Mathf.Abs(transform.localPosition.x - correctForm.transform.localPosition.x) <= 0.1f &&  //distance 0,5 à redéfinir = précision qu'on attend pour lacher
            Mathf.Abs(transform.localPosition.y - correctForm.transform.localPosition.y) <= 0.1f) {
                this.transform.position = new Vector3(correctForm.transform.position.x,correctForm.transform.position.y,correctForm.transform.position.z);
                finish = true;
            }
        else {
            this.transform.localPosition = new Vector3(resetPosition.x,resetPosition.y, resetPosition.z);
        }
    }

}
