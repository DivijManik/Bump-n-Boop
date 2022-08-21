using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField]
    public LevelScriptable[] LevelSettings;

    public int PlayerLevel;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("Level"))
        {
            PlayerLevel = PlayerPrefs.GetInt("Level");
        }

        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public static int PlayerLvl()
    {
        return Instance.PlayerLevel;
    }
}
