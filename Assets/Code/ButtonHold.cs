using UnityEngine;
using System.Collections;

public class ButtonHold : MonoBehaviour {

	public Player thePlayer;

	void OnPress (bool isDown) {
		if (!isDown) {
			this.enabled = false;
			thePlayer.downTime = 0.0f;
			thePlayer.Shoot();
		} else this.enabled = true;
	}

	void Update () {
		thePlayer.CheckSelfieMode ();
	}
}
