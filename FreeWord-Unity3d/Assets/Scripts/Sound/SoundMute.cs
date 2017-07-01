using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Sprites;

public class SoundMute : MonoBehaviour
{

    public bool Smute = false;
    public bool TextureMute = false;
    private Sprite OnTexture;
    private Sprite OffTexture;
    private SoundManager theSM;
    private PlaySound PSF;


    void Awake()
    {
        theSM = FindObjectOfType<SoundManager>();
        PSF = FindObjectOfType<PlaySound>();
        TextureMute = GameObject.Find("SoundManager").GetComponent<SoundManager>().mute;
        if (TextureMute == true)
        {
            GameObject.Find("Sound_On").GetComponent<Image>().sprite = Resources.Load("gris", typeof(Sprite)) as Sprite;
            GameObject.Find("Sound_Off").GetComponent<Image>().sprite = Resources.Load("blanc", typeof(Sprite)) as Sprite;
        }
        Smute = TextureMute;
    }
    // Use this for initialization
    public void TurnMute()
    {
        if (Smute == false)
        {
            Smute = true;
            theSM.SetMute(true);
            PSF.SetMute(true);
        }
    }

    public void TurnUnMute()
    {
        if (Smute == true)
        {
            Smute = false;
            theSM.SetMute(false);
            PSF.SetMute(false);
        }
    }
}
