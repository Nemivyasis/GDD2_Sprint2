using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour {
    //position of the mirror
    Vector3 pos;

    //for getting the angle of rotation
    int theta;

    //speed of rotation
    float speed;

	// Use this for initialization
	void Start () {
        pos = new Vector3(0.0f, 0.0f, 0.0f);
        theta = 0;
        speed = 20.0f;
	}

    //when mouse hovers over mirror, that mirror can rotate
    private void OnMouseOver()
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
        //rotate right
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(new Vector3(0, 0, 1) * speed * Time.deltaTime, Space.World);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(new Vector3(0, 0, -1) * speed * Time.deltaTime, Space.World);
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


    //returns the angle of rotation
    public int GetTheta()
    {
        theta = (int) transform.localRotation.z;
        return theta;
    }
}
