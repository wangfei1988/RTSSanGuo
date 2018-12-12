using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestNavmesh02 : MonoBehaviour
{
    public Transform hitTarget;
    private NavMeshAgent agent;
    // Use this for initialization
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame

    private RaycastHit Hit; 
    private Ray RayCheck;
    void Update()
    {

        RayCheck = Camera.main.ScreenPointToRay(Input.mousePosition); //这个是错误的
        RaycastHit Hit;
        Debug.DrawRay(RayCheck.origin, RayCheck.direction*2000 ,Color.red);       
       
       
       
        // Debug.DrawRay(RayCheck.origin, RayCheck.direction);
        if (Physics.Raycast(RayCheck, out Hit, Mathf.Infinity))
        {
           
            //选择碰撞体
            if (Input.GetMouseButtonDown(0) && Hit.transform != null)//右键Action
            {
                // MovePoint(Hit.transform.position); //这个地方用错了，Hit.transform代表的是点中的Collider对应的物体     
                MovePoint(Hit.point);
                hitTarget.position = Hit.point;
                Debug.Log(Hit.point);
            }

        }
        
    }

    void MovePoint(Vector3 point)
    {
        agent.SetDestination(point);
    }
}