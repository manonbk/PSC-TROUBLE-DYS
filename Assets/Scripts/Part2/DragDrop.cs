using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    public GameObject ObjectToDrag;
    public GameObject ObjectDragToPos;
    public float DropDistance;
    public bool islocked;
    Vector2 objectInitPos;
    void Start()
    {
        objectInitPos = ObjectToDrag.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DragObject() {
        if(!islocked) {
            ObjectToDrag.transform.position = Input.mousePosition;
        }
    }

    public void DropObject() {
        float Distance = Vector3.Distance(ObjectToDrag.transform.position,ObjectDragToPos.transform.position);
        if(Distance<DropDistance) {
            islocked=true;
            ObjectToDrag.transform.position = ObjectDragToPos.transform.position;
        }
        else {
            ObjectToDrag.transform.position = objectInitPos;
        }
    }
}

