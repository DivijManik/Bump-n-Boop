using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class StartButtonAction : MonoBehaviour {

    [SerializeField]
    GameObject ScriptHolderPANEL;

    [SerializeField]
    GameObject PauseButton;

    // For volume and vibration
    [SerializeField]
    Sprite VolOffImg, VolOnImg;
    [SerializeField]
    Sprite VibOffImg, VibOnImg;

    [SerializeField]
    Image VolImg;
    [SerializeField]
    Image VibImg;

    [SerializeField]
    PostProcessVolume postProcess;

    private void Start()
    {
        //ScriptHolderPANEL.gameObject.SetActive(false);

        if(PlayerPrefs.HasKey("UseVolume") )
        {
            InitVol();
        }
        if(PlayerPrefs.HasKey("UseVibration"))
        {
            InitVib();
        }
    }
    public void StartButtonClick()
    {
        PlayerController.Instance.StartGame = true;
        postProcess.weight = 0;

        //ScriptHolderPANEL.gameObject.SetActive(true);
        transform.parent.gameObject.SetActive(false);
        PauseButton.gameObject.SetActive(true);
    }

    public void InstructionClick()
    {
        transform.gameObject.SetActive(false);
    }

    /// <summary>
    ///
    ///                 VOLUME AND VIBRATION
    /// 
    /// </summary>
    public void OnVolumeButtonClicked()
    {
        if(!PlayerPrefs.HasKey("UseVolume"))
        {
            PlayerPrefs.SetInt("UseVolume", 0);

        }
        else
        {
            int useVol = PlayerPrefs.GetInt("UseVolume");

            if (useVol == 0)
            {
                PlayerPrefs.SetInt("UseVolume", 1);
            }
            else
            {
                PlayerPrefs.SetInt("UseVolume", 0);
            }
        }

        InitVol();
    }

    public void OnVibrationButtonClicked()
    {
        if (!PlayerPrefs.HasKey("UseVibration"))
        {
            PlayerPrefs.SetInt("UseVibration", 0);

        }
        else
        {
            int useVol = PlayerPrefs.GetInt("UseVibration");

            if (useVol == 0)
            {
                PlayerPrefs.SetInt("UseVibration", 1);
            }
            else
            {
                PlayerPrefs.SetInt("UseVibration", 0);
            }
        }

        InitVib();
    }

    private void InitVib()
    {
        if(PlayerPrefs.GetInt("UseVibration") == 0)
        {
            VibImg.sprite = VibOffImg;
            PlayerController.Instance.UseVib = false;
        }
        else
        {
            VibImg.sprite = VibOnImg;
            PlayerController.Instance.UseVib = true;
        }
    }

    private void InitVol()
    {
        if (PlayerPrefs.GetInt("UseVolume") == 0)
        {
            VolImg.sprite = VolOffImg;
            FindObjectOfType<AudioListener>().enabled = false;
        }
        else
        {
            VolImg.sprite = VolOnImg;
            FindObjectOfType<AudioListener>().enabled = true;
        }
    }
}
