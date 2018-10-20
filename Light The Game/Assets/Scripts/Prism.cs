using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prism : MonoBehaviour {

    public bool activated;
    public enum Direction { Up, Down, Left, Right, None };
    public enum PrismColor { Red, Blue, Yellow, Purple, Green, Orange, White };

    public PrismColor color;
    public Direction openFace;

    Laser contactingLaserScript;
    bool laserContact;

    //vince stuff pls help
    public Vector3 topMirror;
    public Vector3 botMirror;

    // Use this for initialization
    void Start () {
        Vector4 startTop = new Vector4(-transform.localScale.x / 2, transform.localScale.y / 2, 0, 0);
        Vector4 startBot = new Vector4(-transform.localScale.x / 2, -transform.localScale.y / 2, 0, 0);
        float angle = Mathf.Deg2Rad * transform.eulerAngles.z;

        Matrix4x4 rotMat = new Matrix4x4(new Vector4(Mathf.Cos(angle), Mathf.Sin(angle), 0, 0),
            new Vector4(-Mathf.Sin(angle), Mathf.Cos(angle), 0, 0), new Vector4(0, 0, 1, 0),
            new Vector4(0, 0, 0, 0));

        topMirror = rotMat * startTop;
        botMirror = rotMat * startBot;
    }
	
	// Update is called once per frame
	void Update () {
        if (laserContact)
        {
            activated = contactingLaserScript.color.ToString() == color.ToString();
        }

        if (Input.GetKeyDown(KeyCode.R)){
            activated = false;
            laserContact = false;
            contactingLaserScript = null;
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Beam")
        {
            Laser tempContactLaser = other.GetComponentInParent<Laser>();

            bool correctDirection = false;

            switch (openFace)
            {
                case Direction.Up:
                    if (tempContactLaser.dir == Laser.Direction.Down)
                        correctDirection = true;
                    break;
                case Direction.Down:
                    if (tempContactLaser.dir == Laser.Direction.Up)
                        correctDirection = true;
                    break;
                case Direction.Left:
                    if (tempContactLaser.dir == Laser.Direction.Right)
                        correctDirection = true;
                    break;
                case Direction.Right:
                    if (tempContactLaser.dir == Laser.Direction.Left)
                        correctDirection = true;
                    break;
                case Direction.None:
                    break;
                default:
                    break;
            }

            if (correctDirection)
            {
                laserContact = true;
                contactingLaserScript = tempContactLaser;

                activated = contactingLaserScript.color.ToString() == color.ToString();
            }
        }
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
