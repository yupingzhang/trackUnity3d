using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class photRating : MonoBehaviour {

	public List<TweenScale> content = new List<TweenScale>();
	public GameObject StarFull;

	public void TweenIn () {
		TweenIt (true);
	}

	public void TweenOut () {
		TweenIt (false);
	}

	void TweenIt (bool Forward) {
		if (Forward) {
			if (StarFull) StarFull.GetComponent<TweenScale> ().ResetToBeginning ();
			if (StarFull) StarFull.GetComponent<TweenRotation> ().ResetToBeginning ();
		} else {
			if (StarFull) StarFull.GetComponent<TweenScale> ().PlayReverse ();
			if (StarFull) StarFull.GetComponent<TweenRotation> ().PlayReverse ();
		}
		for (int z = 0; z < content.Count; z++) {
			if (Forward) {
				content[z].ResetToBeginning();
				content[z].PlayForward();
			} else content[z].PlayReverse();
		}
	}

	public void TweenInSuccess () {
		if (StarFull) StarFull.GetComponent<TweenScale> ().PlayForward ();
		if (StarFull) StarFull.GetComponent<TweenRotation> ().PlayForward ();
	}
}
