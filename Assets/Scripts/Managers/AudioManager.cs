using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] AudioSource BgAudio;

    private void Awake()
    {
        if(Instance== null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void BG_MusicSpeed(bool SpeedUp)
    {
        if (SpeedUp)
        {
            BgAudio.pitch = 1.06f;
        }
        else
        {
            BgAudio.pitch = 1f;
        }
    }
}
