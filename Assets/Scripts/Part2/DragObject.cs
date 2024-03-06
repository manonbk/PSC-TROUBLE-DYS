using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
//Ce script permet de bouger le game object dont c'est le script avec la souris/ le toucher sur tablette

{
    private Vector2 mOffset = Vector2.zero;
    //vecteur qui prend en compte le décalage entre le pointeur et le game object. Vector2.zero = (0,0)


    //OnMouseDown est un mot clé Unity. La méthode appelée quand la souris clique/le doigt touche le "collider" du GameObject
    void OnMouseDown()
    {
        //store offset = gameobject position - mouse position
        //transform.position est le vecteur position du game object. transform contient Position, rotation and scale of an object.
        mOffset = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
    }


    //OnMouseDrag est un mot clé : OnMouseDrag is called when the user has clicked on a Collider and is still holding down the mouse.
    void OnMouseDrag()
    {
        //On veut changer la position de l'object à la position de la souris, mais il faut prendre en compte le décalage initial
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) + mOffset;

    }

}
