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


    //vince stuff pls help
    public Vector3 topMirror;
    public Vector3 botMirror;

    bool upToDate = false;

    public bool isLocked = false;

	// Use this for initialization
	void Start () {
        pos = new Vector3(0.0f, 0.0f, 0.0f);
        theta = 0;
        speed = 55.0f;
	}

    //when mouse hovers over mirror, that mirror can rotate
    private void OnMouseOver()
    {
        if (!isLocked)
            Rotate();
    }

    //for placing the mirror
    void OnMouseDrag()
    {
        if (!isLocked)
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
    }

    //for rotating the mirror
    //locked to 90 degrees
    public void Rotate()
    {
        if (!isLocked)
        {
            //rotate right
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Rotate(new Vector3(0, 0, 1) * speed * Time.deltaTime, Space.World);
                upToDate = false;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Rotate(new Vector3(0, 0, -1) * speed * Time.deltaTime, Space.World);
                upToDate = false;
            }
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


    //calculates properties of the mirror edge (is important)
    public void CalculateProperties()
    {
        if (!upToDate)
        {
            CalcMirrorTopAndBotPoints();

            upToDate = true;
        }
    }

    private void CalcMirrorTopAndBotPoints()
    {
        Vector4 startTop = new Vector4(transform.localScale.x / 2, transform.localScale.y / 2, 0, 0);
        Vector4 startBot = new Vector4(transform.localScale.x / 2, -transform.localScale.y / 2, 0, 0);
        float angle = Mathf.Deg2Rad * transform.eulerAngles.z;

        Matrix4x4 rotMat = new Matrix4x4(new Vector4(Mathf.Cos(angle), Mathf.Sin(angle), 0, 0),
            new Vector4(-Mathf.Sin(angle), Mathf.Cos(angle), 0, 0), new Vector4(0, 0, 1, 0),
            new Vector4(0, 0, 0, 0));

        topMirror = rotMat * startTop;
        botMirror = rotMat * startBot;
    }

    public Vector3 GetGlobalTopCoord()
    {
        return topMirror + transform.position;
    }
    public Vector3 GetGlobalBotCoord()
    {
        return botMirror + transform.position;
    }
}
