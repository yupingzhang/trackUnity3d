using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

 
public class mynodescript : MonoBehaviour {

	public List<Vector3> nodes = new List<Vector3>(){Vector3.zero, Vector3.zero};   // movable points
	public List<NodeObj> myPath = new List<NodeObj>(){};    // decide the path and branch by set the node index in the path
	public int nodeCount = 2;
	public bool pathVisible = true;

//	public mynodescript() {
//	}

	// Use this for initialization
	void Start () {
		NodeObj first = new NodeObj (false, 1, 0);
		NodeObj second = new NodeObj (false, -1, 0);
		myPath.Add (first);
		myPath.Add (second);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDrawGizmosSelected(){
		if(pathVisible){
			if(nodes.Count > 0){
				iTween.DrawPath(nodes.ToArray(), Color.cyan);
			}	
		}
	}

}
