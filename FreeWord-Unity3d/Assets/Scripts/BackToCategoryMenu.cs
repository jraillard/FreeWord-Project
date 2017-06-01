using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class BackToCategoryMenu : MonoBehaviour {

 //simple script for the Replay button to load again the scene 

    public void ComeBack()
    {
        //SceneManager.LoadSceneAsync("SceneTest", LoadSceneMode.Single); => doesn't work ???! (even if we don't need it)
        SceneManager.LoadSceneAsync("ChoiceCategoryScene", LoadSceneMode.Single);
    }
}
