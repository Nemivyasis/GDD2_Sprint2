using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTracker : MonoBehaviour {

    public int level;

    public int Level {
        get { return level; }
        set { level = value; }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        level = 2;
    }
}
