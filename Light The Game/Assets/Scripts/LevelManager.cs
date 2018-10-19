using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    //for calling scenes from buttons
    public void LoadLevel(int sceneIndex)
    {
        print("Button clicked");

        SceneManager.LoadScene(sceneIndex);
    }
}
