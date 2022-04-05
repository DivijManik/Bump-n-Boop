using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleHandler : MonoBehaviour
{
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
        BoostForPlayer();
    }

    void BoostForPlayer()
    {
        if (Mathf.Abs(Time.time) % 3 == 0)
        {
            int rand = Random.Range(0, Boosts.Count - 1);

            if (Boosts[rand].position.z < -15)
            {
                Boosts[rand].position = new Vector3(Random.Range(-4f, 4f), Random.Range(-4f, 4f), 20f);
            }
        }
    }

}
