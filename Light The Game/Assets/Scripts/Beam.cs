/** Jonathan So, jds7523@rit.edu
 * The laser beam, a child of the laser, has a color, direction, etc. However, the beam itself checks for collisions.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Beam : MonoBehaviour {

	private Laser.LaserColor color;
	public bool hasMadeContact = false; // Whether or not this beam has hit something.

    public List<GameObject> ignoreObjects;
    public GameObject fakeLaser;

    bool delayedCollision = false;
    bool mirrorCollision = false;
    float mirrorTimer = 0;
    public float mirrorStopTime = 0.05f;

    public Vector3 direction;

    private Vector3 endPoint;

    public GameObject nextLaser = null;
    public Laser nextLaserScript = null;

    private GameObject mirrorToReflectOn;
	// Ensures that the rigidbody attached to this beam is kinematic.
	private void Awake() {
        endPoint = new Vector3(-1000, -1000, -1000);
		GetComponent<Rigidbody>().isKinematic = true;
	}

    public void CalcEnd()
    {
        RaycastHit endObj;

        Vector3 toRayCastPoint = new Vector3(-direction.x * .5f * transform.localScale.x, -direction.y * .5f * transform.localScale.x, 0);

        bool hitObj = Physics.Raycast(transform.position + toRayCastPoint + direction * .01f, direction, out endObj);

        if (hitObj)
        {
            if(!endObj.transform.gameObject.tag.Equals("Mirror")){
                DestroyNextLasers();
            }

            if (endObj.transform.gameObject.tag.Equals("Wall"))
            {
                DrawLaser(endObj, transform.position + toRayCastPoint);
            }
            else if (endObj.transform.gameObject.tag.Equals("Prism"))
            {
                DrawLaser(endObj, transform.position + toRayCastPoint);
            }
            else if (endObj.transform.gameObject.tag.Equals("Mirror"))
            {
                DrawLaser(endObj, transform.position + toRayCastPoint);
                Reflect(endObj);
            }
            else if (endObj.transform.gameObject.tag.Equals("LaserBody"))
            {
                DrawLaser(endObj, transform.position + toRayCastPoint);
            }
        }

        if(nextLaser != null)
        {
            nextLaserScript.CalcLaser();
        }
    }

    private void DrawLaser(RaycastHit endObj, Vector3 rayCastPos)
    {
        float x = endObj.point.x - rayCastPos.x;
        float y = endObj.point.y - rayCastPos.y;
        transform.localScale = new Vector3(Mathf.Abs(Vector3.Magnitude(new Vector3(x, y, 0))), transform.localScale.y, transform.localScale.z);

        transform.position = transform.parent.position;

        if (gameObject.tag != "Beam")
        {
            Debug.DrawLine(transform.position, endObj.point);
        }

        if (direction.x == 0)
        {
            if(y > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 270);
            }
        }
        else if (direction.x < 0)
        {
            Debug.Log(" < 0");
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * (float) Math.Atan(direction.y / direction.x) + 180);
        }
        else
        {
            Debug.Log( Math.Tanh(direction.y / direction.x));
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * (float) Math.Atan(direction.y / direction.x));
        }

        
        transform.Translate(new Vector3(Vector3.Magnitude(new Vector3(x, y, 0)) / 2, 0, 0));
    }

    private void Reflect(RaycastHit endObj)
    {
        Mirror mirror = endObj.transform.gameObject.GetComponent<Mirror>();

        //check if it hits the right part
        mirror.CalculateProperties();
        Vector3 topMirror = mirror.GetGlobalTopCoord();
        Vector3 botMirror = mirror.GetGlobalBotCoord();

        if((endObj.point.x > topMirror.x && endObj.point.x > botMirror.x) || (endObj.point.x < topMirror.x && endObj.point.x < botMirror.x))
        {
            DestroyNextLasers();
            return;
        }
        if ((endObj.point.y > topMirror.y && endObj.point.y > botMirror.y ) || (endObj.point.y < topMirror.y && endObj.point.y < botMirror.y))
        {
            DestroyNextLasers();
            return;
        }


        //if it does make a new laser if necessary
        if(nextLaser == null)
        {
            nextLaser = Instantiate(fakeLaser);
            nextLaserScript = nextLaser.GetComponent<Laser>();
            nextLaserScript.color = transform.parent.gameObject.GetComponent<Laser>().color;
            nextLaser.GetComponentInChildren<Beam>().fakeLaser = fakeLaser;

            nextLaserScript.OnAwake();
        }
        //update nextlaser attributes based on angle and such
        nextLaser.transform.position = endObj.point;
        
        //the direction
        float lightAngle = transform.eulerAngles.z;

        while(lightAngle >= 360)
        {
            lightAngle -= 360;
        }
        while (lightAngle < 0)
        {
            lightAngle += 360;
        }

        float mirrorAngle = mirror.transform.eulerAngles.z;
        while (mirrorAngle >= 360)
        {
            mirrorAngle -= 360;
        }
        while(mirrorAngle < 0)
        {
            mirrorAngle += 360;
        }
        float incidentAngle = mirrorAngle - (180 + lightAngle);

        float newAngle = mirrorAngle + incidentAngle;

        nextLaser.GetComponentInChildren<Beam>().direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * newAngle), Mathf.Sin(Mathf.Deg2Rad * newAngle), 0);
    }

    private void DestroyNextLasers()
    {
        if(nextLaser != null)
        {
            List<GameObject> toDestroy = new List<GameObject>();
            toDestroy.Add(nextLaser);

            while (toDestroy[toDestroy.Count - 1].GetComponentInChildren<Beam>().nextLaser != null)
            {
                toDestroy.Add(toDestroy[toDestroy.Count - 1].GetComponentInChildren<Beam>().nextLaser);
            }

            int size = toDestroy.Count;

            for (int i = 0; i < size; i++)
            {
                Destroy(toDestroy[i]);
            }
        }
    }


    // Getter for if the beam has made contact with something.
    public bool GetMadeContact() {
		return hasMadeContact;
	}

	// Setter for if the beam has made contact with something.
	// param[newVal] - bool that'll be the new value of hasMadeContact.
	public void SetMadeContact(bool newVal) {
		hasMadeContact = newVal;
	}

	// Setter for the beam's color.
	// param[newVal] - Laser.LaserColor that'll be the new value of color.
	public void SetColor(Laser.LaserColor newVal) {
		color = newVal;
	}

	// Getter for the beam's color.
	// return - the Laser.LaserColor value of this beam's color.
	public Laser.LaserColor SetColor() {
		return color;
	}

	/** Checks for collisions against walls, prisms, and mirrors.
	 * In the current implementation, all three of these behave the same way: they stop the laser.
	 */
	private void OnTriggerEnter(Collider coll) {
        if (!GetComponentInParent<Laser>().isLaserOn)
        {
            return;
        }

        for (int i = 0; i < ignoreObjects.Count; i++)
        {
            if(coll.gameObject == ignoreObjects[i])
            {
                return;
            }
        }

		if (coll.gameObject.tag.Equals("Wall")) {
			hasMadeContact = true;
		} else if (coll.tag.Equals("Prism")) {
			hasMadeContact = true;
		} else if (coll.tag.Equals("Mirror")) {
			mirrorCollision = true;
            delayedCollision = true;
            mirrorToReflectOn = coll.gameObject;
		} else if (coll.tag.Equals("MirrorBody")) {
            delayedCollision = true;
        }
        else if (coll.tag.Equals("LaserBody"))
        {
            hasMadeContact = true;
        }
    }


    private void GenerateNewLaser(Vector3 startPos, Laser.LaserColor color, Laser.Direction direction, GameObject mirror)
    {
        GameObject newLaser = Instantiate(fakeLaser);

        //set position
        newLaser.transform.position = startPos;

        //set color and direction
        Laser laserScript = newLaser.GetComponent<Laser>();
        laserScript.color = color;
        laserScript.dir = direction;

        //set the ignore object of the beam
        newLaser.GetComponentInChildren<Beam>().ignoreObjects.Add(mirror);
        newLaser.GetComponentInChildren<Beam>().ignoreObjects.Add(mirror.transform.parent.gameObject);
        newLaser.GetComponentInChildren<Beam>().fakeLaser = fakeLaser;

        newLaser.transform.GetChild(0).position = newLaser.transform.position;

        laserScript.OnAwake();

        newLaser.tag = "FakeLaser";


        //activate the laser
        laserScript.ActivateLaser(true);


    }
}
