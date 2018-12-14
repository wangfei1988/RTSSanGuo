#define TestSanGuo

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace RTSSanGuo
{
    public class EntityMgr :MonoBehaviour
    {
        public static EntityMgr Instacne;
        private void Awake()
        {
            if (Instacne == null)
                Instacne = this;
            else
                Debug.LogError("more than one instance");
        }

        public Transform factionEntityParent;
        public Transform sectionEntityParent;
        public Transform cityEntityParent;
        public Transform troopEntityParent;
        public Transform personEntityParent;


#if TestSanGuo
        private Faction[] loadedFaction;
        private Section[] loadedSection;
        private CityBuilding[] loadedCity;

        private void Start()
        {
            //loadedFaction = GetComponentsInChildren<Faction>();
            //foreach (Faction fac in loadedFaction) {
            //    dic_Faction.Add(fac.ID, fac);
            //}

            //loadedSection = GetComponentsInChildren<Section>();
            //foreach (Section fac in loadedSection)
            //{
            //    dic_Section.Add(fac.ID, fac);
            //}
            //loadedCity = GetComponentsInChildren<CityBuilding>();
            //foreach (CityBuilding city in loadedCity)
            //{
            //    dic_City.Add(city.ID, city);
            //    HudMgr.Instacne.AddHudCity(city.ID);                
            //}

        }



#endif


        
        
        public Dictionary<int, Person> dic_Person = new Dictionary<int, Person>();
        public Dictionary<int, Troop> dic_Troop = new Dictionary<int, Troop>();        
        public Dictionary<int, CityBuilding> dic_City = new Dictionary<int, CityBuilding>();
        public Dictionary<int, Section> dic_Section = new Dictionary<int, Section>();
        public Dictionary<int, Faction> dic_Faction = new Dictionary<int, Faction>();
        //public Dictionary<int, Troop> dic_Troop = new Dictionary<int, Troop>();



        #region Play过程数据改变
        //增删统一放到EntityManager 不能其他位只删除。 
        public void RemoveTroop(int id) {
            Troop troop= dic_Troop[id];
            HudMgr.Instacne.DelHudTroop(id); //先移除Hud
            //step1 先移除数据对象 父对象对他的引用
            if (troop.ParentCity && troop.ParentCity.IDList_Troop.Contains(id))
                troop.ParentCity.IDList_Troop.Remove(id);
            //再移除（处理）他的子对象

            //step2 销毁数据对象  数据对象和entity对象id是一样的
            DataMgr.Instacne.dic_Troop.Remove(id);
            //step3 销毁entiyt对象
            dic_Troop.Remove(id); //再移除id                          
            //step4 销毁图形
            Destroy(troop.gameObject); //再销毁对象          
        }

        //主要操作的还是Troop 
        //必须先把数据data准备好，然后才是Entity
        public void  AddTroop(int troopTypeid,DCityBuilding dcityfrom, Vector3 bornpos ,Vector3 movetopos ,int soldiernum ,int personid1 ,int personid2,int personid3)
        {
           // step1 设置数据对象
            DTroop dtroop = DataMgr.Instacne.AddNewTroopData(troopTypeid ,personid1,personid2,personid3);
            dtroop.origsoldiernum = soldiernum;
            dtroop.cursoldiernum = soldiernum;

            //step2 生成gameobject 设置entity对象
            GameObject prefab = ResMgr.Instacne.dic_TroopPrefab[troopTypeid];
            GameObject go = Instantiate(prefab) as GameObject;
            go.transform.position = bornpos;
            go.transform.SetParent(troopEntityParent, true); //这个设置不设置不影响坐标，因为troopEntityParent 本身就在原点
            Troop troop = go.GetComponent<Troop>();
            //需要建立Data对象  这里先忽略，直接使用prefab数据
            troop.Data = dtroop;
            dic_Troop.Add(troop.ID, troop);
            go.SetActive(true);
            HudMgr.Instacne.AddHudTroop(troop.ID);

            //step3 设置父子关系
            //设置city 数据对象的子对象id
            dcityfrom.idlist_troop.Add(dtroop.id);
            //子对象设置父对象id
            dtroop.parentid_city = dcityfrom.id;

            troop.targetType = ETroopTargetType.MoveToPoint;
            troop.CommandMoveToPoint(movetopos);           
        }


        #endregion



        #region 数据加载
        //City 不建议new ，建议直接固定在场景当中，位置也不会变，所以只能初始化数据.而且不能摧毁
        public void  InitAllCityData() {            
            CityBuilding[] cities = cityEntityParent.GetComponentsInChildren<CityBuilding>();
            foreach (CityBuilding city in cities) {
                if (DataMgr.Instacne.dic_City.ContainsKey(city.initid)) {
                    city.Data = DataMgr.Instacne.dic_City[city.initid];
                    dic_City.Add(city.ID, city);
                    HudMgr.Instacne.AddHudCity(city.ID);
                }                    
                else
                {
                    LogTool.LogError("DataMgr not have id " + city.initid);
                }
            }
        }

        public Section AddSectionFromData(int sectionid) {
            if (!DataMgr.Instacne.dic_Section.ContainsKey(sectionid))
            {
                LogTool.LogError("DataMgr not have id " + sectionid);
                return null;
            }
            DSection dsection = DataMgr.Instacne.dic_Section[sectionid];
            GameObject go = new GameObject();
            go.transform.SetParent(sectionEntityParent);
            Section section = go.AddComponent<Section>();
            section.data = dsection;
            dic_Section.Add(section.ID, section);
            if (section.ID == DataMgr.Instacne.selSaveData.id_playerSection)
                section.isPlayer = true;
            return section;
        }

        public Faction AddFactionFromData(int factionid)
        {
            if (!DataMgr.Instacne.dic_Faction.ContainsKey(factionid))
            {
                LogTool.LogError("DataMgr not have id " + factionid);
                return null;
            }
            DFaction dfaction = DataMgr.Instacne.dic_Faction[factionid];
            GameObject go = new GameObject();
            go.transform.SetParent(factionEntityParent);
            Faction faction = go.AddComponent<Faction>();
            faction.Data = dfaction;
            dic_Faction.Add(faction.ID, faction);
            if (faction.ID == DataMgr.Instacne.selSaveData.id_playerFaction)
                faction.isPlayer = true;
            return faction;
        }

        public Person AddPersonFromData(int personid)
        {
            if (!DataMgr.Instacne.dic_Person.ContainsKey(personid))
            {
                LogTool.LogError("DataMgr not have id " + personid);
                return null;
            }
            DPerson dfaction = DataMgr.Instacne.dic_Person[personid];
            GameObject go = new GameObject("person_"+personid);
            go.transform.SetParent(personEntityParent);
            Person person = go.AddComponent<Person>();
            person.Data = dfaction;
            dic_Person.Add(person.ID, person);            
            return person;
        }


        #endregion

    }
}
