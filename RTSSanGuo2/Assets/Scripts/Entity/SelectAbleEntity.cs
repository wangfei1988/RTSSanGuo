using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace RTSSanGuo
{
    public enum ESelectType {Building=1, Troop}
    public  class SelectAbleEntity:EntityBase
    {
        //public bool playerControl;//初始化就赋值，不用每次都去判断。 和Faction 有关
        //只有PlayerFaction才可以被选中，其他的只能hover
        public virtual bool CanBeAttack {
            get {
                return false; //非玩家阵营都可以被Attack
            }
        }

        public virtual bool CanAAttack
        {
            get
            {
                return false; //非玩家阵营都可以被Attack
            }
        }

        public virtual bool CanBeSelect
        {
            get
            {
                return false; //非玩家阵营都可以被Attack
            }
        }

        public virtual bool IsPlayerCtr {
            get {
                return true;
            }
        }



        public virtual int CurHP
        {
            get
            {
                Debug.LogError("Must override in child ");
                return 0;
            }
            set
            {
                Debug.LogError("Must override in child ");
            }
        }

        public virtual int MaxHP //血条显示这个为max 
        {
            get
            {
                Debug.LogError("Must override in child ");
                return 0;
            }
        }

    
        public virtual int Atk
        {
            get { Debug.LogError("Must override in child "); return 100; }
        }
        public virtual int Def
        {
            get { Debug.LogError("Must override in child ");  return 100; }
        }

        public float hudXOffset;
        public float hudYOffset;

        

        public AudioSource selectAudio;// 暂时不要使用配置
        public ESelectType selectType = ESelectType.Building;
        

        public Action<SelectAbleEntity> OnSelect;
        public virtual void  Select() {
           /// Debug.LogError("Must Over Ride in child");
        }
        public Action<SelectAbleEntity> OnUnSelect;
        public virtual void UnSelect() {
           ////// Debug.LogError("Must Over Ride in child");
        }

        public Action<SelectAbleEntity> OnHover;
        public virtual void Hover()
        {
           // Debug.LogError("Must Over Ride in child");
        }
        public Action<SelectAbleEntity> OnUnHover;
        public virtual void UnHover()
        {
          //  Debug.LogError("Must Over Ride in child");
        }

    }
}
