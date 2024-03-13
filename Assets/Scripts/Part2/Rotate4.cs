using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate4 : MonoBehaviour
{
private float initialDistance;
private float newDistance;
 
private void OnMouseDown() {
    // if the second touch just started
    if(Input.touchCount == 2 && Input.GetTouch(1).phase == TouchPhase.Began) {
        Vector2 touch1 = Input.GetTouch(0).position;
        Vector2 touch2 = Input.GetTouch(1).position;
 
        // save the initial distance
        initialDistance = (touch1 - touch2).sqrMagnitude;
 
    }
}
 
private void OnMouseDrag() {
    // if there are still 2 touches and one of the first two touches moved
    if(Input.touchCount >= 2 && (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)) {
        Vector2 touch1 = Input.GetTouch(0).position;
        Vector2 touch2 = Input.GetTouch(1).position;
 
        newDistance = (touch1 - touch2).sqrMagnitude;
        float changeInDistance = newDistance - initialDistance;
        float percentageChange = changeInDistance / initialDistance;
 
        Vector3 newScale = transform.localScale;
        newScale += percentageChange * transform.localScale * 0.1f;
 
        transform.localScale = newScale;
    }
}
 
private void OnMouseUp() {
    // less than two touches, reset the initial distance
    if(Input.touchCount < 2) {
        initialDistance = 0;
    }
}
}
