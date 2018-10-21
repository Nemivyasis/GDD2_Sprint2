using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTracker : MonoBehaviour {

    public int level = 2;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public int GetNextLevel()
    {
        return level++;
    }
}
