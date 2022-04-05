using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallIntervalScript : MonoBehaviour {

	void FixedUpdate () {
		//if(transform.position.z < -1f)
  //      {
  //          Destroy(this.transform.gameObject);
  //      }
  //      else
  //      {
            transform.Translate(0, 0, -0.14f, Space.World);
  //      }
	}
}
