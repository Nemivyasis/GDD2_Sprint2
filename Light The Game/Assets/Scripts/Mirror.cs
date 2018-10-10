using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour {
    //position of the mirror
    Vector3 pos;
    //rotation of the mirror
    Vector3 eulerTheta;
    float theta;

	// Use this for initialization
	void Start () {
        pos = new Vector3(0.0f, 0.0f, 0.0f);
        eulerTheta = new Vector3(0.0f, 0.0f, 0.0f);
        theta = 0;
	}

    private void Update()
    {
        Rotate();
    }

    //for placing the mirror
    void OnMouseDrag()
    {
        //get the mouse pos
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);

        //translate the mouse pos to the world
        pos = Camera.main.ScreenToWorldPoint(mousePos);

        //set the pos' z back so it can be seen by the camera
        pos = new Vector3(pos.x, pos.y, 0);

        //move the mirror
        this.transform.position = pos;
    }

    //for rotating the mirror
    //locked to 90 degrees
    public void Rotate()
    {
        if (Input.GetKeyDown("r"))
        {
            //increment rotation by 45 degrees
            theta += 45.0f;

            //if theta exceeds 360 degrees, set the theta back to 0
            if(theta >= 360.0f)
            {
                theta = 0;
            }

            //set the vector3 to the theta 
            eulerTheta = new Vector3(0.0f, 0.0f, theta);

            //convert to magical quaternion so no gimbal lock
            Quaternion rotation = Quaternion.Euler(eulerTheta);

            //transform the object
            transform.rotation = rotation;
        }
    }

    //set the position of the mirror
    public void SetPos(Vector3 inputPos)
    {
        //make it so whoever uses SetPos cannot change the z
        Vector3 newPos = new Vector3(inputPos.x, inputPos.y, 0.0f);
        transform.position = newPos;

    }

    //for retrieving the position
    public Vector3 GetPos()
    {
        return pos;
    }

    //for retrieving the rotation
    public Vector3 GetRotation()
    {
        return eulerTheta;
    }
}
