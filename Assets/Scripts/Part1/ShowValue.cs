using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowValue : MonoBehaviour
{
    public string preText;
    public string postText;
    private string value;
    private TextMesh TextMesh;  // The Text Mesh component generates 3D geometry that displays text strings.

    // Start is called before the first frame update
    void Awake()
    {
        TextMesh = GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setValue(string value)
    {
        if (TextMesh != null)
        TextMesh.text = preText + value + postText;
    }
}
