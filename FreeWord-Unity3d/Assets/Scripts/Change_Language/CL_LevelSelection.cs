using UnityEngine;
using UnityEngine.UI;

public class CL_LevelSelection : MonoBehaviour
{
    public GameObject title;
    public GameObject play;
    public GameObject back;
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
        title.GetComponent<Text>().text = " Which category do you want to pratice ?";
        back.GetComponent<Text>().text = "Back";
        play.GetComponent<Text>().text = "Play";
    }

    private void CL_Français()
    {
        title.GetComponent<Text>().text = "Dans quelle catégorie voulez-vous vous exercer ?";
        back.GetComponent<Text>().text = "Retour";
        play.GetComponent<Text>().text = "Jouer";
    }
}
