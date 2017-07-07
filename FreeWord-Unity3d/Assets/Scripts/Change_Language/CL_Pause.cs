using UnityEngine;
using UnityEngine.UI;

public class CL_Pause : MonoBehaviour
{
    public GameObject levelSelection;
    public GameObject resume;
    public GameObject infos;
    public GameObject music;
    public GameObject sound;
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
        levelSelection.GetComponent<Text>().text = "Level Selection";
        resume.GetComponent<Text>().text = "Resume";
        infos.GetComponent<Text>().text = "TIPS\n\nBonus Word : Reveal several letters at the good place, filling area have to be empty.\n\nBonus Replay : Replace the letters in their original place.";
        music.GetComponent<Text>().text = "Music";
        sound.GetComponent<Text>().text = "Sound";
    }

    private void CL_Français()
    {
        levelSelection.GetComponent<Text>().text = "Choix niveau";
        resume.GetComponent<Text>().text = "Retour";
        infos.GetComponent<Text>().text = "CONSEILS\n\nBonus Mot : Révèle plusieurs lettres à la bonne place, la zone de création doit être vide.\n\n Bonus Replace : Replace les lettres à leur place originale.";
        music.GetComponent<Text>().text = "Musique";
        sound.GetComponent<Text>().text = "Son";
    }
}
