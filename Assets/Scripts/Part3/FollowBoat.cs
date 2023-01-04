using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBoat : MonoBehaviour
{
    public float smoothing;
    public float rotSmoothing;
    public Transform boat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = UnityEngine.Vector3.Lerp(transform.position, boat.position, smoothing);
        transform.rotation = Quaternion.Slerp(transform.rotation, boat.rotation, rotSmoothing);
    }
}
