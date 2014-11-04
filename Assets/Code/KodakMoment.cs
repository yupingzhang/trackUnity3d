using UnityEngine;
using System.Collections;

public class KodakMoment : MonoBehaviour {

	public GameManager theManager;

	public float distance;
	public Vector3 rotation;
	public bool isDoingSpecial = false;
	public int points;


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

	public void checkDistance () {
		if (distance == -1) return;

		if (Vector3.Distance (theManager.thePlayer.transform.position, transform.position) > distance) {
			return;
		} else points += 1;

		distance = -1;
	}

	public void checkRotation () {
		if (rotation == new Vector3 (-1, -1, -1)) return;



	}

	public void checkisDoingSpecial () {

	}
}
