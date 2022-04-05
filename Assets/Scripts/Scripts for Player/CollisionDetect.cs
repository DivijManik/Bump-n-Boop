using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollisionDetect : MonoBehaviour
{
    bool DetecCollision = false;

    public PlayerController PC;

    string otherMat;

    private void Start()
    {
        if (gameObject.GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        StartCoroutine(WaitToDetect());   // WaitToDetect()
    }

    // FOr Obstacles
    private void OnTriggerEnter(Collider other)
    {
        if (DetecCollision == false || other.transform.CompareTag("Player"))
        {
            return;
        }
        else if(other.transform.CompareTag("Boost"))
        {
            TunnelScript.Instance.StopCooldown();
            return;
        }

        //  get text object
        if (other.transform.childCount > 0)
        {
            Transform canvas = other.transform.GetChild(0);
            Text TextGO = canvas.GetChild(0).GetComponent<Text>();

            otherMat = TextGO.text.Substring(0, 1);
        }
        else
        {
            // or get block
            MeshRenderer OtherMR = other.gameObject.GetComponent<MeshRenderer>();
            otherMat = OtherMR.material.name.Substring(0, 1);
        }

        if (otherMat != "0")
        {
            PC.InsertBalls(otherMat);
        }
        else
        {
            // you collided with obstacle like axe // GAME OVER //

        }
        
        // move obstacle behind the view so Obstacle Handler can use it
        other.transform.parent.position = new Vector3(0, 0, -20);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.transform.CompareTag("Boost"))
        {
            TunnelScript.Instance.TunnelSpeed -= 0.1f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Boost"))
        {
            TunnelScript.Instance.StartCooldown();
        }
    }

    IEnumerator WaitToDetect()
    {
        yield return new WaitForSeconds(1);

        DetecCollision = true;
    }
}
