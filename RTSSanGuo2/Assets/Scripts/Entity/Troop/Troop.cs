#define TestTT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;
namespace RTSSanGuo
{
    //所有功能指令都在这Entity ，如果写到其他Component 也要这里Wrap 一下

    //Building 的AI 很简单
    public enum ETroopTargetType {None=1,MoveToPoint,FollowTroop,AttackTroop,AttackBuilding,MoveIntoBuilding,RePairBuilding}
    

    public  class Troop:SelectAbleEntity
    {

        private DTroop data =null;//这个只在内部使用，外部使用必须通过Wrap
        public DTroop Data {
            set { data = value; }
        }

        #region wrap data
        /*******wrap basic本身 ************/
        public override int ID
        {
            get { return data.id; }
        }
        public virtual int TypeID
        {
            get { return data.id_trooptype; }
        }
        public virtual int ResID
        {
            get { return TypeData.resid; }
        }

        public override string Alias
        {
            get { return data.alias; }
        }

        public string TroopName {
            get { return Alias; }
        }

        public override string ShortDesc
        {
            get { return data.shortdesc; }
        }
        public override string FullDesc
        {
            get { return data.fulldesc; }
        }
        public DTroopType TypeData
        {
            get
            {

                return DataMgr.Instacne.dic_TroopType[data.id_trooptype];
            }
        }
        public float MoveSpeed
        {
            get
            {                
                return TypeData.basemovespeed/GameMgr.Instacne.RunSpeed;
            }
        }
        public float AtkFrequency
        {
            get
            {                 
                return TypeData.baseatkfrequency / GameMgr.Instacne.RunSpeed; 
                //长期Skill 无论near 还是remote 都是受到整个影响
                //一次性 以及释放对象的skill不 被影响
            }
        }


        /******一级子对象（id直接记录在data）+ 多级子对象（通过级联获取）全局唯一的东西 **********************/
        public Person Person1 {
            get
            {
                if (!DataMgr.Instacne.dataPrepared)
                    LogTool.LogError("data not prepared");
                int personid = data.id_person1;
                if (personid != -1 && EntityMgr.Instacne.dic_Person.ContainsKey(personid))
                {                    
                    return EntityMgr.Instacne.dic_Person[personid];
                }
                else
                {
                    LogTool.LogError("can not find section " + personid);
                    return null;
                }
            }
        }
        public Person Person2
        {
            get
            {
                if (!DataMgr.Instacne.dataPrepared)
                    LogTool.LogError("data not prepared");
                int personid = data.id_person2;
                if (personid != -1 && EntityMgr.Instacne.dic_Person.ContainsKey(personid))
                {
                    return EntityMgr.Instacne.dic_Person[personid];
                }
                else
                {                     
                    return null;
                }
            }
        }
        public Person Person3
        {
            get
            {
                if (!DataMgr.Instacne.dataPrepared)
                    LogTool.LogError("data not prepared");
                int personid = data.id_person3;
                if (personid != -1 && EntityMgr.Instacne.dic_Person.ContainsKey(personid))
                {
                    return EntityMgr.Instacne.dic_Person[personid];
                }
                else
                {
                    return null;
                }
            }
        }

        /******一级父对象（id在子对象有，但是是反向推算） +多级父对象（通过级联获取）******/
        public virtual CityBuilding ParentCity
        {
            get
            {
                if (!DataMgr.Instacne.dataPrepared)
                    LogTool.LogError("data not prepared");
                int cityid = data.parentid_city;
                if (cityid != -1 && EntityMgr.Instacne.dic_City.ContainsKey(cityid))
                {
                    return EntityMgr.Instacne.dic_City[cityid];
                }
                else
                {
                    LogTool.LogError("can not find section " + cityid);
                    return null;
                }
            }
        }
        public override Section ParentSection
        {
            get
            {
                if (!DataMgr.Instacne.dataPrepared)
                    LogTool.LogError("data not prepared");
                return ParentCity.ParentSection;
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

        /******其他Wrap******/
        public override int CurHP
        {
            get
            {                 
                return data.cursoldiernum;
            }
            set
            {
                data.cursoldiernum = value;
                if (data.cursoldiernum > data.origsoldiernum)
                    data.cursoldiernum = data.origsoldiernum;
                if (data.cursoldiernum <= 0)
                {
                    //被占领
                }
            }
        }
              
        public override int MaxHP  //血条显示这个为max 
        {
            get
            {                
                return data.origsoldiernum;
            }
        }

        public float AtkDefMult_WuWei {
            get {
                //60 为准  最大50+%
                return ((Person1.Tong + Person1.Wu) / 2 - 60) / 120.0f;
            }
        }
        public float AtkDefMult_ShiXing
        {
            get
            {
                ETroopKind kind = TypeData.kind;
                //最大+60%  S=5   A  B  C  D
                float personshixingmult = 0f;
                if (kind == ETroopKind.BuBing)
                {
                    personshixingmult += (Person1.Level_BuBing - 2) * 0.2f;
                }
                else if (kind == ETroopKind.QiBing)
                {
                    personshixingmult += (Person1.Level_QiBing - 2) * 0.2f;
                }
                else if (kind == ETroopKind.GongCheng)
                {
                    personshixingmult += (Person1.Level_GongChen - 2) * 0.2f;
                }
                else if (kind == ETroopKind.GongBing)
                {
                    personshixingmult += (Person1.Level_GongBing - 2) * 0.2f;
                }
                else if (kind == ETroopKind.ShuiBing)
                {
                    personshixingmult += (Person1.Level_ShuiBing - 2) * 0.2f;
                }
                return personshixingmult;
            }
        }

        
        public float AtkMult_BeiSkill {
            get { return 0f; }
        }

        public float DefMult_BeiSkill
        {
            get { return 0f; }
        }
        public float AtkRangeAdd_BeiSkill //绝对值
        {
            get { return 0f; }
        }
        public float MoveSpeedAdd_BeiSkill
        {
            get { return 0f; }
        }

        public float AtkMult_Tech
        {
            get { return 0f; }
        }

        public float DefMult_Tech
        {
            get { return 0f; }
        }
        public float AtkRangeAdd_Tech //绝对值
        {
            get { return 0f; }
        }
        public float MoveSpeedAdd_Tech
        {
            get { return 0f; }
        }

        public override int Atk //近战只会破坏城防，不会破坏hp，只有射箭才会
        {
            get
            {
                int atk = (int) (TypeData.baseatk*(1 + AtkDefMult_WuWei + AtkDefMult_ShiXing));
                return atk;
            }
        }
                
        public override int Def
        {
            get
            {
                int def = (int)(TypeData.basedef * (1 + AtkDefMult_WuWei + AtkDefMult_ShiXing));
                return def;
            }
        }
        public override bool IsPlayer
        {
            get { return ParentSection.isPlayer; }
        }

        public override bool CanBeAttack
        {
            get
            {
                return !IsPlayer;
            }
        }

        public override bool CanBeSelect
        {
            get
            {
                return IsPlayer; //非玩家阵营都可以被Attack
            }
        }

        
       
        #endregion

        //Troop 有2个Trigger  Selection 和Attack
        //Building 有3个Triiger  Selection Attack Building   RangeAvoid（武器）



        public Transform rectSelectedPoint; // 这一点出现在框选区域，就算选中
        public ETroopTargetType targetType = ETroopTargetType.None;
        public Building targetBuilding;
        public Troop targetTroop;
        public Vector3 targetPoint;
        public float moveToDiff = 0.5f;//不可能完全重叠，允许误差
        private NavMeshAgent agent;
        public  TroopInterActionCheck interActionCheck; //直接赋值算了


        public void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.speed = MoveSpeed;
        }

        #region attack 
        /******** 被攻击**************/
        private void OnBeAttackByBuilding() {

        }
        private void OnBeAttackByTroop()
        {

        }

        /******** 自动攻击操作**************/
        private float lastAutoAttackStartTime=0f;//上一次attack 开始时间      
        public bool multiAttack = false;
        public List<Troop> CanAutoAttackTroopList {
            get {
                List<Troop> list = new List<Troop>();
                foreach (Troop troop in interActionCheck.list_Intertroop) {
                    if (troop.ParentFaction != ParentFaction)
                        list.Add(troop);
                }
                return list;
            }
        }
        public List<Building> CanAutoAttackBuildingList
        {
            get
            {
                List<Building> list = new List<Building>();
                foreach (Building troop in interActionCheck.list_InterBuilding)
                {
                    if (troop.ParentFaction != ParentFaction)
                        list.Add(troop);
                }
                return list;
            }
        }
        private void DoAutoAttack() {
            //只能攻击一个对象
            if (!multiAttack)
            {
                if (CanAutoAttackTroopList.Count > 0)
                {
                    if (CanAutoAttackTroopList.Contains(targetTroop) && targetType == ETroopTargetType.AttackTroop)
                        DoAttackTroop(targetTroop);
                    else
                        DoAttackTroop(CanAutoAttackTroopList[0]); //随机挑选一个最近的，并且不改变目标
                }
                else if (CanAutoAttackBuildingList.Count > 0)
                {
                    if (CanAutoAttackBuildingList.Contains(targetBuilding) && targetType == ETroopTargetType.AttackBuilding)
                        DoAttackBuilding(targetBuilding);
                    else
                        DoAttackBuilding(CanAutoAttackBuildingList[0]); //随机挑选一个最近的，并且不改变目标
                }

            }
            else {
                foreach (Building building in CanAutoAttackBuildingList) {
                    DoAttackBuilding(building);
                }
                foreach (Troop troop in CanAutoAttackTroopList) {
                    DoAttackTroop(troop);
                }
            }
            CanAutoAttackBuildingList.Clear();
            CanAutoAttackTroopList.Clear();
       }


        /******** 自动图形音效操作**************/
        public Transform followFxParent; //
        public GameObject curFollowFx;//
        public Transform  remoteBulletStartPoint;
        public GameObject remoteBulletObj;
        public  void DoAttackTroop(Troop tarTroop) {
            float damage = this.Atk * (this.Atk / tarTroop.Def);  //this.atk / troop.def 伤害吸收率
            tarTroop.CurHP = tarTroop.CurHP -(int) damage;
            //tarTroop.NearAttackTroop(this); 没有反击，不然就是一对多
        }
        public void DoAttackBuilding(Building tarBuilding)
        {
            float damage = this.Atk * (this.Atk / tarBuilding.Def);  //this.atk / troop.def 伤害吸收率
            tarBuilding.CurHP = tarBuilding.CurHP - (int)damage;
            tarBuilding.DefAttackTroop(this);//反击
        }

        #endregion



        private float runningtime =0f ;
        private void Update()
        {
            if (GameMgr.Instacne.state != EGameState.Running) {
                agent.isStopped = true;
                return;
            }
            agent.isStopped = false;
            runningtime += Time.deltaTime;
            if (runningtime > lastAutoAttackStartTime + AtkFrequency)//过了一个周期
            {
                lastAutoAttackStartTime = runningtime;                
                DoAutoAttack(); //自动近战攻击
            }
            UpdateAI(); //主要是设定目标
            UpdateSkillAI(); //技能释放

            //下面都是被动的，主动的技能通过UI
            if (targetType == ETroopTargetType.None)
            {

            }
            else if (targetType == ETroopTargetType.AttackBuilding || targetType == ETroopTargetType.RePairBuilding)
            {//只有工程队可以，都是近战
                if (targetBuilding != null)
                {
                    if (!interActionCheck.list_InterBuilding.Contains(targetBuilding))
                    { //不在interAction区域内，往前移动
                        agent.isStopped = false;
                        agent.SetDestination(targetBuilding.transform.position);
                    }
                    else
                    { //已经在范围内了
                        agent.isStopped = true;
                    }
                }
                else
                {

                }
            }
            else if (targetType == ETroopTargetType.MoveIntoBuilding)
            {
                if (targetBuilding != null && targetBuilding.ParentFaction == this.ParentFaction)
                {
                    CityBuilding parentCity = targetBuilding as CityBuilding;
                    if (!interActionCheck.list_InterBuilding.Contains(targetBuilding))
                    { //不在interAction区域内，往前移动
                        agent.isStopped = false;
                        agent.SetDestination(targetBuilding.transform.position);
                    }
                    else
                    { //已经在范围内了
                        agent.isStopped = true;
                        parentCity.EnterTroop(this);
                        return;
                    }
                }
                else if (targetBuilding != null && targetBuilding.ParentFaction != this.ParentFaction)
                {
                    targetType = ETroopTargetType.AttackBuilding;


                }
            }
            else if (targetType == ETroopTargetType.AttackTroop|| targetType ==ETroopTargetType.FollowTroop) {
                if (targetTroop != null)
                {
                    if (!interActionCheck.list_InterBuilding.Contains(targetBuilding))
                    { //不在interAction区域内，往前移动
                        agent.isStopped = false;
                        agent.SetDestination(targetBuilding.transform.position);
                    }
                    else
                    { //已经在范围内了,啥也不干，攻击是自动攻击
                        agent.isStopped = true;
                    }
                }
                else
                {

                }

            }
            else if (targetType == ETroopTargetType.MoveToPoint)
            {
                if (Vector3.Distance(targetPoint,transform.position) <=moveToDiff)
                {
                    agent.isStopped = true;
                }
                else
                {
                    agent.isStopped = false;
                    agent.SetDestination(targetPoint);
                }

            }            
        }
        

        #region commmad
        public void CommandMoveToPoint(Vector3 point,float differ=0.5f) {
            if (point.y < 1) point.y = 1f;
            targetType = ETroopTargetType.MoveToPoint;
            targetPoint = point;
            targetBuilding = null;
            targetTroop = null;
            moveToDiff = differ;
           // Debug.Log("" + this.entityname + "move destination" + point.ToString());
            agent.SetDestination(point); 

        }        
        //这个必须使用update操作，并非一次性
        public void CommadAttackTroop(Troop troop) {
            targetType = ETroopTargetType.AttackTroop;
            targetBuilding = null;
            targetTroop = troop;
        }

        public void CommandFollowTroop(Troop troop, float differ = 0.5f)
        {
            targetType = ETroopTargetType.FollowTroop;
            targetBuilding = null;
            targetTroop = troop;
            moveToDiff = differ;
        }

        public void CommandAttackBuilding(Building building) {
            targetType = ETroopTargetType.AttackBuilding;
            targetBuilding = building;
            targetTroop = null;
        }

        public void CommandMoveInToBuilding(Building building , float differ = 0.5f)
        {
            targetType = ETroopTargetType.MoveIntoBuilding;
            targetBuilding = building;
            targetTroop = null;
            moveToDiff = differ;//小于这个距离直接enter
        }
        #endregion


        private void UpdateAI() {

        }
        private void UpdateSkillAI()
        {

        }

    }
}
