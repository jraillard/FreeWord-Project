using UnityEngine;
using UnityEngine.UI;

public class CL_LanguageToLearn : MonoBehaviour
{
    public GameObject back;
    private Data data;
    private GameObject title;
    private GameObject infos;

    private string Username;
    private string[] Lines;
    private string[] Line;

    // Use this for initialization
    void Start()
    {
        data = GameObject.Find("DataObject").GetComponent<Data>();
        title = GameObject.Find("Title");
        infos = GameObject.Find("Infos");
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
        back.GetComponent<Text>().text = "Back";
        title.GetComponent<Text>().text = " Which language do you want to pratice ? ";
        infos.GetComponent<Text>().text = " Check the achievements before playing ";
    }

    private void CL_Français()
    {
        back.GetComponent<Text>().text = "Retour";
        title.GetComponent<Text>().text = " Dans quelle langue voulez-vous vous exercer ? ";
        infos.GetComponent<Text>().text = " Allez dans réalisations avant de jouer ";
    }
}
