using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjRotattionScript : MonoBehaviour
{
    [SerializeField]
    Vector3[] EulerAngles;

    void Start()
    {
        transform.eulerAngles = EulerAngles[Random.Range(0, EulerAngles.Length)];
    }
}


