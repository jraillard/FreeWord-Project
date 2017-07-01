using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class AudioManager : MonoBehaviour {

    public AudioSource BGM;
    public bool mute=false;

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(gameObject);

    }

    public void ChangeBGM(AudioClip music)
    {
        if (BGM.clip != music && mute==false) {
            StartCoroutine(AudioFadeOut.FadeOut(BGM,music,0.5f));
        }
        if (mute == true) { BGM.clip = music; }
    }

    public void SetMute(bool b)
    {
        mute = b;
    }
}
