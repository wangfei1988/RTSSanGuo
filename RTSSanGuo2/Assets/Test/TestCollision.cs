using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("my" + name +"和"+ collision.transform.name +"发生Collsion 碰撞事件，无法相互穿透");
    }

   
        

    private void OnTriggerEnter(Collider other)
    {
            Debug.Log("my" + name + "和" + other.transform.name + "发生Triger 碰撞事件，可以相互穿透");
        
    }
}
