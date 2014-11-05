using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

 
public class mynodescript : MonoBehaviour {

	public List<Vector3> nodes = new List<Vector3>(){Vector3.zero, Vector3.zero};   // movable points
	public List<NodeObj> myPath = new List<NodeObj>(){};    // decide the path and branch by set the node index in the path
	public int nodeCount = 4;
	public bool pathVisible = true;

	public mynodescript() {
	}
	
	// Use this for initialization
	void Start () {
		NodeObj first = new NodeObj (false, 1, 0);
		NodeObj second = new NodeObj (true, 2, 3);
		NodeObj third = new NodeObj (false, -1, 0);
		NodeObj fourth = new NodeObj (false, 4, 0);
		NodeObj fifth = new NodeObj (false, -1, 0);
		myPath.Add (first);
		myPath.Add (second);
		myPath.Add (third);
		myPath.Add (fourth);
		myPath.Add (fifth);
	}
	
	// Update is called once per frame
	void Update () {

	
	}

	void OnDrawGizmosSelected(){
		int curr = 0, next = 1;
		Vector3[] path;
		if(pathVisible){
//			if(nodes.Count > 0){
//				iTween.DrawPath(nodes.ToArray(), Color.cyan);
//			}	
			for(int i=0; i<myPath.Count; i++) {
		
				next = myPath[i].left;
				if(next == -1)  { continue; }     
				path = new Vector3[] {nodes[curr], nodes[next]};   //next /left
				iTween.DrawPath( path, Color.cyan);
				if(myPath[i].branch) {    //right
					next = myPath[i].right; 
					path = new Vector3[] {nodes[curr], nodes[next]};
					iTween.DrawPath( path, Color.green);
				}
				curr = next;
			}
		}
	}

}
