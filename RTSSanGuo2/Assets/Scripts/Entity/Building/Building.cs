using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace RTSSanGuo
{
    public enum EBuildingType {City,PBuilding,WBuilding }

    //所有功能指令都在这Entity ，如果写到其他Component 也要这里Wrap 一下
    // 
    public class Building : SelectAbleEntity
    {

        /******其他Wrap******/
        public virtual bool  CanDefAttack{
            get { return type == EBuildingType.City; } //city类型可以这么做 pbuilding  wbuilding troop 都不行
        }
        public virtual  bool CanTroopMoveInto
        {
            get { return type == EBuildingType.City;}
        }
        //每一种building都要单独写，因为行为都不一样。这个和Troop还不一样。Troop 行为都是一样的，move attack skill



        public EBuildingType type = EBuildingType.City;
        //父 所有层级     
        
        public Action onHpChange;
        public virtual void DefAttackTroop(Troop troop) //反击
        {            
            Debug.Log("must use child");
        }


       
        //在附近没有敌方单位的时候可以，有的话，直接走到敌方单位.
        public virtual void  SetTroopRollyPoint(Vector3 point)
        {

        }
    }
}
