using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class Replay : MonoBehaviour {
 //simple script for the Replay button to load again the scene 

    public void PlayAgain()
    {
        SceneManager.LoadScene("SceneTest", 0);
    }
}
