using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Internet_Android : MonoBehaviour {

    private Data data;

    private void Start()
    {
        data = GameObject.Find("DataObject").GetComponent<Data>();
    }

    private void Update()
    {
        if(Application.internetReachability != NetworkReachability.NotReachable)
        {
            data.SetdetectWeb = true;
            data.Setverifload = false;
            SceneManager.UnloadSceneAsync("Internet");
        }
    }

}
