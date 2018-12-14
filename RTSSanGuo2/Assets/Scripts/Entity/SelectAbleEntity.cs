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

        #region wrap
        /***************wrap basic本身 ************/
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
            get { Debug.LogError("Must override in child "); return 100; }
        }
        //攻击范围和图形有关，直接在prefab里面

        /******一级父对象（id在子对象有，但是是反向推算） +多级父对象（通过级联获取）******/
        public virtual Faction ParentFaction { get { LogTool.LogError("must override in child ");  return null; } }
        public virtual Section ParentSection { get { LogTool.LogError("must override in child ");  return null; } }

        /******其他Wrap******/      
       
        public virtual bool IsPlayer
        {
            get
            {
                LogTool.LogError("must override in child ");
                return false; //非玩家阵营都可以被Attack
            }
        }
        public virtual bool CanBeAttack {
            get {
                LogTool.LogError("must override in child ");
                return false; //非玩家阵营都可以被Attack
            }
        }
        //只有PlayerFaction才可以被选中，其他的只能hover
        public virtual bool CanBeSelect
        {
            get
            {
                LogTool.LogError("must override in child ");
                return false; //非玩家阵营都可以被Attack
            }
        }       

        #endregion
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
