using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnDrive : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Vector2 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 thisgam = new Vector2(this.transform.position.x, this.transform.position.y);
        float angle = anglewhat(mousepos, thisgam);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private float anglewhat(Vector2 a, Vector2 b){
        return Mathf.Atan2(a.y-b.y, a.x-b.x)*Mathf.Rad2Deg;
    }
}