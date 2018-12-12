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


#if TestSanGuo
        private Faction[] loadedFaction;
        private Section[] loadedSection;
        private CityBuilding[] loadedCity;

        private void Start()
        {
            loadedFaction = GetComponentsInChildren<Faction>();
            foreach (Faction fac in loadedFaction) {
                dic_Faction.Add(fac.ID, fac);
            }

            loadedSection = GetComponentsInChildren<Section>();
            foreach (Section fac in loadedSection)
            {
                dic_Section.Add(fac.ID, fac);
            }
            loadedCity = GetComponentsInChildren<CityBuilding>();
            foreach (CityBuilding city in loadedCity)
            {
                dic_City.Add(city.ID, city);
                HudMgr.Instacne.AddHudCity(city.ID);                
            }

        }



#endif


        //没几个，就算啰嗦一点先实现吧
        private int GetValidTroopID() {
            for (int i = 1; i < 100000; i++) {
                if (!dic_Troop.ContainsKey(i))
                    return i;
            }
            Debug.LogError("Too Many Entity");
            return -1;
        }

        public Dictionary<int, Troop> dic_Troop = new Dictionary<int, Troop>();        
        public Dictionary<int, CityBuilding> dic_City = new Dictionary<int, CityBuilding>();
        public Dictionary<int, Section> dic_Section = new Dictionary<int, Section>();
        public Dictionary<int, Faction> dic_Faction = new Dictionary<int, Faction>();
        //public Dictionary<int, Troop> dic_Troop = new Dictionary<int, Troop>();

        


        //增删统一放到EntityManager 不能其他位只删除。 
        public void RemoveTroop(int id) {
            Troop troop= dic_Troop[id];
            HudMgr.Instacne.DelHudTroop(id); //先移除Hud
            //先移除父对象对他的引用
            if (troop.parentCity && troop.parentCity.dic_troop.ContainsKey(id))
                troop.parentCity.dic_troop.Remove(id);
            //再移除子对象对他的引用

            dic_Troop.Remove(id); //再移除id         
                 
            Destroy(troop.gameObject); //再销毁对象          
        }

        //主要操作的还是Troop 
        //prefabid --也可以看做typetype 类型的类型  数据就是Prefab的数据
        public Troop AddTroopFromPrefab(int prefabid,Vector3 pos)
        {
            GameObject prefab = ResMgr.Instacne.dic_TroopPrefab[prefabid];
            GameObject go = Instantiate(prefab) as GameObject;
            go.transform.position = pos;
            go.transform.SetParent(troopEntityParent, true); //这个设置不设置不影响坐标，因为troopEntityParent 本身就在原点
            Troop troop = go.GetComponent<Troop>();
            //需要建立Data对象  这里先忽略，直接使用prefab数据
            int id = GetValidTroopID();
            dic_Troop.Add(id, troop);
            go.SetActive(true);
            HudMgr.Instacne.AddHudTroop(id);

            return troop; //这里只负责返回，父子关系由调用者处理            
        }

        public Troop AddTroopFromData(int dataid, Vector3 pos)
        {
            return null;    
        }


    }
}
