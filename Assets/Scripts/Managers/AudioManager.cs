using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] AudioSource BgAudio;

    [SerializeField] AudioSource SoundEffectsAS;

    [SerializeField] AudioClip[] soundEffects;

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

    public void PlaySoundEffect(Sounds s)
    {
        SoundEffectsAS.clip = soundEffects[(int)s];
        SoundEffectsAS.Play();
       
    }
}

public enum Sounds
{
    ButtonClick,
    ObstacleCollide,
    GoodCollide
}