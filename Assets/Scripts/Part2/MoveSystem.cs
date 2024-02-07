using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSystem : MonoBehaviour
//ce script permet de bouger l'objet et de le bloquer sur la bonne forme
{
    public GameObject correctForm; //cible
    public float distance;
    private bool moving;
    private bool finish; //est-ce vraiment la bonne forme ?
    private float startPosX;
    private float startPosY;

    private Vector3 resetPosition; //position de depart si erreur 


    void Start()
    {
        resetPosition = this.transform.localPosition;
    }

    void Update()
    {
        if (!finish) { //on ne peut plus bouger l'objet s'il est sur la bonne forme
            if (moving) {
                Vector3 mousePos;
                mousePos = Input.mousePosition;
                mousePos = Camera.main.ScreenToWorldPoint(mousePos);
                mousePos.z = 0;

                transform.position = new Vector3(mousePos.x - startPosX, mousePos.y - startPosY, transform.position.z);
            }
        }
    }
    
    private void OnMouseDown() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousePos;
            mousePos = Input.mousePosition; // mouse position in screen
            mousePos = Camera.main.ScreenToWorldPoint(mousePos); // find mouse position in world
            mousePos.z = 0;

            startPosX = mousePos.x - transform.localPosition.x;
            startPosY = mousePos.y - transform.localPosition.y;

            moving = true;
        }
    }

    private void OnMouseUp() {
        moving = false;

        if (Mathf.Abs(transform.position.x - correctForm.transform.position.x) <= distance &&  //distance 0,5 a redefinir = precision qu'on attend pour lacher
            Mathf.Abs(transform.position.y - correctForm.transform.position.y) <= distance) {
                this.transform.position = new Vector3(correctForm.transform.position.x,correctForm.transform.position.y,correctForm.transform.position.z);
                finish = true;
            }
    }

}
