using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshScript : MonoBehaviour {

   

    void Start()
    {

       //gameObject.AddComponent<MeshFilter>();

       
    }


    void Update()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;

        //Debug.Log(normals.Length);

        for (var i = 0; i < vertices.Length; i++)
        {
            vertices[i] += normals[i] * Mathf.Sin(Time.time)* 0.01f;
        }

        mesh.vertices = vertices;


        //transform.localScale += transform.localScale* Mathf.Sin(Time.time) * 0.01f;

        transform.Translate(0,Mathf.Sin(Time.time)*0.01f , 0);

       // Debug.Log(Time.time);
    }
}
