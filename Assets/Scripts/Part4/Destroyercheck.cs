using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyercheck : MonoBehaviour
{
    public AudioClip passSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col){
        Destroy(col.gameObject);
        AudioSource.PlayClipAtPoint(passSound, transform.position);

    }
}
