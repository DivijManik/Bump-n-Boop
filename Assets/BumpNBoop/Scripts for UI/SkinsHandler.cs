using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SkinsHandler : MonoBehaviour
{
    [SerializeField]
    Transform skinsGrid; // for configuring if children are locked or unlocked

    private void Start()
    {
        if (!PlayerPrefs.HasKey("Skins"))
        {
            //Default Skin
            OnSkinUnlocked("Ball");
        }

        int f = 0;
        string s = PlayerPrefs.GetString("Skins");

        foreach (Transform skin in skinsGrid)
        {
            Transform tChild = skin.GetChild(0);
            string skinName = tChild.GetChild(0).GetComponent<TextMeshProUGUI>().text;

            if (s.Contains(skinName))
            {
                skin.GetComponent<Button>().onClick.AddListener(delegate { OnSkinClicked(f); });
                skin.GetChild(1).gameObject.SetActive(false);
            }

            if (f == PlayerPrefs.GetInt("currentSkin"))
            {
                Debug.Log("Current selected skin " + skinName);
            }

            f++;
        }
        
  
    }

    private void OnSkinClicked(int i)
    {
        PlayerPrefs.SetInt("currentSkin", i);
    }

    public void OnSkinUnlocked(string skinName)
    {
        string s = PlayerPrefs.GetString("Skins");
        s += skinName;

        PlayerPrefs.SetString("Skins", s); 
    }
}
