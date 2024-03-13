using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transportation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y != -2.5f){// if has been teletransported or not
            passtest();
        }
    }

    void passtest(){
        //Debug.Log("passing test");
        if (transform.position.x >= DrawGates.startteleport){
            //Debug.Log("test passed");
            //Debug.Log("at position: " + transform.position.x);
            transform.position = new Vector3(DrawGates.endteleport, -2.5f, 0);
            //haspassed = true;
        }

    }
}