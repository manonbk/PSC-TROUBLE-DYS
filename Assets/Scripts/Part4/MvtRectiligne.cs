using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MvtRectiligne : MonoBehaviour
{
    private float speed = 3f;

    void Start(){
    }

    void Update(){
        transform.Translate(- Vector2.left * speed * Time.deltaTime);
    }
}
