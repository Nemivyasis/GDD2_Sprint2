/** Jonathan So, jds7523@rit.edu
 * The laser beam, a child of the laser, has a color, direction, etc. However, the beam itself checks for collisions.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Beam : MonoBehaviour {

	private Laser.LaserColor color;
	private bool hasMadeContact = false; // Whether or not this beam has hit something.

	// Ensures that the rigidbody attached to this beam is kinematic.
	private void Awake() {
		GetComponent<Rigidbody>().isKinematic = true;
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
	private void OnCollisionEnter(Collision coll) {
		if (coll.gameObject.tag.Equals("Wall")) {
			hasMadeContact = true;
		} else if (coll.gameObject.tag.Equals("Prism")) {
			hasMadeContact = true;
		} else if (coll.gameObject.tag.Equals("Mirror")) {
			hasMadeContact = true;
		}
	}


}
