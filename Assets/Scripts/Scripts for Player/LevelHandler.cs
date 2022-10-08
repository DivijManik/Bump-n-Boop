using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelHandler : MonoBehaviour
{
    [SerializeField]
    Transform levelGrid;

    [SerializeField]
    Transform levelPrefab;

    [SerializeField]
    Sprite levelOpenImg;

    [SerializeField]
    GameObject MainPanel;

    private void Start()
    {
        int lvl = PlayerPrefs.GetInt("Level");
        int currentLvl = PlayerPrefs.GetInt("CurrenLvl");

        if (lvl > currentLvl)
        {
           PlayerPrefs.SetInt("CurrenLvl", lvl);
           currentLvl = PlayerPrefs.GetInt("CurrenLvl");
        }

        for (int i = 0; i < LevelManager.Instance.LevelSettings.Length-1; i++)
        {
            Transform t = Instantiate(levelPrefab, levelGrid);
            t.position = new Vector3(0, -i * 300, 0);

            int f = i + 1;

            Transform tChild = t.GetChild(0);
            tChild.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Level " + f;

            if(f== lvl)
            {
                t.GetComponent<Button>().onClick.AddListener(delegate { OnPlayingLvlClicked(); });
                tChild.GetComponent<Image>().sprite = levelOpenImg;
            }
            else if(f<= currentLvl)
            {
                t.GetComponent<Button>().onClick.AddListener(delegate { OnLevelClicked(f); });
                tChild.GetComponent<Image>().sprite = levelOpenImg;
            }
        }
        levelGrid.GetComponent<RectTransform>().sizeDelta = new Vector2(1172,(LevelManager.Instance.LevelSettings.Length-1) / 3 * 350);

        Debug.Log(LevelManager.Instance.LevelSettings.Length % 3 * 350);
    }

    private void OnLevelClicked(int i)
    {
        int lvl = PlayerPrefs.GetInt("Level");
        int currentLvl = PlayerPrefs.GetInt("CurrenLvl");

        if (lvl > currentLvl)
        {
            PlayerPrefs.SetInt("CurrenLvl", lvl);
        }

        if (i <= currentLvl)
        {
            PlayerPrefs.SetInt("Level", i);
            SceneManager.LoadScene(0);
        }
    }

    private void OnPlayingLvlClicked()
    {
        MainPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}

// Line number => 50, 66, 71 commented for testing