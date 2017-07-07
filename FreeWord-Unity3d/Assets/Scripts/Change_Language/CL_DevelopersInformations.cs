using UnityEngine;
using UnityEngine.UI;

public class CL_DevelopersInformations : MonoBehaviour {
    
    public GameObject title;
    public GameObject back;
    private Data data;

    private string Username;
    private string[] Lines;
    private string[] Line;

	// Use this for initialization
	void Start () {
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
        title.GetComponent<Text>().text = "They are beautifull ! <3";
        back.GetComponent<Text>().text = "Back";
    }

    private void CL_Français()
    {
        title.GetComponent<Text>().text = "Ils sont beaux ! <3";
        back.GetComponent<Text>().text = "Retour";
    }
}
