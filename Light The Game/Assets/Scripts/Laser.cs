/** Jonathan So, jds7523@rit.edu
 * The laser is able to shoot its beam in a direction and has its own assigned color.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

	// These enums might be better off in the manager class.
	public enum LaserColor {Red, Blue, Yellow, Purple, Green, Orange, White};
	public enum Direction {Up, Down, Left, Right, None};

	public LaserColor color; 
	public Direction dir;

	private Beam beam; // The child which represents the laser beam.
	private BoxCollider beamColl; 
	private Vector3 transDir; // The direction at which the laser beam expands.

	private float laserTimer = 0f; // The laser beam scales by time. Used instead of Time.time so we can reset it as we wish.
	public bool isLaserOn = false; // By default, the lasers are off until the player says to shoot them (via input).

	private const float BEAM_SCALE = 1/16f; // The beam's scale will always be 1/4th of the laser.
	private const float LASER_SPD = 1/20f; // The speed at which the laser shoots, as an alternative to Time.deltaTime.

	/** Grab a reference to the child object, the laser beam. We can then access the 
	 * beam's BoxCollider. Then, based on this laser's direction, set the center of its 
	 * collider and its color.
	 */
	private void Awake() {
        OnAwake();
	}

    public void OnAwake()
    {
        beam = this.gameObject.transform.GetChild(0).GetComponent<Beam>();
        beamColl = beam.GetComponent<BoxCollider>();
        ChooseColor();
        ChooseDirection();

    }
	/** Based on the four possible directions, choose a direction for this laser.
	 * Every frame, expand the laser beam based on time and translate it to give the illusion that it
	 * is properly shooting out of the cube.
	 * 
	 * If the lasers are off or the beam's made contact with something, then skip expanding the lasers and updating time entirely.
	 */
	private void ChooseDirection() {

        if (dir.Equals(Direction.Up))
        {
            beam.transform.localScale = new Vector3(BEAM_SCALE, BEAM_SCALE, BEAM_SCALE);
            beam.direction = Vector3.up;
        }
        else if (dir.Equals(Direction.Down))
        {
            beam.transform.localScale = new Vector3(BEAM_SCALE, BEAM_SCALE, BEAM_SCALE);
            beam.direction = -Vector3.up;
        }
        else if (dir.Equals(Direction.Left))
        {
            beam.transform.localScale = new Vector3(BEAM_SCALE, BEAM_SCALE, BEAM_SCALE);
            beam.direction = -Vector3.right;
        }
        else if (dir.Equals(Direction.Right))
        {
            beam.transform.localScale = new Vector3(BEAM_SCALE, BEAM_SCALE, BEAM_SCALE);
            beam.direction = Vector3.right;
        }
        else
        { // No direction.
            beam.transform.localScale = new Vector3(BEAM_SCALE, BEAM_SCALE, BEAM_SCALE);
        }

        laserTimer += LASER_SPD;
	}

	/** Based on the seven possible colors, choose a color for this laser.
	 * Set the color of the material of the laser beam.
	 * Also set the beam's color internally.
	 */
	private void ChooseColor() {
		if (color == LaserColor.Red) {
			beam.GetComponent<Renderer>().material.color = Color.red;
			if(GetComponent<Renderer>() != null)
            {
                GetComponent<Renderer>().material.color = Color.red;
            }
		} else if (color == LaserColor.Blue) {
			beam.GetComponent<Renderer>().material.color = Color.blue;
            if (GetComponent<Renderer>() != null)
            {
                GetComponent<Renderer>().material.color = Color.blue;
            }
        } else if (color == LaserColor.Yellow) {
			beam.GetComponent<Renderer>().material.color = Color.yellow;
            if (GetComponent<Renderer>() != null)
            {
                GetComponent<Renderer>().material.color = Color.yellow;
            }
        } else if (color == LaserColor.Purple) {
			beam.GetComponent<Renderer>().material.color = new Color(1.0f, 0.0f, 1.0f);
            if (GetComponent<Renderer>() != null)
            {
                GetComponent<Renderer>().material.color = new Color(1.0f, 0.0f, 1.0f);
            }
        } else if (color == LaserColor.Green) {
			beam.GetComponent<Renderer>().material.color = Color.green;
            if (GetComponent<Renderer>() != null)
            {
                GetComponent<Renderer>().material.color = Color.green;
            }
        } else if (color == LaserColor.Orange) {
			beam.GetComponent<Renderer>().material.color = new Color(1.0f, 0.5f, 0.0f);
            if (GetComponent<Renderer>() != null)
            {
                GetComponent<Renderer>().material.color = new Color(1.0f, 0.5f, 0.0f);
            }
        } else if (color == LaserColor.White) {
			beam.GetComponent<Renderer>().material.color = Color.white;
            if (GetComponent<Renderer>() != null)
            {
                GetComponent<Renderer>().material.color = Color.white;
            }
        }
		beam.SetColor(color);

	}

	/** Handles shooting the laser 'S' and restarting the puzzle 'R'.
	 * 'R': Upon restarting the puzzle, reset the timer and the isLaserOn boolean, and reset the
	 * beam's position and scale.
	 * 'S': When shooting the laser, simply activate the isLaserOn boolean to true.
	 */
	private void ProcessInput() {

        if (Input.GetKeyDown(KeyCode.R)) { // 'R' = *R*estart the puzzle
			laserTimer = 0f;
			isLaserOn = false;
			beam.transform.localScale = new Vector3(BEAM_SCALE, BEAM_SCALE, BEAM_SCALE);
			beam.transform.localPosition = Vector3.zero;
		} else if (Input.GetKeyDown(KeyCode.S)) { // 'S' = *S*hoot the laser
			isLaserOn = true;
        }
	}

	// Every frame, shoot the laser outwards.
	private void Update () {
		//ChooseDirection();
		//ProcessInput();
	}

    public void CalcLaser()
    {
        beam.CalcEnd();
    }

    public void ActivateLaser(bool active)
    {
        isLaserOn = active;
    }
}

