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

	[SerializeField]
	private GameObject beam; // The gameObject child which represents the laser beam.
	private BoxCollider beamColl; 

	private const float BEAM_SCALE = 1/4f; // The beam's scale will always be 1/4th of the laser.

	/** Grab a reference to the child object, the laser beam. We can then access the 
	 * beam's BoxCollider. Then, based on this laser's direction, set the center of its 
	 * collider.
	 */
	private void Awake() {
		beam = Transform.FindObjectOfType<GameObject>(); 
		beamColl = beam.GetComponent<BoxCollider>();
		// Set center of laser beam's collider.
		if (dir.Equals(Direction.Up)) {
			beamColl.center = new Vector3(0, 0, -BEAM_SCALE);
		} else if (dir.Equals(Direction.Down)) {
			beamColl.center = new Vector3(0, 0, BEAM_SCALE);
		} else if (dir.Equals(Direction.Left)) {
			beamColl.center = new Vector3(BEAM_SCALE, 0, 0);
		} else if (dir.Equals(Direction.Right)) {
			beamColl.center = new Vector3(-BEAM_SCALE, 0, 0);
		} else { // No direction.
			beamColl.center = new Vector3(0, 0, 0);
		}
	}

	void Start () {
		
	}

	void Update () {
		
	}
}
