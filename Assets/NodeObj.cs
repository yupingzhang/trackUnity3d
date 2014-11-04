using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class NodeObj : MonoBehaviour {

	public bool branch = false;
	public int left = 0;
	public int right = 0;

	public NodeObj(bool br, int l, int r) {
		branch = br;
		left = l;
		right = r;
	}

	// Use this for initialization
//	void Start () {
//	}
//	
	// Update is called once per frame
//	void Update () {
//	
//	}
}
