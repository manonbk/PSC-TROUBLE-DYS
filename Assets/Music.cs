using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    
    public AudioClip musique;
    // Start is called before the first frame update
    void Start()
    {
        AudioSource.PlayClipAtPoint(musique, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
