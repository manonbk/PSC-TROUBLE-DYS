/*using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DrawCircle : MonoBehaviour
{
    public int numSegments = 50; // Number of line segments to approximate the circle
    public float radius = 1f; // Radius of the circle

    public LineRenderer circleRenderer;

    void Start()
    {
        CircleOutline(numSegments, radius);
    }

    void CircleOutline(int steps, float radius){
        circleRenderer.positionCount = steps;

        for (int currentStep = 0; currentStep < steps; currentStep ++){
            float circumferenceProgress = (float) currentStep/steps;

            float currentRadian = circumferenceProgress * 2 * Mathf.PI;

            float xScaled = Mathf.Cos(currentRadian);
            float yScaled = Mathf.Sin(currentRadian);

            float x = xScaled * radius;
            float y = yScaled * radius;

            Vector3 currentPosition = new Vector3(x, y , 0 );

            circleRenderer.SetPosition(currentStep, currentPosition);
            
        }
    }
}
*/
using UnityEngine;

public class DrawCircle : MonoBehaviour
{
    public int numSegments = 50; // Number of line segments to approximate the circle
    public float radius = 1f; // Radius of the circle

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        // Calculate positions for the circle outline
        Vector3[] positions = new Vector3[numSegments + 1];
        float angle = 0f;
        float angleIncrement = 2f * Mathf.PI / numSegments;
        for (int i = 0; i <= numSegments; i++)
        {
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            positions[i] = new Vector3(x, y, 0f);
            angle += angleIncrement;
        }

        // Assign positions to the LineRenderer
        lineRenderer.positionCount = numSegments + 1;
        lineRenderer.SetPositions(positions);
    }
}
