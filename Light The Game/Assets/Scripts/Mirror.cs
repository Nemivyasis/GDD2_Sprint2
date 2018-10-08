using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour {

    //made public so it can be seen in the editor
    //position of the mirror
    public int x;
    public int z;
    public Vector3 pos;

    //rotation of the mirror
    public float theta;

	// Use this for initialization
	void Start () {
        x = 0;
        z = 0;
        pos.x = x;
        pos.z = z;
        theta = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //for retrieving the position
    public Vector3 GetPos()
    {
        return pos;
    }

    //for retrieving the rotation
    public float GetRotation()
    {
        return theta;
    }

    //for placing the mirror
    public void Placer()
    {
        //put mouse drag code here
    }

    //for rotating the mirror
    //locked to 90 degrees
    public void Rotate()
    {
        if (Input.GetKeyDown("r"))
        {
            //put rotation code here
        }
    }
}
