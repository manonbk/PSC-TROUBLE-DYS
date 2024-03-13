using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountFrame : MonoBehaviour
{
    public int nb = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Time.frameCount % 50 == 0){
            //Debug.Log("Time (fame count): " + Time.frameCount + ", Time (s): " + Time.time);
        Debug.Log(nb + " " + Time.time);
        //}
        nb++;
    }
}
