using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class MvtRectiligne : MonoBehaviour
{
    public static float speed = 2.8f; //1.5f / spawnereaction.tempo

    void Start(){
    }

    void Update(){
        transform.Translate(- Vector2.left * speed * Time.deltaTime);
    }
}
