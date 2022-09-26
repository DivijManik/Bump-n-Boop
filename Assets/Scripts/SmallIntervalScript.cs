using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallIntervalScript : MonoBehaviour {

    [SerializeField]
    Transform[] Tetris_shapes;

    private void Awake()
    {
        Instantiate(Tetris_shapes[Random.Range(0, Tetris_shapes.Length)], transform);

        MeshRenderer SIMeshRend = transform.GetChild(0).GetComponent<MeshRenderer>();

        int RandColor = Random.Range(0, PlayerController.Instance.MatPrefabs.Length);

        SIMeshRend.material = PlayerController.Instance.MatPrefabs[RandColor];
    }

    void FixedUpdate ()
	{
        if (!PlayerController.Instance.StartGame) { return; }

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
