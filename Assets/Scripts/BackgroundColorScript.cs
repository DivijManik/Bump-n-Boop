using System.Collections;
using System.Collections.Generic;
using Imphenzia;
using UnityEngine;

public class BackgroundColorScript : MonoBehaviour
{
    [SerializeField]
    Gradient[] RandColours;
    
    void Start()
    {
        transform.GetComponent<GradientSkyCamera>().gradient = RandColours[Random.Range(0, RandColours.Length)];
    }
}
