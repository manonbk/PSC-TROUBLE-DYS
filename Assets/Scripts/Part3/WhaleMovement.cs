using UnityEngine;

public class WhaleMovement : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float amplitude = 0.5f;
    public float frequency = 1f;
    public float speed = 2f;

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, endPoint.position, speed * Time.deltaTime);
        //transform.position = new Vector3(transform.position.x, transform.position.y + amplitude * Mathf.Sin(frequency * Time.time), transform.position.z);
    }
    
}
