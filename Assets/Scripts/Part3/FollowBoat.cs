using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBoat : MonoBehaviour
{
    public float smoothing;
    public float rotSmoothing;
    public Transform boat;

    Vector3 diff;
    // Start is called before the first frame update
    void Start()
    {
        diff = gameObject.transform.position - boat.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = boat.position + diff;
        //transform.rotation = Quaternion.Slerp(transform.rotation, boat.rotation, rotSmoothing);
    }
}
