using System.Collections;
using UnityEngine;

public class SmoothMove : MonoBehaviour
{

    public float duration;
    public Vector3[] checkpoints;
    private int lastCheckPoint = -1;
    public void MoveToPosition(Vector3 targetPosition)
    {
        StartCoroutine(SmoothMovement(targetPosition));
    }

    public void GoToCheckpoint(int i)
    {
        MoveToPosition(checkpoints[i]);
        lastCheckPoint = i;
    }

    public void goToNext()
    {
        GoToCheckpoint(lastCheckPoint+1);
    }

    private IEnumerator SmoothMovement(Vector3 targetPosition)
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