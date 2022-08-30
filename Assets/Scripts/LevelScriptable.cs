using UnityEngine;

[CreateAssetMenu(fileName = "LevelSettings", menuName = "ScriptableObjects/LevelSettings", order = 1)]
public class LevelScriptable : ScriptableObject
{
    public int NoOfBalls;

    public float speed;

    public Transform[] Obstacles;
    public Transform[] ColObj;

    public bool ShowAd;

    public bool CubeSpawnLvl;
    public bool PlaneLevel;

    public int  NoOfGems;
    public int LevelRewardGems;

    [Header("Boost value is multiplied to speed ( which is the tunnel speed)")]
    public float BoostMultiplier;

    public LevelMode Mode;

    public int Obstacle_ZPos = 60;

}