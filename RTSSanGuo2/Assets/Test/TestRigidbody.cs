using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRigidbody : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddForce(transform.forward*10);
        //向forward方向添加10牛顿的力
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
