using UnityEngine;
using System.Collections;

public class LoadScene : MonoBehaviour {

	public Settings settinghack;

	public void load_google_cardboard () {
		PlayerPrefs.SetInt ("googlecardboard", 1);
		Application.LoadLevel ("Game");
	}

	public void load_non_google_cardboard () {
		PlayerPrefs.SetInt ("googlecardboard", 0);
		Application.LoadLevel ("Game");
	}

	public void Hack_set_GoogleCardboardMagnet () {
		settinghack.set_GoogleCardboardMagnet ();
	}
}
