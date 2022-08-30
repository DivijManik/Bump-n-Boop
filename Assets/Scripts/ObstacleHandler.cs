using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Random = UnityEngine.Random;

public class ObstacleHandler : MonoBehaviour
{
    // Obstacle & collectibles z pos is to be 60f
    float zPos = 60f;
    
    List<Transform> StackObstacle = new List<Transform>();
    List<Transform> StackCol = new List<Transform>();

    //Boost
    [SerializeField] List<Transform> Boosts = new List<Transform>();

    Transform LastObj;

    private void Start()
    {
        zPos = LevelManager.Instance.LevelSettings[LevelManager.PlayerLvl()].Obstacle_ZPos;
    }

    private void FixedUpdate()
    {
        if (PlayerController.Instance.StartGame)
        {
            BoostForPlayer();
            int lvl = PlayerController.Instance.PlayerLevel;

            switch (LevelManager.Instance.LevelSettings[lvl].Mode)
            {
                case LevelMode.Normal:

                    Obstacles(lvl);

                    break;
                case LevelMode.Spiral:

                    Spiral_Obstacles(lvl);

                    break;
                default:
                    break;
            }
        }
    }

    // BOOST 
    void BoostForPlayer()
    {
        if (Mathf.Abs(Time.time) % 6 == 0)
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

            for (int i = 0; i < 3; i++)
            {

                Transform t = null;
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
                        Transform[] t_ = StackObstacle.Where(x => x.position.z <= -10).ToArray();

                        if (t_.Length > 0)
                        {
                            int rand = Random.Range(0, t_.Length);
                            //while (t_[rand].position.z >= -10)
                            //{
                            //    rand = Random.Range(0, t_.Length);
                            //}
                            t = t_[rand];
                        }
                        else
                        {
                            break;
                        }
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
                        Transform[] t_= StackCol.Where(x => x.position.z <= -10).ToArray();

                        if (t_.Length > 0)
                        {
                            int rand = Random.Range(0, t_.Length);
                            //while (t_[rand].position.z >= -10)
                            //{
                            //    rand = Random.Range(0, t_.Length);
                            //}
                            t = t_[rand];
                        }
                        else
                        {
                            break;
                        }
                    }
                    // SET BLOCK COLOR
                    if(t!=null)
                        BlocksColor(t);
                }

                if (t != null)
                {
                    if (LastObj != null)
                    {
                        t.position = new Vector3(t.position.x, t.position.y, LastObj.position.z + zPos);

                        //if (LevelManager.Instance.LevelSettings[level].Mode == LevelMode.Spiral)
                        //{
                        //    t.eulerAngles = new Vector3(0, 0, LastObj.eulerAngles.z + 10f);
                        //}
                    }
                    else
                    {
                        t.position = new Vector3(t.position.x, t.position.y, zPos);
                    }
                    LastObj = t;
                }
                //    break;

                //case int n when (n <= 50):

                //    break;
                //}
            }
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

    public void BlockColorOnNewBall()
    {
        if (LevelManager.Instance.LevelSettings[LevelManager.PlayerLvl()].Mode == LevelMode.Normal)
        {
            Transform[] t_ = StackCol.Where(x => x.position.z >= zPos).ToArray();

            foreach (Transform Blocks in t_)
            {
                int childWithSameColor = Random.Range(0, Blocks.childCount); // Get a random child object from blockParent

                string BallMatName = PlayerController.Instance.Balls[0].GetComponent<MeshRenderer>().material.name.Substring(0, 1);

                MeshRenderer[] childBlock = Blocks.GetComponentsInChildren<MeshRenderer>().Where(x => x.material.name.Substring(0, 1) == BallMatName).ToArray();

                if (childBlock.Length == 0)
                {
                    Blocks.GetChild(childWithSameColor).GetComponent<MeshRenderer>().material = MatName(BallMatName);
                }
            }
        }
        else
        {
            List<Transform> t_ = StackCol.Where(x => x.position.z >= zPos).ToList();

            Transform[] t_1 = t_.Where(x => x.childCount == 1).ToArray();

            Transform[] t_2 = t_.Where(x => x.childCount > 1).ToArray();

            //Transform

            foreach (Transform Blocks in t_1)
            {
                int changeColor = Random.Range(0, 3);

                if (changeColor == 1)
                {
                    string BallMatName = PlayerController.Instance.Balls[0].GetComponent<MeshRenderer>().material.name.Substring(0, 1);

                    MeshRenderer[] childBlock = Blocks.GetComponentsInChildren<MeshRenderer>().Where(x => x.material.name.Substring(0, 1) == BallMatName).ToArray();

                    if (childBlock.Length == 0)
                    {
                        Blocks.GetChild(0).GetComponent<MeshRenderer>().material = MatName(BallMatName);
                    }
                }
            }

            foreach (Transform Blocks in t_2)
            {
                int childWithSameColor = Random.Range(0, Blocks.childCount); // Get a random child object from blockParent

                string BallMatName = PlayerController.Instance.Balls[0].GetComponent<MeshRenderer>().material.name.Substring(0, 1);

                MeshRenderer[] childBlock = Blocks.GetComponentsInChildren<MeshRenderer>().Where(x => x.material.name.Substring(0, 1) == BallMatName).ToArray();

                if (childBlock.Length == 0)
                {
                    Blocks.GetChild(childWithSameColor).GetComponent<MeshRenderer>().material = MatName(BallMatName);
                }
            }
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
        LastObj = null;
        StackCol.Clear();
        StackObstacle.Clear();

        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }
    }

    /// <summary>
    ///
    ///
    ///                             Spiral levels > 50 level
    ///
    /// 
    /// </summary>
    /// <param name="level"></param>
    void Spiral_Obstacles(int level)
    {
        if (Mathf.Abs(Time.time) % 0.5 == 0)
        {
            //switch (level)
            //{
            //    case int n when (n <= 25):

            for (int i = 0; i < 3; i++)
            {

                Transform t = null;
                int random_ = Random.Range(0, 2);
                // OBSTACLE
                if (random_ == 0)
                {
                    if (StackObstacle.Count < 40) // Instantiate only 25 objects
                    {
                        int rand = Random.Range(0, LevelManager.Instance.LevelSettings[level].Obstacles.Length);
                        t = Instantiate(LevelManager.Instance.LevelSettings[level].Obstacles[rand], transform);
                        StackObstacle.Add(t);
                    }
                    else
                    {
                        Transform[] t_ = StackObstacle.Where(x => x.position.z <= -10).ToArray();

                        if (t_.Length > 0)
                        {
                            int rand = Random.Range(0, t_.Length);
                            //while (t_[rand].position.z >= -10)
                            {
                                //rand = Random.Range(0, t_.Length);
                            }
                            t = t_[rand];
                        }
                        else
                        {
                            break;
                        }
                    }
                } // COLOR OBJECT
                else
                {
                    if (StackCol.Count < 40) // Instantiate only 25 objects
                    {
                        int rand = Random.Range(0, LevelManager.Instance.LevelSettings[level].ColObj.Length);
                        t = Instantiate(LevelManager.Instance.LevelSettings[level].ColObj[rand], transform);
                        StackCol.Add(t);
                    }
                    else
                    {
                        Transform[] t_ = StackCol.Where(x => x.position.z <= -10).ToArray();

                        if (t_.Length > 0)
                        {
                            int rand = Random.Range(0, t_.Length);
                            //while (t_[rand].position.z >= -10)
                            {
                                //rand = Random.Range(0, t_.Length);
                            }
                            t = t_[rand];
                        }
                        else
                        {
                            break;
                        }
                    }
                    // SET BLOCK COLOR
                    if (t != null)
                        SpiralBlockColor(t);
                }

                if (t != null)
                {
                    if (LastObj != null)
                    {
                        t.position = new Vector3(t.position.x, t.position.y, LastObj.position.z + zPos);

                        t.eulerAngles = new Vector3(0, 0, LastObj.eulerAngles.z + 5f);
                    }
                    else
                    {
                        t.position = new Vector3(t.position.x, t.position.y, 60);
                    }
                    LastObj = t;
                }
                //    break;

                //case int n when (n <= 50):

                //    break;
                //}
            }
        }
    }

    void SpiralBlockColor(Transform Blocks)
    {
        int childWithSameColor = Random.Range(0, 2); // ..

        int iter = 0;
        string BallMatName = PlayerController.Instance.Balls[0].GetComponent<MeshRenderer>().material.name.Substring(0, 1);

        foreach (Transform childBlock in Blocks)
        {
            MeshRenderer BlockMR = childBlock.GetComponent<MeshRenderer>();

            if (childWithSameColor == 1)
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
}

public enum LevelMode
{
    Normal,
    Spiral
}
