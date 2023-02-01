using System.Collections;
using UnityEngine;

public class WhaleMovement : MonoBehaviour
{
    public Transform[] checkpoints;
    public Transform[] bezierPoints;
    public float amplitude = 0.8f;
    public float frequency = 1f;
    public float movementDuration = 2f;

    private void Start()
    {
        GoToCheckpoint(1, 4, 0);
    }

    private void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position, endPoint.position, speed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, amplitude * Mathf.Sin(frequency * Time.time), transform.position.z);
    }

    public void GoToCheckpoint(int index, float duration, float delay)
    {
        StartCoroutine(GoTo(checkpoints[index].position, duration,delay,bezierPoints[index-1].position));
    }

    private IEnumerator GoTo(Vector3 targetPos, float duration, float delay, Vector3 bezierPos)
    {
        yield return new WaitForSeconds(delay);

        float progress = 0;
        Vector3 startingPos = transform.position;

        while (progress <= 1)
        {
            float t = Mathf.SmoothStep(0, 1, progress);
            transform.position = (1 - t)*(1-t) * startingPos + 2*t*(1-t)* bezierPos + t*t*targetPos;
            progress += Time.deltaTime / duration;
            transform.rotation = Quaternion.LookRotation(-(-2 * (1 - t) * startingPos + 2 * (1 - 2 * t) * bezierPos + 2 * t * targetPos));
            yield return null;
        }
    }

}
