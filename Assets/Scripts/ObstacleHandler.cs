using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleHandler : MonoBehaviour
{
    // Obstacle & collectibles z pos is to be 60f
    const float zPos = 60f;
    //PREFABS
    [SerializeField]
    List<Transform> level_25_Obstacles;

    [SerializeField]
    List<Transform> level_50_Obstacles;

    [SerializeField]
    List<Transform> level_100_Obstacles;

    [SerializeField]
    List<Transform> level_175_Obstacles;

    [SerializeField]
    List<Transform> level_300_Obstacles;

    //END

    List<Transform> Stack;

    //Boost
    [SerializeField] List<Transform> Boosts = new List<Transform>();

    // Stack are objects that are pooled for current round 
    void AddToStack()
    {
        if(PlayerController.Instance.PlayerLevel <= 25)
        {

        }
        else if (PlayerController.Instance.PlayerLevel <= 50)
        {

        }
        else if(PlayerController.Instance.PlayerLevel <= 100)
        {

        }
        else if (PlayerController.Instance.PlayerLevel <= 175)
        {

        }
        else if (PlayerController.Instance.PlayerLevel <= 300)
        {

        }
        
    }

    private void FixedUpdate()
    {
        if (PlayerController.Instance.StartGame)
        {
            BoostForPlayer();
            Obstacles(PlayerController.Instance.PlayerLevel);
        }
    }

    void BoostForPlayer()
    {
        if (Mathf.Abs(Time.time) % 5 == 0)
        {
            int rand = Random.Range(0, Boosts.Count - 1);

            if (Boosts[rand].position.z < -15)
            {
                Boosts[rand].position = new Vector3(Random.Range(-4f, 4f), Random.Range(-4f, 4f), zPos);
            }
        }       
    }

    void Obstacles(int level)
    {
        if (Mathf.Abs(Time.time) % 2 == 0)
        {
            switch (level)
            {
                case int n when (n <= 25):

                    Transform t;

                    if (transform.childCount < 25)
                    {
                        int rand = Random.Range(0, level_25_Obstacles.Count);
                        t = Instantiate(level_25_Obstacles[rand], transform);
                    }
                    else
                    {
                        int rand = Random.Range(0, transform.childCount);
                        while (transform.GetChild(rand).position.z >= -10)
                        {
                            rand = Random.Range(0, transform.childCount);
                        }
                        t = transform.GetChild(rand);
                    }

                    if(t!=null)
                        t.position = new Vector3(t.position.x,t.position.y,zPos);

                    //if (level_25_Obstacles[rand].position.z < -15)
                    //{
                    //    Boosts[rand].position = new Vector3(Random.Range(-4f, 4f), Random.Range(-4f, 4f), 20f);
                    //}

                    break;
                case int n when (n <= 50):

                    break;
            }
        }
    }

}
