using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSystem : MonoBehaviour
//ce script permet de bouger l'objet et de le bloquer sur la bonne forme
{
    public GameObject correctForm; //cible
    public float distance; //distance d'erreur autorisee
    public float taille; //taille d'erreur autorisee
    public float rotation; 

    private bool moving;
    private bool finish; //est-ce vraiment la bonne forme ?
    private float startPosX;
    private float startPosY;

    //private Vector3 resetPosition; UTILISER SI on veut revenir au point de depart quand on lache


    void Start()
    {
        //resetPosition = this.transform.localPosition; UTILISER SI on veut revenir au point de depart quand on lache
    }

    void Update()
    {
        if (finish==false) { //on ne peut plus bouger l'objet s'il est sur la bonne forme UTILISER SI on veut revenir au point de depart quand on lache
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
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            mousePos.z =0;

            startPosX = mousePos.x - transform.localPosition.x;
            startPosY = mousePos.y - transform.localPosition.y;

            moving = true;
        }
    }

    private void OnMouseUp() {
        moving = false;


        if (Mathf.Abs(transform.position.x - correctForm.transform.position.x) <= distance &&  //distance a redefinir = precision qu'on attend pour lacher
            Mathf.Abs(transform.position.y - correctForm.transform.position.y) <= distance) {
                    if (Mathf.Abs(transform.localScale.magnitude - correctForm.transform.localScale.magnitude) <= taille) {  //check de la taille
                        if(Mathf.Abs(transform.localRotation.eulerAngles.z - correctForm.transform.localRotation.eulerAngles.z) <= rotation) {
                            this.transform.position = new Vector3(correctForm.transform.position.x,correctForm.transform.position.y,correctForm.transform.position.z);
                            //this.transform.localScale.magnitude = correctForm.transform.localScale.magnitude;
                            //this.transform.localRotation.eulerAngles.z = correctForm.transform.localRotation.eulerAngles.z;
                            finish = true;
            }}}
        //else { UTILISER SI on veut revenir au point de depart quand on lache
            //this.transform.localPosition = new Vector3(resetPosition.x,resetPosition.y, resetPosition.z);
        //}
    }

}
