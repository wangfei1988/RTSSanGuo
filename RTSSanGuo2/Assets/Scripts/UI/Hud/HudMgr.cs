using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace RTSSanGuo
{
    //
    public class HudMgr :MonoBehaviour
    {
        public static HudMgr Instacne;
        private void Awake()
        {
            if (Instacne == null)
                Instacne = this;
            else
                Debug.LogError("more than one instance");
        }

        public HudCity hudCityPrefab;
        public HudTroop hudTroopPrefab;

        public Dictionary<int, HudCity> dic_city = new Dictionary<int, HudCity>();
        public Dictionary<int, HudTroop> dic_troop = new Dictionary<int, HudTroop>();


        public void AddHudCity(int cityid) {
            CityBuilding city = EntityMgr.Instacne.dic_City[cityid];
            HudCity hudcity = Instantiate(hudCityPrefab) ;
            hudcity.transform.SetParent(transform, true);
            hudcity.Init(city);
           
            hudcity.gameObject.SetActive(true);
            hudcity.Update();
            //city.onHpChange += hudcity.onCityInfoChange; 不用事件，直接update修改
            dic_city.Add(cityid, hudcity);
        }
        public void DelHudCity(int cityid) {
            HudCity hudcity = dic_city[cityid];
            if (hudcity)
                Destroy(hudcity);            
        }

        public void AddHudTroop(int troopid)
        {
            Troop troop = EntityMgr.Instacne.dic_Troop[troopid];
            HudTroop hudTroop = Instantiate(hudTroopPrefab);
            hudTroop.transform.SetParent(transform, true);
            hudTroop.Init(troop);
           
            hudTroop.gameObject.SetActive(true);
            hudTroop.Update(); //没有setActive  Awake方法不一定执行
            //troop.onHpChange +=
            dic_troop.Add(troopid, hudTroop);
        }
        public void DelHudTroop(int troopID)
        {
            HudTroop hudcity = dic_troop[troopID];
            if (hudcity)
                Destroy(hudcity);
        }

    }
}
