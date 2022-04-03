using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger_evt : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (other.tag=="Player")
        {

            rb.useGravity = true;
            rb.isKinematic = false;
        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (other.tag == "Player")
        {

            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }



}
