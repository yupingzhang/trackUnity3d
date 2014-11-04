using UnityEngine;
using System.Collections;

public class Tester : MonoBehaviour {

	public mynodescript nodeCode;
	Vector3 playStart = Vector3.zero;
	int currNodeIndex = 0;
	int count;
	// Use this for initialization
	void Start () {
//		iTween.MoveTo (gameObject, iTween.Hash("path", iTweenPath.GetPath("track1"), "time", 10));
		iTween.MoveTo (gameObject, playStart, 10);
		count = nodeCode.nodes.Count;
	}
	
	// Update is called once per frame
	void Update () {
		int index = 0;
		NodeObj temp = nodeCode.myPath[currNodeIndex];
		// if next node has no branch, go to next node
		if(temp.branch) {
			index = temp.left;
 		}
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			index = temp.left;
			print ("left pressed at " + Time.time);
		}
		else if(Input.GetKeyDown (KeyCode.RightArrow)) {
			index = temp.right;
			print ("right pressed at " + Time.time);
		} 
		if(index < count) {
			currNodeIndex = index;
		}
	
		iTween.MoveTo (gameObject, nodeCode.nodes[index], 10);
	
	}
}
