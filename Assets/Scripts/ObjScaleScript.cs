using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class ObjScaleScript : MonoBehaviour
{
    [SerializeField]
    Transform[] ObjsToScale;

    [SerializeField]
    Vector3 ScaleValue;

    private void Awake()
    {
        foreach (Transform item in ObjsToScale)
        {
            item.localScale = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CollisionDetect>() != null)
        {
            foreach (Transform item in ObjsToScale)
            {
                item.DOScale(ScaleValue, 0.5f);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CollisionDetect>() != null)
        {
            foreach (Transform item in ObjsToScale)
            {
                item.localScale = Vector3.zero;
            }
        }
    }

}
