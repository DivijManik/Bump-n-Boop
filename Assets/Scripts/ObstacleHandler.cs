using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleHandler : MonoBehaviour
{
    // Obstacle & collectibles z pos is to be 60f
    const float zPos = 60f;
    
    List<Transform> StackObstacle = new List<Transform>();
    List<Transform> StackCol = new List<Transform>();

    //Boost
    [SerializeField] List<Transform> Boosts = new List<Transform>();

    private void FixedUpdate()
    {
        if (PlayerController.Instance.StartGame)
        {
            BoostForPlayer();
            Obstacles(PlayerController.Instance.PlayerLevel);
        }
    }

    // BOOST 
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

    // StackObstacle & StackCol after new case in switch statement

    void Obstacles(int level)
    {
        if (Mathf.Abs(Time.time) % 2 == 0)
        {
            //switch (level)
            //{
            //    case int n when (n <= 25):

            Transform t;
            int random_ = Random.Range(0, 2);
            // OBSTACLE
            if (random_ == 0)
            {
                if (StackObstacle.Count < 25) // Instantiate only 25 objects
                {
                    int rand = Random.Range(0, LevelManager.Instance.LevelSettings[level].Obstacles.Length);
                    t = Instantiate(LevelManager.Instance.LevelSettings[level].Obstacles[rand], transform);
                    StackObstacle.Add(t);
                }
                else
                {
                    int rand = Random.Range(0, StackObstacle.Count);
                    while (StackObstacle[rand].position.z >= -10)
                    {
                        rand = Random.Range(0, StackObstacle.Count);
                    }
                    t = StackObstacle[rand];
                }
            } // COLOR OBJECT
            else
            {
                if (StackCol.Count < 25) // Instantiate only 25 objects
                {
                    int rand = Random.Range(0, LevelManager.Instance.LevelSettings[level].ColObj.Length);
                    t = Instantiate(LevelManager.Instance.LevelSettings[level].ColObj[rand], transform);
                    StackCol.Add(t);
                }
                else
                {
                    int rand = Random.Range(0, StackCol.Count);
                    while (StackCol[rand].position.z >= -10)
                    {
                        rand = Random.Range(0, StackCol.Count);
                    }
                    t = StackCol[rand];
                }
                // SET BLOCK COLOR
                BlocksColor(t);
            }

            if (t != null)
                t.position = new Vector3(t.position.x, t.position.y, zPos);

            //    break;

            //case int n when (n <= 50):

            //    break;
            //}
        }
    }


    /// <summary>
    ///
    ///             FOR COLOR OBJECTS
    ///             Random Color === Block color to Ball color
    ///
    /// </summary>

    void BlocksColor(Transform Blocks)
    {
        int childWithSameColor = Random.Range(0, Blocks.childCount); // Get a random child object from blockParent

        int iter = 0;
        string BallMatName = PlayerController.Instance.Balls[0].GetComponent<MeshRenderer>().material.name.Substring(0, 1);

        foreach (Transform childBlock in Blocks)
        {
            MeshRenderer BlockMR = childBlock.GetComponent<MeshRenderer>();

            if (iter != childWithSameColor)
            {
                int RandMat = Random.Range(0, PlayerController.Instance.MatPrefabs.Length);
                BlockMR.material = PlayerController.Instance.MatPrefabs[RandMat];

                while (PlayerController.Instance.MatPrefabs[RandMat].name.Substring(0, 1) == BallMatName)
                {
                    RandMat = Random.Range(0, PlayerController.Instance.MatPrefabs.Length);
                    BlockMR.material = PlayerController.Instance.MatPrefabs[RandMat];
                }
            }
            else
            {
                BlockMR.material = MatName(BallMatName);
            }
            //else
            //{
            //    string BallMatName = PlayerController.Instance.Balls[1].GetComponent<MeshRenderer>().material.name.Substring(0, 1);

            //    BlockMR.material = MatName(BallMatName);
            //}

            iter++;

        }
    }

    Material MatName(string BallMatName)
    {
        if (BallMatName == "b")
        {
            return PlayerController.Instance.MatPrefabs[0];
        }
        else if (BallMatName == "g")
        {
            return PlayerController.Instance.MatPrefabs[1];
        }
        else if (BallMatName == "o")
        {
            return PlayerController.Instance.MatPrefabs[2];
        }
        else if (BallMatName == "r")
        {
            return  PlayerController.Instance.MatPrefabs[3];
        }
        else if (BallMatName == "y")
        {
             return PlayerController.Instance.MatPrefabs[4];
        }

        return null;
    }

    public void DestroyLastLevelObstacles()
    {
        StackCol.Clear();
        StackObstacle.Clear();

        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }
    }

}
