using UnityEngine;
using System.Collections;

public class OnTriggerEnterCall : MonoBehaviour {

	public string targetTag;
	public GameObject sendMessageTo;
	public string message;

	void OnTriggerEnter(Collider other) {
		if (!other.tag.Contains (targetTag)) return;
		sendMessageTo.SendMessage (message, SendMessageOptions.DontRequireReceiver);
	}
}
