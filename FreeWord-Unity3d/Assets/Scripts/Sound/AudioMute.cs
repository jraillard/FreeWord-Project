using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Sprites;

public class AudioMute : MonoBehaviour {

    public bool Amute=false;
    public bool TextureMute = false;
    private Sprite OnTexture;
    private Sprite OffTexture;
    private AudioManager theAM;

    
    void Awake()
    {
        theAM = FindObjectOfType<AudioManager>();
        TextureMute = GameObject.Find("AudioManager").GetComponent<AudioManager>().mute;
        if (TextureMute == true)
        {
            GameObject.Find("Music_On").GetComponent<Image>().sprite = Resources.Load("gris", typeof(Sprite)) as Sprite;
            GameObject.Find("Music_Off").GetComponent<Image>().sprite = Resources.Load("blanc", typeof(Sprite)) as Sprite;
        }
        Amute = TextureMute;
    }
    // Use this for initialization
    public void TurnMute()
    {
        if (Amute == false)
        {
            theAM.BGM.Pause();
            Amute = true;
            theAM.SetMute(true);
        }
    }

    public void TurnUnMute()
    {
        if (Amute == true)
        {
            theAM.BGM.Play();
            Amute = false;
            theAM.SetMute(false);
        }
    }
}
