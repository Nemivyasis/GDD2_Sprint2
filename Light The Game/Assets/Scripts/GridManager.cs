using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GridManager : MonoBehaviour {
    GameObject[] prisms;
    bool victory = false;
    public Text victoryText;
    Laser[] lasers;

    public LevelTracker level;

    public Image black;
    public Animator anime;

	private bool isFading = false; // Tells us whether or not we're in the process of fading.

    IEnumerator Fading()
    {
        anime.SetBool("fade", true);
		if (!isFading) { // If this is our first time in the fading process...
			isFading = true;
			Jukebox.instance.FadeSpeakers(); // Fade out the regular piece of level music...
			Jukebox.instance.Victory(); // And play the victory music instead.
		}
        yield return new WaitUntil(() => black.color.a == 1);
        SceneManager.LoadScene(sceneBuildIndex: ++level.Level);
    }

	// Use this for initialization
	void Start () {
        //find lasers here
        GameObject[] temp = GameObject.FindGameObjectsWithTag("LaserBody");

        lasers = new Laser[temp.Length];
        for (int i = 0; i < temp.Length; i++)
        {
            lasers[i] = temp[i].GetComponent<Laser>();
        }

        prisms = GameObject.FindGameObjectsWithTag("Prism");

        level = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelTracker>();
	}

    // Update is called once per frame
    void Update() {
        foreach (var item in prisms)
        {
            item.GetComponent<Prism>().activated = false;
        }
        foreach (var item in lasers)
        {
            item.CalcLaser();
        }
        if (!victory)
        {
            //set an initial boolean
            bool willWin = true;

            for (int i = 0; i < prisms.Length; i++)
            {
                //if any prism is not active, the game is not won, yet
                if (!prisms[i].GetComponent<Prism>().activated)
                {
                    willWin = false;
                    break;
                }
            }


            if (willWin)
            {
                //victory = true;
                //victoryText.gameObject.SetActive(true);
                if (!victory)
                {
                    if(level.level < level.maxLevel)
                    {
                        StartCoroutine("Fading");
                    }
                    else
                    {
                        Debug.Log("Ultimate Victory");
                        victoryText.gameObject.SetActive(true);
                    }
                    victory = true;
                }
            }
        }


        //reset
        if (Input.GetKeyDown(KeyCode.R))
        {
            victory = false;
            victoryText.gameObject.SetActive(false);

            GameObject[] fakeLasers = GameObject.FindGameObjectsWithTag("FakeLaser");

            int size = fakeLasers.Length;
            for (int i = 0; i < size; i++)
            {
                Destroy(fakeLasers[i]);
            }
        }
	}
}
