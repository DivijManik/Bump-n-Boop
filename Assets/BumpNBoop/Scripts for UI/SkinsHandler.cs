using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;

public class SkinsHandler : MonoBehaviour
{
    [SerializeField]
    Transform skinsGrid; // for configuring if children are locked or unlocked

    private RewardedAd rewardedAd;
    AdRequest request;
    private void Start()
    {
#if UNITY_ANDROID
        this.rewardedAd = new RewardedAd("ca-app-pub-9285045534177890/5117380834");
#elif UNITY_IPHONE
        this.rewardedAd = new RewardedAd("ca-app-pub-9285045534177890/4159522385");
#endif
        // Create an empty ad request.
        request = new AdRequest.Builder().Build();
        

        if (!PlayerPrefs.HasKey("Skins"))
        {
            //Default Skin
            OnSkinUnlocked("Ball");
        }

        string s = PlayerPrefs.GetString("Skins");

        foreach (Transform skin in skinsGrid)
        {
            Transform tChild = skin.GetChild(0);
            string skinName = tChild.GetChild(0).GetComponent<TextMeshProUGUI>().text;

            if (s.Contains(skinName))
            {
                skin.GetComponent<Button>().onClick.AddListener(delegate { OnSkinClicked(skin.name); });
                skin.GetChild(1).gameObject.SetActive(false); // lock
            }
            else
            {
                skin.GetComponent<Button>().onClick.AddListener(delegate { UnlockUsingAd(skin.name); });

            }
        }
        
  
    }

    private void OnSkinClicked(string s)
    {
        if(PlayerPrefs.GetString("Skins").Contains(s))
            PlayerPrefs.SetString("currentSkin", s);

        SceneManager.LoadScene(0);
    }

    public void OnSkinUnlocked(string skinName)
    {
        string s = PlayerPrefs.GetString("Skins");
        s += skinName;

        PlayerPrefs.SetString("Skins", s); 
    }
    string s_;
    void UnlockUsingAd(string s)
    {
        s_ = s;

        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);

        this.rewardedAd.OnUserEarnedReward += RewardedAd_OnUserEarnedReward; 

        
    }

    private void RewardedAd_OnUserEarnedReward(object sender, Reward e)
    {
        string s = PlayerPrefs.GetString("Skins");
        if (!s.Contains(s_))
            s += s_;

        PlayerPrefs.SetString("Skins", s);

        SceneManager.LoadScene(0);
    }
}
