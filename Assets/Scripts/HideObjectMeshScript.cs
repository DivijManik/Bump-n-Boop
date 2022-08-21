using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObjectMeshScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Obstacle") || other.CompareTag("ColorObj"))
        {
            other.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Obstacle") || other.CompareTag("ColorObj"))
        {
            other.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
