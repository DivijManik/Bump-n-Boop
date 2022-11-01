using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;
using System;

public class SkinsHandler : MonoBehaviour
{
    [SerializeField]
    Transform skinsGrid; // for configuring if children are locked or unlocked

    private RewardedAd rewardedAd;
    AdRequest request;
    private void Start()
    {
        string adUnitId;
#if UNITY_ANDROID
        adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
            adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
            adUnitId = "unexpected_platform";
#endif
        AdRequest request = new AdRequest.Builder().Build();

        this.rewardedAd = new RewardedAd(adUnitId);
        this.rewardedAd.OnUserEarnedReward += RewardedAd_OnUserEarnedReward;

        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);

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
        Debug.Log("showing ad");
        s_ = s;


        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();

        }
        else
        {
            StartCoroutine(WaitShowAd());
        }
    }
    private void RewardedAd_OnUserEarnedReward(object sender, Reward e)
    {
        string s = PlayerPrefs.GetString("Skins");
        if (!s.Contains(s_))
            s += s_;

        PlayerPrefs.SetString("Skins", s);
        PlayerPrefs.SetString("currentSkin", s_);

        SceneManager.LoadScene(0);
    }

    IEnumerator WaitShowAd()
    {
        yield return new WaitUntil(AdLaoded);

        this.rewardedAd.Show();
    }

    private bool AdLaoded()
    {
        return this.rewardedAd.IsLoaded();
    }
}
