using UnityEngine;
using System.Collections;

public class Tester : MonoBehaviour {

	public mynodescript nodeCode;
	public float interval;
	int index = 0;
	 
	// Use this for initialization
	void Start () {

		interval = 1;

//		iTween.MoveTo (gameObject, iTween.Hash("path", iTweenPath.GetPath("track1"), "time", 10));
		iTween.MoveTo (gameObject, nodeCode.nodes[0], 1);
		Invoke ("NextNode", interval);
		 
	}

	void NextNode () {
		print ("Index: " + index);
		NodeObj temp = nodeCode.myPath[index];
		if (temp.left == -1)
			return;         // stop at the end
		// if next node has no branch, go to next node
		if(!temp.branch) {     
			print ("go to next");
			index = temp.left;
		}
		else if (Input.GetKey (KeyCode.LeftArrow)) {
			index = temp.left;
			print ("turn left ");
		}
		else if(Input.GetKey (KeyCode.RightArrow)) { 
			index = temp.right;
			print ("turn right ");
		} 

		iTween.MoveTo (gameObject, nodeCode.nodes[index], interval);
		Invoke ("NextNode", interval);
	}
	
	// Update is called once per frame
	void Update () {
//		Invoke ("NextNode", interval);
		
	}
}
