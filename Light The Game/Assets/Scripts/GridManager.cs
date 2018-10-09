using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {
    public int x;
    public int y;
    int[,] grid;

    GameObject[] lasers;
    List<Vector3> laserColors;
    List<List<Vector2>> laserPaths;
	// Use this for initialization
	void Start () {
        grid = new int[x, y]; //current management for grid population and stuff

        //find lasers here
        lasers = GameObject.FindGameObjectsWithTag("Laser");
	}
	
	// Update is called once per frame
	void Update () {
        CalculatePaths();
        ClearLasers();
        DrawPaths();
	}

    private void DrawPaths()
    {
        throw new NotImplementedException();
    }

    //hopefully unnecessary but I feel like we may need it
    private void ClearLasers()
    {
        throw new NotImplementedException();
    }

    private void CalculatePaths()
    {
        
    }

    int GetObjectAt(int objX, int objY)
    {
        if(objX <= 0 || objX > x)
        {
            return -1;
        }
        if (objY <= 0 || objY > y)
        {
            return -1;
        }

        return grid[objX, objY];
    }
}
