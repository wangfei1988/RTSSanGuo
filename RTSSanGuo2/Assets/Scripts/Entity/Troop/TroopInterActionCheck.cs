using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace RTSSanGuo {
public class TroopInterActionCheck : MonoBehaviour {

   
    public List<Building> list_InterBuilding = new List<Building>();
    public List<Troop> list_Intertroop = new List<Troop>();

         
    private void OnTriggerStay(Collider other)
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        //  Debug.Log(other.gameObject + "ontigerStay");
        if (other.transform.tag == "InterAction")
        {
            Transform parentTrans = other.transform.parent;
            SelectAbleEntity entity = parentTrans.GetComponent<SelectAbleEntity>();
            if (entity.selectType == ESelectType.Building)
            {
                    if(!list_InterBuilding.Contains(entity as Building))
                        list_InterBuilding.Add(entity as Building);
            }
            else {
                    if (!list_Intertroop.Contains(entity as Troop))
                        list_Intertroop.Add(entity as Troop);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "InterAction")
        {
                Transform parentTrans = other.transform.parent;
                SelectAbleEntity entity = parentTrans.GetComponent<SelectAbleEntity>();
                if (entity.selectType == ESelectType.Building)
                {
                    if (list_InterBuilding.Contains(entity as Building))
                        list_InterBuilding.Remove(entity as Building);
                }
                else
                {
                    if (list_Intertroop.Contains(entity as Troop))
                        list_Intertroop.Remove(entity as Troop);
                }
            }
    }
}
}