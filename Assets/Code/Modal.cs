using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Modal : MonoBehaviour {

	public List<GameObject> content = new List<GameObject>(); 

	void Start () {
		//gameObject.GetComponent<UISprite>().enabled = true;
		//openInterfaceChoose();
	}

	public void openPause () {
		turnOffContent();
		content[0].SetActive(true);
		toggleBody(true);
	}

	public void openResults () {
		turnOffContent();
		content[1].SetActive(true);
		toggleBody(true);
	}

	public void closeModal () {
		turnOffContent();
		toggleBody(false);
	}

	void turnOffContent () {
		for (int z = 0; z < content.Count; z++) {
			content[z].SetActive(false);
		}
	}

	void toggleBody (bool State) {
		gameObject.SetActive(State);
	}
}
