using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjRotattionScript : MonoBehaviour
{
    [SerializeField]
    Vector3[] EulerAngles;

    int dir = 1;
    int speed = 100;

    [SerializeField]
    RotationType rotationType;

    [SerializeField]
    RotationAxis rotationAxis;

    void Start()
    {
        if (rotationType == RotationType.RandomEulAngles)
        {
            transform.eulerAngles = EulerAngles[Random.Range(0, EulerAngles.Length)];
        }
        else
        {
            dir = Random.Range(0, 2) == 0 ? -1 : 1;
            speed = Random.Range(100, 150);
        }
    }

    private void FixedUpdate()
    {
        if (rotationType == RotationType.Continious)
        {
            if (rotationAxis == RotationAxis.Z)
            {
                transform.Rotate(0, 0, (speed * dir) * Time.deltaTime);
            }
            else if(rotationAxis == RotationAxis.Y)
            {
                transform.Rotate(0, (speed * dir) * Time.deltaTime, 0);
            }
            else // X
            {
                transform.Rotate( (speed * dir) * Time.deltaTime, 0, 0);
            }
        }
    }
}

enum RotationType
{
    RandomEulAngles,
    Continious
}

enum RotationAxis
{
    X,
    Y,
    Z
}


