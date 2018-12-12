using System.Collections.Generic;
using System;
using System.IO;
using RTSSanGuo.Data;
using RTSSanGuo.Util;
namespace RTSSanGuo.Entity
{
    public class EPerson
    {
        public DPerson dPerson = null;
        //id  加成比
        public Dictionary<int, float> dic_affectPBuilding = new Dictionary<int, float>();
        public Dictionary<ETroopTypeType, float> dic_affectTroopAtk = new Dictionary<ETroopTypeType, float>();
        public Dictionary<ETroopTypeType, float> dic_affectTroopDef = new Dictionary<ETroopTypeType, float>();
        public Dictionary<ETroopTypeType, float> dic_affectTroopMoveSpeed = new Dictionary<ETroopTypeType, float>();
        public Dictionary<ETroopTypeType, float> dic_affectTroopRemoteAtkRange = new Dictionary<ETroopTypeType, float>();
        public bool Init(DPerson person)
        {
            dPerson = person;
            InitSkill();
            return true;
        }

        #region objwrap
        public List<DBeiDongToPbuildingSkill> BeiDongToPbuildingSkillList {
            get {
                List<DBeiDongToPbuildingSkill> list = new List<DBeiDongToPbuildingSkill>();
                foreach (int id in dPerson.id_BeiDongToPbuildingSkillList)
                    list.Add(DataMgr.Instance.dic_BeiDongToPbuildingSkill[id]);
                return list;
            }
        }
        public List<DBeiDongToTroopSkill> BeiDongToTroopSkillList
        {
            get
            {
                List<DBeiDongToTroopSkill> list = new List<DBeiDongToTroopSkill>();
                foreach (int id in dPerson.id_BeiDongToPbuildingSkillList)
                    list.Add(DataMgr.Instance.dic_BeiDongToTroopSkill[id]);
                return list;
            }
        }
        public List<DZhuDongSkill> DZhuDongSkillList
        {
            get
            {
                List<DZhuDongSkill> list = new List<DZhuDongSkill>();
                foreach (int id in dPerson.id_BeiDongToPbuildingSkillList)
                    list.Add(DataMgr.Instance.dic_ZhuDongSkill[id]);
                return list;
            }
        }
        #endregion

        #region parent
        public ETroo

        #endregion


        private void InitSkill() {
            foreach (DBeiDongToPbuildingSkill skill in BeiDongToPbuildingSkillList) {
                foreach (int id in skill.affectBuildingIDList) {
                    CommonUtil.DicPlusOrAdd(dic_affectPBuilding, id, skill.affectPercent);                    
                }             
            }

            foreach (DBeiDongToTroopSkill skill in BeiDongToTroopSkillList)
            {
                ETroopTypeType type = skill.affectType;
                if (dic_affectTroopAtk.ContainsKey(type))
                    dic_affectTroopAtk[type] = dic_affectTroopAtk[type] + skill.affectAtkPercent; //简单起见---多个种类建筑有效，数值一致
                else
                    dic_affectTroopAtk.Add(type, skill.affectAtkPercent);

                if (dic_affectTroopDef.ContainsKey(type))
                    dic_affectTroopDef[type] = dic_affectTroopDef[type] + skill.affectDefPercent; //简单起见---多个种类建筑有效，数值一致
                else
                    dic_affectTroopDef.Add(type, skill.affectDefPercent);

                if (dic_affectTroopMoveSpeed.ContainsKey(type))
                    dic_affectTroopMoveSpeed[type] = dic_affectTroopMoveSpeed[type] + skill.affectmoveSpeedPercent; //简单起见---多个种类建筑有效，数值一致
                else
                    dic_affectTroopMoveSpeed.Add(type, skill.affectmoveSpeedPercent);

                if (dic_affectTroopRemoteAtkRange.ContainsKey(type))
                    dic_affectTroopRemoteAtkRange[type] = dic_affectTroopRemoteAtkRange[type] + skill.affectRemoteAtkRangePercent; //简单起见---多个种类建筑有效，数值一致
                else
                    dic_affectTroopRemoteAtkRange.Add(type, skill.affectRemoteAtkRangePercent);

            }
            //主动的不需要一开始就计算好
        }

    } 
}
