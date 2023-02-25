using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeacherModeManager : MonoBehaviour
{
    private int nextIndex = 0;

    private int[] sequence = { 0, 2, 3, 1, 3, 1, 0, 2 };

    private float activationTime;

    public float duration = 100.0f;

    public GameObject[] normalMode;
    public GameObject[] teacherMode;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPressed(int i)
    {
        print(i);
        print(sequence[nextIndex]);
        if (Time.time > activationTime + duration)
        {
            print("Too late");
            ResetSequence();
        }
        if (i == sequence[nextIndex])
        {
            
            if (nextIndex == 0)
            {
                print("SET TIME");
                activationTime = Time.time;
            }
            nextIndex++;
            if (nextIndex == sequence.Length)
            {
                print("ACTIVATION");
                Activate();
            }
        }
        else
        {
            print("Wrong order");
            ResetSequence();
        }

    }

    private void ResetSequence()
    {
        nextIndex = 0;
    }

    public void Activate()
    {
        for (int i = 0; i < normalMode.Length; i++)
        {
            normalMode[i].SetActive(false);
        }
        for (int i = 0; i < teacherMode.Length; i++)
        {
            teacherMode[i].SetActive(true);
        }
    }
    public void Deactivate()
    {
        nextIndex = 0;
        for (int i = 0; i < normalMode.Length; i++)
        {
            normalMode[i].SetActive(true);
        }
        for (int i = 0;i < teacherMode.Length; i++)
        {
            teacherMode[i].SetActive(false);
        }
    }
}
