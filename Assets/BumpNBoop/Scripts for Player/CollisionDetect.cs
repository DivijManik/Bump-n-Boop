using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollisionDetect : MonoBehaviour
{
    //public bool remove = false;
    MeshRenderer Our;

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
        Our = gameObject.GetComponent<MeshRenderer>();

        StartCoroutine(WaitToDetect());   // WaitToDetect()
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (DetecCollision == false || other.transform.parent.name == "Player")
        {
            return;
        }
        if (other.transform.childCount >0 )
        {
            Transform canvas = other.transform.GetChild(0);
            Text TextGO = canvas.GetChild(0).GetComponent<Text>();

            otherMat = TextGO.text.Substring(0, 1);

           // Destroy(other.transform.gameObject);
        }
        else
        {
            MeshRenderer OtherMR = other.gameObject.GetComponent<MeshRenderer>();
            otherMat = OtherMR.material.name.Substring(0, 1);

            //string ourMat = Our.material.name.Substring(0, 1);
            
        }
       // if(otherMat == ourMat)
        //{
            //Debug.Log("YES");
            //remove = true;
          //  PC.RemoveBalls();
       // }
        //else
        //{
            PC.InsertBalls(otherMat);
        //}


        Destroy(other.transform.parent.gameObject);


    }

    IEnumerator WaitToDetect()
    {
        yield return new WaitForSeconds(1);

        DetecCollision = true;
    }
}
