using UnityEngine;
using System.Collections;

public class KodakMoment : MonoBehaviour {

	public GameManager theManager;

	public int snapped = -1;
	public float distance;
	public Vector3 rotation;
	public bool isDoingSpecial = false;
	bool caughtBeingSpecial = false;

	void OnBecameVisible() {
		theManager.kodakMoments.Add (this);
		theManager.photoMoments.Add (gameObject.GetComponent<BoxCollider> ());
	}

	void OnBecameInvisible() {
		for (int a = 0; a < theManager.kodakMoments.Count; a++) {
			if (theManager.kodakMoments[a] == this) theManager.kodakMoments.RemoveAt(a);
		}

		for (int b = 0; b < theManager.photoMoments.Count; b++) {
			if (theManager.photoMoments[b] == gameObject.GetComponent<BoxCollider> ()) theManager.photoMoments.RemoveAt(b);
		}
	}

	public bool checkDistance () {
		print ("Distance is " + Vector3.Distance (theManager.thePlayer.transform.position, transform.position));
		if (distance == -1) return true;

		if (Vector3.Distance (theManager.thePlayer.transform.position, transform.position) < distance) {
			distance = -1;
			return true;
		} else return false;
	}

	public bool checkRotation () {
		print ("Rotation distance is " + Vector3.Distance (transform.localEulerAngles, rotation));
		if (rotation == -Vector3.one) return true;

		if (Vector3.Distance (transform.localEulerAngles, rotation) < 1) {
			rotation = -Vector3.one;
			return true;
		} else return false;
	}

	public bool checkisDoingSpecial () {
		print ("Being Special is " + isDoingSpecial);
		if (caughtBeingSpecial) return true;

		if (isDoingSpecial) {
			caughtBeingSpecial = true;
			return true;
		} else return false;

	}

}
