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

	private GameObject beam; // The gameObject child which represents the laser beam.
	private BoxCollider beamColl; 
	private Vector3 transDir; // The direction at which the laser beam expands.

	private const float BEAM_SCALE = 1/4f; // The beam's scale will always be 1/4th of the laser.

	/** Grab a reference to the child object, the laser beam. We can then access the 
	 * beam's BoxCollider. Then, based on this laser's direction, set the center of its 
	 * collider and its color.
	 */
	private void Awake() {
		beam = this.gameObject.transform.GetChild(0).gameObject;
		beamColl = beam.GetComponent<BoxCollider>();
		ChooseColor();
	}

	/** Based on the four possible directions, choose a direction for this laser.
	 * Every frame, expand the laser beam based on time and translate it to give the illusion that it
	 * is properly shooting out of the cube.
	 */
	private void ChooseDirection() {
		if (dir.Equals(Direction.Up)) {
			beam.transform.localScale = new Vector3(BEAM_SCALE, BEAM_SCALE, Time.time);
			beam.transform.Translate(Vector3.forward * (Time.deltaTime / 2 ));
		} else if (dir.Equals(Direction.Down)) {
			beam.transform.localScale = new Vector3(BEAM_SCALE, BEAM_SCALE, -Time.time);
			beam.transform.Translate(Vector3.forward * (-Time.deltaTime / 2) );
		} else if (dir.Equals(Direction.Left)) {
			beam.transform.localScale = new Vector3(-Time.time, BEAM_SCALE, BEAM_SCALE);
			beam.transform.Translate(Vector3.right * (-Time.deltaTime / 2) );
		} else if (dir.Equals(Direction.Right)) {
			beam.transform.localScale = new Vector3(Time.time, BEAM_SCALE, BEAM_SCALE);
			beam.transform.Translate(Vector3.right * (Time.deltaTime / 2) );
		} else { // No direction.
			beamColl.center = new Vector3(0, 0, 0);
		}
	}

	/** Based on the seven possible colors, choose a color for this laser.
	 * Set the color of the material of the laser beam.
	 */
	private void ChooseColor() {
		if (color == LaserColor.Red) {
			beam.GetComponent<Renderer>().material.color = Color.red;
		} else if (color == LaserColor.Blue) {
			beam.GetComponent<Renderer>().material.color = Color.blue;
		} else if (color == LaserColor.Yellow) {
			beam.GetComponent<Renderer>().material.color = Color.yellow;
		} else if (color == LaserColor.Purple) {
			beam.GetComponent<Renderer>().material.color = new Color(1.0f, 0.0f, 1.0f);
		} else if (color == LaserColor.Green) {
			beam.GetComponent<Renderer>().material.color = Color.green;
		} else if (color == LaserColor.Orange) {
			beam.GetComponent<Renderer>().material.color = new Color(1.0f, 0.5f, 0.0f);
		} else if (color == LaserColor.White) {
			beam.GetComponent<Renderer>().material.color = Color.white;
		}
			

	}

	void Start () {
		
	}

	// Every frame, shoot the laser outwards.
	private void Update () {
		ChooseDirection();
	}
}
