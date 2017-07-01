using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioFadeOut {

    public static IEnumerator FadeOut(AudioSource music,AudioClip newmusic, float FadeTime)
    {
        float startVolume = music.volume;
        if (newmusic.name != "john cena")
        {
            while (music.volume > 0)
            {
                music.volume -= startVolume * Time.deltaTime / FadeTime;
                yield return null;
            }
        }
            music.Stop();
            music.clip = newmusic;
        if (newmusic.name != "john cena")
        {

            while (music.volume < 1)
            {
                music.volume += startVolume * Time.deltaTime / FadeTime;
                yield return null;
            }
            music.Play();
        }
        music.volume = startVolume;
        music.Play();
        

    }
}
