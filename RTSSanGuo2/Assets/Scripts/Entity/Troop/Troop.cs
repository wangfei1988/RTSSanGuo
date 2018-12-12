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

#if TestTT
        public DTroop data;
#endif
        #region wrapdata
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

        public virtual int CurHP
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
              
        public virtual int MaxHp  //血条显示这个为max 
        {
            get
            {                
                return data.origsoldiernum;
            }
        }

        public DTroopType TypeData {
            get {
                return DataMgr.Instacne.dic_TroopType[data.id_trooptype];
            }
        }

         
        public virtual int Atk //近战只会破坏城防，不会破坏hp，只有射箭才会
        {
            get
            {
                return TypeData.baseatk;
            }
        }




        public virtual int Def
        {
            get
            {
                return TypeData.baseatk;
            }
        }

 
        public override bool CanBeAttack
        {
            get
            {
                return !parentFaction.isPlayer;
            }
        }

        public override bool CanAAttack
        {
            get
            {
                return true; //非玩家阵营都可以被Attack
            }
        }

        public override bool CanBeSelect
        {
            get
            {
                return parentFaction.isPlayer; //非玩家阵营都可以被Attack
            }
        }

        public Person person1;
        public Person person2;
        public Person person3;

        public Faction parentFaction;
        public Section parentSection;
        public CityBuilding parentCity;

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
            interActionCheck.OnTriggerInterAction += OnTrigggerOther;
            Init();
        }

        public void Init()
        {
            agent.speed = moveSpeed;
             
        }


        //Trigger自动触发，而非命令
        private void OnTrigggerOther(Transform transform) {
            //Debug.Log("Trigger" + transform.name);
          SelectAbleEntity entity = transform.GetComponent<SelectAbleEntity>();
            if (entity.selectType == ESelectType.Troop)
            {
                TriggerTroop(entity as Troop);
            }
            else {
                TriggerBuilding(entity as Building);
            }
        }

        private void OnBeAttackByBuilding() {

        }
        private void OnBeAttackByTroop()
        {

        }
        //Trigger并不会改变目标
        private void TriggerTroop(Troop troop) {
            if (troop.parentFaction == this.parentFaction)
                return;
            else
                CheckAutoNearAttackTroop(troop);
        }

        private void TriggerBuilding(Building building)
        {
            if (building.parentFaction == this.parentFaction)
                return;
            else
                CheckAutoNearAttackBuilding(building);
        }


        public float nearAttackFrequence=2f;
        public float lastNearStartAttackTime=0f;//上一次attack 开始时间
        public float nearAttackCheckTime=0.2f; //attack 检测持续时间，建议设置比较小 10帧
        public List<Troop> canAttackTroopList = new List<Troop>();
        public List<Building> canAttackBuildingList = new List<Building>();
        private void CheckAutoNearAttackTroop(Troop troop) {
            if (canAttackTroopList.Contains(troop))
                return;
            float time = Time.timeSinceLevelLoad;
            if (time < lastNearStartAttackTime + nearAttackCheckTime && time>lastNearStartAttackTime)
            {
                canAttackTroopList.Add(troop);
            }
        }
        private void CheckAutoNearAttackBuilding(Building building)
        {
            if (canAttackBuildingList.Contains(building))
                return;
            float time = Time.timeSinceLevelLoad;
            if (time < lastNearStartAttackTime + nearAttackCheckTime&& time>lastNearStartAttackTime) {
                canAttackBuildingList.Add(building);
            }
        }

        public bool multiAttack = false;
        private void DoAutoNearAttack() {
            //只能攻击一个对象
            if (!multiAttack)
            {
                if (canAttackTroopList.Count > 0)
                {
                    if (canAttackTroopList.Contains(targetTroop) && targetType == ETroopTargetType.AttackTroop)
                        NearAttackTroop(targetTroop);
                    else
                        NearAttackTroop(canAttackTroopList[0]); //随机挑选一个最近的，并且不改变目标
                }
                else if (canAttackBuildingList.Count > 0)
                {
                    if (canAttackBuildingList.Contains(targetBuilding) && targetType == ETroopTargetType.AttackBuilding)
                        NearAttackBuilding(targetBuilding);
                    else
                        NearAttackBuilding(canAttackBuildingList[0]); //随机挑选一个最近的，并且不改变目标
                }

            }
            else {
                foreach (Building building in canAttackBuildingList) {
                    NearAttackBuilding(building);
                }
                foreach (Troop troop in canAttackTroopList) {
                    NearAttackTroop(troop);
                }
            }
            canAttackBuildingList.Clear();
            canAttackTroopList.Clear();
       }


        //NearAttack 和remote Attack 同时只能有一个
        public Transform followFxParent; //
        public GameObject curFollowFx;//
        public Transform  remoteBulletStartPoint;
        public GameObject remoteBulletObj;
        public  void NearAttackTroop(Troop tarTroop) {
            float damage = this.atk * (this.atk / tarTroop.def);  //this.atk / troop.def 伤害吸收率
            tarTroop.hp = tarTroop.hp -(int) damage;
            //tarTroop.NearAttackTroop(this); 没有反击，不然就是一对多
        }
        public void NearAttackBuilding(Building tarBuilding)
        {
            float damage = this.atk * (this.atk / tarBuilding.Def);  //this.atk / troop.def 伤害吸收率
            tarBuilding.CurHP = tarBuilding.CurHP - (int)damage;
            tarBuilding.DefAttackTroop(this);//反击
        }

        //改一下，远程武器近战也是射箭，无所谓近战还是远程
        //把那个interAction设置更大范围
        private void DoAutoRemoteAttack() {
            List<Building> canRemoteAttackBuildingList = new List<Building>();
            foreach (KeyValuePair<int, CityBuilding> pair in EntityMgr.Instacne.dic_City)
            {
                if (Vector3.Distance(pair.Value.transform.position, transform.position) <= attackRange)
                {

                }

            }


            if (!multiAttack)
            {
                if (canAttackTroopList.Count > 0)
                {
                    if (canAttackTroopList.Contains(targetTroop) && targetType == ETroopTargetType.AttackTroop)
                        NearAttackTroop(targetTroop);
                    else
                        NearAttackTroop(canAttackTroopList[0]); //随机挑选一个最近的，并且不改变目标
                }
                else if (canAttackBuildingList.Count > 0)
                {
                    if (canAttackBuildingList.Contains(targetBuilding) && targetType == ETroopTargetType.AttackBuilding)
                        NearAttackBuilding(targetBuilding);
                    else
                        NearAttackBuilding(canAttackBuildingList[0]); //随机挑选一个最近的，并且不改变目标
                }

            }
            else
            {
                foreach (Building building in canAttackBuildingList)
                {
                    NearAttackBuilding(building);
                }
                foreach (Troop troop in canAttackTroopList)
                {
                    NearAttackTroop(troop);
                }
            }


            //遍历还是碰撞检测，实际上遍历更快
            if (targetType == ETroopTargetType.AttackBuilding&& targetBuilding!=null) {
                RemoteAttackBuilding(targetBuilding);
            }
            else  if (targetType == ETroopTargetType.AttackTroop && targetTroop != null)
            {
                RemoteAttackTroop(targetTroop);
            }

           
        }

        private void RemoteAttackTroop(Troop troop)
        {

        }
        private void RemoteAttackBuilding(Building building)
        {

        }

        

        private void Update()
        {
            float time = Time.timeSinceLevelLoad;
            if (time > lastNearStartAttackTime + nearAttackFrequence)//过了一个周期
            {
                lastNearStartAttackTime = time;                
                DoAutoNearAttack(); //自动近战攻击
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
                if (targetBuilding != null && targetBuilding.parentFaction == this.parentFaction)
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
                else if (targetBuilding != null && targetBuilding.parentFaction != this.parentFaction)
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




        public void MoveToPoint(Vector3 point,float differ=0.5f) {
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
        public void AttackTroop(Troop troop) {
            targetType = ETroopTargetType.AttackTroop;
            targetBuilding = null;
            targetTroop = troop;
        }

        public void FollowTroop(Troop troop, float differ = 0.5f)
        {
            targetType = ETroopTargetType.FollowTroop;
            targetBuilding = null;
            targetTroop = troop;
            moveToDiff = differ;
        }

        public void AttackBuilding(Building building) {
            targetType = ETroopTargetType.AttackBuilding;
            targetBuilding = building;
            targetTroop = null;
        }

        public void MoveInToBuilding(Building building , float differ = 0.5f)
        {
            targetType = ETroopTargetType.MoveIntoBuilding;
            targetBuilding = building;
            targetTroop = null;
            moveToDiff = differ;//小于这个距离直接enter
        }

       

        private void UpdateAI() {

        }
        private void UpdateSkillAI()
        {

        }

    }
}
