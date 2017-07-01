using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class SoundManager : MonoBehaviour
{

    public AudioSource BS;
    public static SoundManager instance = null;
    public AudioClip button1;
    public AudioClip button2;
    public AudioClip button3;
    public bool mute = false;

    //private bool Smute = false;

    void Awake()
    {
        if (instance == null) { instance = this; }
        else if (instance != this) { Destroy(gameObject); }
        DontDestroyOnLoad(gameObject);
    }


    public void RandomizeSound(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        BS.clip = clips[randomIndex];
        BS.Play();
    }
    public void PlaySound()
    {
        if (mute == false) { RandomizeSound(button1, button2, button3); }
    }
    public void SetMute(bool b)
    {
        mute = b;
    }
}
