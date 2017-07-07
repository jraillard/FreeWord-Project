using UnityEngine;
using UnityEngine.UI;

public class CL_Options : MonoBehaviour
{

    public GameObject music;
    public GameObject sound;
    public GameObject back;
    public GameObject devInf;
    private Data data;

    private string Username;
    private string[] Lines;
    private string[] Line;

    // Use this for initialization
    void Start()
    {
        data = GameObject.Find("DataObject").GetComponent<Data>();
        if (data.LanguageToPlay == "English")
        {
            CL_English();
        }
        else
        {
            CL_Français();
        }
    }

    private void CL_English()
    {
        music.GetComponent<Text>().text = "Music";
        sound.GetComponent<Text>().text = "Sound";
        back.GetComponent<Text>().text = "Back";
        devInf.GetComponent<Text>().text = "Developers Informations";
    }

    private void CL_Français()
    {
        music.GetComponent<Text>().text = "Musique";
        sound.GetComponent<Text>().text = "Son";
        back.GetComponent<Text>().text = "Retour";
        devInf.GetComponent<Text>().text = "Informations développeurs";
    }
}
