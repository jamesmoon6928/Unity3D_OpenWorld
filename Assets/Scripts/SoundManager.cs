using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    //Making SoundManager as a singleton
    static public SoundManager instance;

    #region singleton
    //Run only one when object created
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion singleton

    public AudioSource[] audioSourceEffects;
    public AudioSource audioSourceBgm;

    public string[] playSoundName;

    public Sound[] soundEffects;
    public Sound[] bgmSounds;

    void Start()
    {
        playSoundName = new string[audioSourceEffects.Length];
    }

    public void PlaySE(string name)
    {
        for(int i = 0; i < soundEffects.Length; i++)
        {
            if(name == soundEffects[i].name)
            {
                for (int j = 0; j < audioSourceEffects.Length; j++)
                {
                    if (!audioSourceEffects[j].isPlaying)
                    {
                        playSoundName[j] = soundEffects[i].name;
                        audioSourceEffects[j].clip = soundEffects[i].clip;
                        audioSourceEffects[j].Play();
                        return;
                    }
                }
                Debug.Log("All Available AudioSources are in use");
                return;
            }
        }
        Debug.Log(name + " does not exist in SoundManager");
    }

    public void StopAllSE()
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            audioSourceEffects[i].Stop();
        }
    }

    public void StopSE(string name)
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            if (playSoundName[i] == name)
            {
                audioSourceEffects[i].Stop();
                return;
            }
        }
        Debug.Log("There's no sound: " + name + "in AudioSource");
    }
}
