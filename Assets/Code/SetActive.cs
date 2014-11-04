using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SetActive : MonoBehaviour {

	public List<GameObject> ToSetActive = new List<GameObject>();

	public List<GameObject> ToSetDeactive = new List<GameObject>();

	public void Set () {
		for (int t = 0; t < ToSetActive.Count; t++ ) {
			ToSetActive[t].SetActive(true);
		}
		for (int a = 0; a < ToSetDeactive.Count; a++ ) {
			ToSetDeactive[a].SetActive(false);
		}
	}

	public void SetReverse () {
		for (int t = 0; t < ToSetActive.Count; t++ ) {
			ToSetActive[t].SetActive(false);
		}
		for (int a = 0; a < ToSetDeactive.Count; a++ ) {
			ToSetDeactive[a].SetActive(true);
		}
	}

	public void SetOnlyThisOn () {
		for (int t = 0; t < ToSetActive.Count; t++ ) {
			ToSetActive[t].SetActive(true);
		}
	}

	public void SetOnlyThisOff () {
		for (int t = 0; t < ToSetActive.Count; t++ ) {
			ToSetActive[t].SetActive(false);
		}
	}
}
