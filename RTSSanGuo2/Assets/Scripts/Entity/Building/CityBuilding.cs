#define Testttt
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace RTSSanGuo
{
    //所有功能指令都在这Entity ，如果写到其他Component 也要这里Wrap 一下
    public  class CityBuilding:Building
    {
#if Testttt
        public int initid;//city 一开始就是在场景，和Data绑定必须和

        [SerializeField]
        private DCityBuilding data;
        public DCityBuilding Data {
            set { data = value; }
        }

#endif 
        #region warpdata

        //基础属性的warp
        public override int ID
        {
            get { return data.id; }
        }

        

        public override string Alias
        {
            get { return data.alias; }
        }
        public override string ShortDesc
        {
            get { return data.shortdesc; }
        }
        public override string FullDesc
        {
            get { return data.fulldesc; }
        }
                
        public override int CurHP
        {
            get {
                if (data.curhp > MaxHP)
                    data.curhp = MaxHP;
                return data.curhp;
            }
            set {
                data.curhp = value;
                if (data.curhp > MaxHP)
                    data.curhp = MaxHP;
                if (data.curhp <= 0) {
                    //被占领
                }
            }
        }


        public override int MaxHP
        {
            get {
                if (data.population < 100000)
                    return 1000;
                else if (data.population < 200000)
                    return 2000;
                else if (data.population < 500000)
                    return 3000;
                else if (data.population < 1000000)
                    return 4000;
                else
                    return 5000;
            } 
        }

        public virtual int CurTotalSoldierNum  //血条显示这个为max 
        {
            get
            {
                if (data.curtotalsoldiernum > MaxSoldierNum)
                    data.curtotalsoldiernum = MaxSoldierNum;
                return data.curtotalsoldiernum;
            }
        }

        public virtual int CurLeftSoldierNum //hp=0 或者 CurLeftSoldierNum=0 都算城市破
        {
            get
            {
                int left = CurTotalSoldierNum - SendTroopSoldierNum;
                if (left < 0) left = 0;
                return left;
            }
        }

        public virtual int SendTroopSoldierNum {
            get {
                int sendout = 0;
                foreach (int id in data.idlist_troop) {
                    if (DataMgr.Instacne.dic_Troop.ContainsKey(id))
                    {
                        DTroop  troop= DataMgr.Instacne.dic_Troop[id];
                        if (troop != null) sendout += troop.origsoldiernum;
                        else
                            Debug.LogError(" " + id);
                    }
                    else
                    {
                        Debug.LogError(" " + id);
                    }
                }
                return sendout;
            }
        }




        public virtual int MaxSoldierNum //
        {
            get
            {
                return (int)(data.population * CurSoldierPercent);
            }
        }
        public virtual float CurSoldierPercent {       
            get { return 0.05f;  }
        }


        public override int Atk //近战只会破坏城防，不会破坏hp，只有射箭才会
        {
            get
            {
                if (data.population < 100000)
                    return 100;
                else if (data.population < 200000)
                    return 200;
                else if (data.population < 500000)
                    return 300;
                else if (data.population < 1000000)
                    return 400;
                else
                    return 500;
            }        
        }




        public override int Def
        {
            get
            {
                if (data.population < 100000)
                    return 200;
                else if (data.population < 200000)
                    return 400;
                else if (data.population < 500000)
                    return 600;
                else if (data.population < 1000000)
                    return 800;
                else
                    return 1000;
            }
        }

        //子一级
        public List<int> IDList_Troop {
            get {
                return data.idlist_troop;//list也是对象
            }
        }

        public Dictionary<int, Troop> dic_troop = new Dictionary<int, Troop>();


        //子多层级


        //父一级
        public override Section ParentSection {
            get {
                if (!DataMgr.Instacne.dataPrepared)
                    LogTool.LogError("data not prepared");
                int  sectionid = data.parentid_section;
                if (sectionid != -1 && EntityMgr.Instacne.dic_Section.ContainsKey(sectionid))
                {
                    return EntityMgr.Instacne.dic_Section[sectionid];
                }
                else {
                    LogTool.LogError("can not find section " + sectionid);
                    return null;
                }
            }
        }
        //public Section parentSection;
        public override Faction ParentFaction
        {
            get
            {
                if (!DataMgr.Instacne.dataPrepared)
                    LogTool.LogError("data not prepared");
                return ParentSection.ParentFaction;
            }
        }


        #endregion


        public override bool CanBeAttack
        {
            get
            {
                return !ParentSection.isPlayer;
            }
        }
        
        public override bool CanBeSelect
        {
            get
            {
                return ParentSection.isPlayer; //非玩家阵营都可以被Attack
            }
        }

        public Transform RollyPoint;
        public Transform troopBornPoint;
        public Person leaderPerson;

        //子 一级
        public Dictionary<int, ProductionBuilding> dic_pbuilding = new Dictionary<int, ProductionBuilding>();
        public Dictionary<int, WeaponBuilding> dic_wbuilding = new Dictionary<int, WeaponBuilding>();       
        public Dictionary<int, Person> dic_person = new Dictionary<int, Person>();

        ////父 所有层级
        //public Faction parentFaction;
        //public Section parentSection;


        //子多级，实时计算

        public void BuildTroop(int typeid )
        {
            DTroop dtroop =  DataMgr.Instacne.AddNewTroopData(typeid);
            dtroop.origsoldiernum = 1000;
            dtroop.cursoldiernum = 1000;
            Troop troop = EntityMgr.Instacne.AddTroop(dtroop, troopBornPoint.position);
            //设置子对象        
            data.idlist_troop.Add(dtroop.id);      
            //子对象设置父对象
            dtroop.parentid_city = ID;


            troop.targetType = ETroopTargetType.MoveToPoint;
            troop.MoveToPoint(RollyPoint.position);

        }

        public void EnterTroop(Troop troop) {
            
        }

        public override void DefAttackTroop(Troop troop) //反击
        {
            float damage = this.Atk * (this.Atk / troop.Def);  //this.atk / troop.def 伤害吸收率
            troop.CurHP = troop.CurHP - (int)damage;
        }
    }
}
