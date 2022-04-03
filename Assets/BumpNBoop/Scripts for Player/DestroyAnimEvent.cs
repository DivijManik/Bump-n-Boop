using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAnimEvent : MonoBehaviour {

    PlayerController PC;

    private void Start()
    {
        PC = PlayerController.Instance;
    }
    
    public void DestroyEvn()
    {
        if (transform.childCount > 0)
        {
            transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            transform.GetChild(0).parent = null;
        }
        PC.Balls.Remove(this.transform);
        PC.RemoveBalls();
        Destroy(this.gameObject);
    }

}
