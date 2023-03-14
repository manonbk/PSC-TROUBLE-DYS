using System.Collections;
using UnityEngine;

public class SmoothMove : MonoBehaviour
{
    public Vector3[] checkpoints;
    private int lastCheckPoint = 0;

    private void Start()
    {
        transform.position = checkpoints[0];
    }
    public void MoveToPosition(Vector3 targetPosition, float duration)
    {
        StartCoroutine(SmoothMovement(targetPosition,duration));
    }

    public void GoToCheckpoint(int i, float duration)
    {
        if (i < checkpoints.Length)
        MoveToPosition(checkpoints[i],duration);
        lastCheckPoint = i;
    }


    private IEnumerator SmoothMovement(Vector3 targetPosition,float duration)
    {
        float progress = 0;
        Vector3 startingPos = transform.position;

        while (progress <= 1)
        {
            transform.position = Vector3.Lerp(startingPos, targetPosition, Mathf.SmoothStep(0,1,progress));
            progress += Time.deltaTime / duration;
            yield return null;
        }
    }
}