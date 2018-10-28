/** Jonathan So, jds7523@rit.edu
 * The laser beam, a child of the laser, has a color, direction, etc. However, the beam itself checks for collisions.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Beam : MonoBehaviour {

    private Beam prevBeam;
    private Laser.LaserColor color;

    public List<GameObject> ignoreObjects;
    public GameObject fakeLaser;

    public Vector3 direction;

    private Vector3 endPoint;

    public GameObject nextLaser = null;
    public Laser nextLaserScript = null;

    private GameObject mirrorToReflectOn;

    private float safeGuardTimer = .05f;
    private float startTime = 0;
    // Ensures that the rigidbody attached to this beam is kinematic.
    private void Awake() {
        endPoint = new Vector3(-1000, -1000, -1000);
        GetComponent<Rigidbody>().isKinematic = true;

        startTime = Time.time;
    }

    private void Update()
    {
        if(Time.time - startTime > safeGuardTimer)
        {
            if(prevBeam == null && gameObject.transform.parent.gameObject.tag != "LaserBody")
            {
                DestroyNextLasers();
                Destroy(transform.parent.gameObject);
            }
        }
    }
    //finds where the laser will end
    public void CalcEnd()
    {
        RaycastHit endObj;

        //finds the point at the far back of the laser
        Vector3 toRayCastPoint = new Vector3(-direction.x * .5f * transform.localScale.x, -direction.y * .5f * transform.localScale.x, 0);

        //raycast
        bool hitObj = Physics.Raycast(new Ray(transform.position + toRayCastPoint + direction * .01f, direction), out endObj, float.MaxValue, 9);

        //what will it hit
        if (hitObj)
        {
            Debug.Log(endObj.transform.tag);
            //if it is not hitting a mirror, make sure it does not have any lasers coming from it
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
                if (endObj.transform.gameObject.GetComponent<Prism>().color.ToString() == gameObject.GetComponentInParent<Laser>().color.ToString())
                {
                    Vector3 topMirror = endObj.transform.gameObject.GetComponent<Prism>().GetGlobalTopCoord();
                    Vector3 botMirror = endObj.transform.gameObject.GetComponent<Prism>().GetGlobalBotCoord();
                    //if it misses the glass part, destroy any following lasers (if they exist) and leave
                    if ((endObj.point.x > topMirror.x && endObj.point.x > botMirror.x) || (endObj.point.x < topMirror.x && endObj.point.x < botMirror.x))
                    {
                        DestroyNextLasers();
                        return;
                    }
                    if ((endObj.point.y > topMirror.y && endObj.point.y > botMirror.y) || (endObj.point.y < topMirror.y && endObj.point.y < botMirror.y))
                    {
                        DestroyNextLasers();
                        return;
                    }
                    Debug.Log(color.ToString());
                    endObj.transform.gameObject.GetComponent<Prism>().activated = true;
                }

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
            else if (endObj.transform.gameObject.tag.Equals("MirrorBody"))
            {
                DrawLaser(endObj, transform.position + toRayCastPoint);
            }
        }

        //if it does have a laser after it, calculate that laser
        if(nextLaser != null)
        {
            nextLaserScript.CalcLaser();
        }
    }

    private void DrawLaser(RaycastHit endObj, Vector3 rayCastPos)
    {
        //x size and y size of the line
        float x = endObj.point.x - rayCastPos.x;
        float y = endObj.point.y - rayCastPos.y;

        //calculate the length of the line
        transform.localScale = new Vector3(Mathf.Abs(Vector3.Magnitude(new Vector3(x, y, 0))), transform.localScale.y, transform.localScale.z);

        //reset position
        transform.position = transform.parent.position;

        //calculate angle, if going up and down, make it 90 or 270 depending (avoids divide by 0 errors)
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
        else if (direction.x < 0) // if x is less than 0 (its going left) add 180 to the calculated angle
        {
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * (float) Math.Atan(direction.y / direction.x) + 180);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * (float) Math.Atan(direction.y / direction.x));
        }

        
        //move the line so the position is in the middle of its own path
        transform.Translate(new Vector3(Vector3.Magnitude(new Vector3(x, y, 0)) / 2, 0, 0));
    }

    private void Reflect(RaycastHit endObj)
    {
        //get the mirror script
        Mirror mirror = endObj.transform.gameObject.GetComponent<Mirror>();

        //check if it hits the right part
        mirror.CalculateProperties();
        Vector3 topMirror = mirror.GetGlobalTopCoord();
        Vector3 botMirror = mirror.GetGlobalBotCoord();

        //Debug.Log(topMirror + " " + botMirror + " " + endObj.point);
        //if it misses the glass part, destroy any following lasers (if they exist) and leave
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

            //set some values
            nextLaser.GetComponentInChildren<Beam>().prevBeam = this;

            nextLaserScript = nextLaser.GetComponent<Laser>();
            nextLaserScript.color = transform.parent.gameObject.GetComponent<Laser>().color;
            nextLaser.GetComponentInChildren<Beam>().fakeLaser = fakeLaser;

            nextLaserScript.OnAwake();
        }
        //update nextlaser position
        nextLaser.transform.position = endObj.point;
        
        //the direction
        float lightAngle = transform.eulerAngles.z;

        //make sure all the angles are within 360 and 0
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

        //find the angle of incidence 
        float incidentAngle = mirrorAngle - (180 + lightAngle);

        //calculate the new angle by adding the angle of reflection to the mirror's rotation
        float newAngle = mirrorAngle + incidentAngle;

        //calculate the direction based on newAngle
        nextLaser.GetComponentInChildren<Beam>().direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * newAngle), Mathf.Sin(Mathf.Deg2Rad * newAngle), 0);
    }

    private void DestroyNextLasers()
    {
        //do nothing if there is no next laser
        if(nextLaser != null)
        {
            List<GameObject> toDestroy = new List<GameObject>();
            toDestroy.Add(nextLaser);

            //destroy any lasers following this one
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
}
