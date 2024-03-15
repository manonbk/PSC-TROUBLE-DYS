using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
float rotSpeed = 20;

void OnMouseDrag(){
    float rotX = Input.GetAxis("Mouse X") * rotSpeed * Mathf.Deg2Rad;
    float rotY = Input.GetAxis("Mouse Y") * rotSpeed * Mathf.Deg2Rad;

    transform.Rotate(Vector2.up, -rotX);
    transform.Rotate(Vector2.right, rotY);
    }
}
