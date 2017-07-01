using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour {

    private SoundManager theSM;
    public bool Smute;

    // Use this for initialization
    public void SoundEffect () {
        theSM = FindObjectOfType<SoundManager>();
        if (Smute == false)
        {
        theSM.PlaySound();
        }
    }
    public void SetMute(bool b)
    {
        Smute = b;
    }
}
