using UnityEngine;
using UnityEngine.UI;

public class CL_Exit : MonoBehaviour
{

    public GameObject title;
    public GameObject yes;
    public GameObject no;
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
        title.GetComponent<Text>().text = "You are leaving the game\nAre you sure ? ";
        yes.GetComponent<Text>().text = "Yes";
        no.GetComponent<Text>().text = "No";
    }

    private void CL_Français()
    {
        title.GetComponent<Text>().text = "Vous allez quitter le jeu.\nQuitter ?";
        yes.GetComponent<Text>().text = "Oui";
        no.GetComponent<Text>().text = "Non";
    }
}
