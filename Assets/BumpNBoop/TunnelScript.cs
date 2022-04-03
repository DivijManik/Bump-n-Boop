using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelScript : MonoBehaviour
{
    [SerializeField]
    List<Transform> tunnelObj;

    void FixedUpdate()
    {
        transform.Translate(0, 0, -0.1f);

        if (tunnelObj[0].transform.position.z < -75f)
        {
            Transform t = tunnelObj[0];
            tunnelObj.Remove(t);

            t.transform.position = tunnelObj[tunnelObj.Count - 1].position + new Vector3(0, 0, 60);

            tunnelObj.Add(t);
        }
    }
}
