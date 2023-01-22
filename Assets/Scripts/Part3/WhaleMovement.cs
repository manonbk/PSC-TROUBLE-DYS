using System.Collections;
using UnityEngine;

public class WhaleMovement : MonoBehaviour
{
    public Transform[] checkpoints;
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
        StartCoroutine(GoTo(checkpoints[index].position, duration,delay));
    }

    private IEnumerator GoTo(Vector3 targetPosition, float duration, float delay)
    {
        yield return new WaitForSeconds(delay);

        float progress = 0;
        Vector3 startingPos = transform.position;

        while (progress <= 1)
        {
            transform.position = Vector3.Lerp(startingPos, targetPosition, Mathf.SmoothStep(0, 1, progress));
            progress += Time.deltaTime / duration;
            yield return null;
        }
    }

}
