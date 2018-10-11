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

    private GameObject mirrorToReflectOn;
	// Ensures that the rigidbody attached to this beam is kinematic.
	private void Awake() {
		GetComponent<Rigidbody>().isKinematic = true;
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            mirrorCollision = false;
            mirrorTimer = 0;
            delayedCollision = false;
        }

        if (delayedCollision)
        {
            if(mirrorTimer >= mirrorStopTime)
            {
                delayedCollision = false;

                mirrorTimer = 0;
                hasMadeContact = true;

                if (mirrorCollision)
                {
                    mirrorCollision = false;
                    Reflect(mirrorToReflectOn);
                }


                return;
            }

            mirrorTimer += Time.deltaTime;
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

    private void Reflect(GameObject mirror)
    {
        int mirrorDir = mirror.GetComponentInParent<Mirror>().GetTheta();
        Laser parentScript = GetComponentInParent<Laser>();

        Vector3 contactPos;

        switch (parentScript.dir)
        {
            case Laser.Direction.Up:
                contactPos = new Vector3(transform.position.x, transform.position.y + transform.localScale.y / 2);

                if (mirrorDir == 45)
                {
                    GenerateNewLaser(contactPos, parentScript.color, Laser.Direction.Left, mirror);
                }
                if (mirrorDir == 135)
                {
                    GenerateNewLaser(contactPos, parentScript.color, Laser.Direction.Right, mirror);
                }
                break;
            case Laser.Direction.Down:
                contactPos = new Vector3(transform.position.x, transform.position.y + transform.localScale.y / 2);

                if (mirrorDir == 225)
                {
                    GenerateNewLaser(contactPos, parentScript.color, Laser.Direction.Right, mirror);
                }
                if (mirrorDir == 315)
                {
                    GenerateNewLaser(contactPos, parentScript.color, Laser.Direction.Left, mirror);
                }
                break;
            case Laser.Direction.Left:
                contactPos = new Vector3(transform.position.x + transform.localScale.x / 2, transform.position.y);

                if (mirrorDir == 135)
                {
                    GenerateNewLaser(contactPos, parentScript.color, Laser.Direction.Down, mirror);
                }
                if (mirrorDir == 225)
                {
                    GenerateNewLaser(contactPos, parentScript.color, Laser.Direction.Up, mirror);
                }
                break;
            case Laser.Direction.Right:
                
                contactPos = new Vector3(transform.position.x + transform.localScale.x / 2, transform.position.y);

                if (mirrorDir == 45)
                {
                    GenerateNewLaser(contactPos, parentScript.color, Laser.Direction.Down, mirror);
                }
                if (mirrorDir == 315)
                {
                    GenerateNewLaser(contactPos, parentScript.color, Laser.Direction.Up, mirror);
                }
                break;
            case Laser.Direction.None:
                break;
            default:
                break;
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
