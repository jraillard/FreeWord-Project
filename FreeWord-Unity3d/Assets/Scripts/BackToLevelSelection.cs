using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class BackToLevelSelection : MonoBehaviour {

    //simple script for the Replay button to load again the scene 

    /********************************* Methods *********************************/

    public void ComeBack()
    {
        SceneManager.LoadSceneAsync("LevelSelection", LoadSceneMode.Single);
    }
}
