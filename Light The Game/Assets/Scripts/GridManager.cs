using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour {
    GameObject[] prisms;
    bool victory = false;
    public Text victoryText;

	// Use this for initialization
	void Start () {
        //find lasers here
        prisms = GameObject.FindGameObjectsWithTag("Prism");
	}
	
	// Update is called once per frame
	void Update () {
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
                victory = true;
                victoryText.gameObject.SetActive(true);
            }
        }

        //reset
        if (Input.GetKeyDown(KeyCode.R))
        {
            victory = false;
            victoryText.gameObject.SetActive(false);
        }
	}
}
