using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

 
public class mynodescript : MonoBehaviour {

	public List<Vector3> nodes = new List<Vector3>(){Vector3.zero, Vector3.zero};   // movable points
	public List<NodeObj> myPath = new List<NodeObj>(){};    // decide the path and branch by set the node index in the path
	public int nodeCount = 2;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
