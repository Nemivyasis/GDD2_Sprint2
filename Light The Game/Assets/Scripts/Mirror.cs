using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour {

    //made public so it can be seen in the editor
    //position of the mirror
    public Vector3 pos;

    //rotation of the mirror
    public Vector3 theta;

	// Use this for initialization
	/*void Start () {
        pos.x = this.transform.position.x;
        pos.y = this.transform.position.y;
        pos.z = 0;
	}*/

    //for retrieving the position
    public Vector3 GetPos()
    {
        return pos;
    }

    //for retrieving the rotation
    public Vector3 GetRotation()
    {
        return theta;
    }

    //for placing the mirror
    public void Placer()
    {
        //get the mouse pos
        Vector3 mousePos = new Vector3(Input.mousePosition.x, 0, Input.mousePosition.z);
        print(mousePos);

        //translate the mouse pos to the world
        pos = Camera.main.ScreenToWorldPoint(mousePos);

        //move the mirror
        this.transform.position = pos;
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
