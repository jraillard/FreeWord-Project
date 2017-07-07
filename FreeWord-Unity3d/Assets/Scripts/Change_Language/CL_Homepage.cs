using UnityEngine;
using UnityEngine.UI;

public class CL_Homepage : MonoBehaviour
{
    public GameObject play;
    public GameObject tutorial;
    public GameObject achievements;
    public GameObject options;
    public GameObject back;
    public GameObject hello;
    public GameObject username;
    private Data data;

    private string[] Lines;
    private string[] Line;

    // Use this for initialization
    void Start()
    {
        data = GameObject.Find("DataObject").GetComponent<Data>();
        data.SetLanguageToPlayAndUsrName();
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
        play.GetComponent<Text>().text = "Play";
        tutorial.GetComponent<Text>().text = "Tutorial";
        achievements.GetComponent<Text>().text = "Achievements";
        back.GetComponent<Text>().text = "Change language";
        hello.GetComponent<Text>().text = "Hello :";
        username.GetComponent<Text>().text = data.Username;
    }

    private void CL_Français()
    {
        play.GetComponent<Text>().text = "Jouer";
        tutorial.GetComponent<Text>().text = "Tutoriel";
        achievements.GetComponent<Text>().text = "Réalisations";
        back.GetComponent<Text>().text = "Changer la langue";
        hello.GetComponent<Text>().text = "Bien le bonjour :";
        username.GetComponent<Text>().text = data.Username;
    }
}
