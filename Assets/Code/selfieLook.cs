using UnityEngine;
using System.Collections;

public class selfieLook : MonoBehaviour {

	public Transform target;

	void FixedUpdate () {
		transform.LookAt (new Vector3 (target.position.x, transform.position.y, target.position.z));
	}
}
