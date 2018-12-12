using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace RTSSanGuo
{
    public class ResMgr :MonoBehaviour
    {
        public static ResMgr Instacne;
        private void Awake()
        {
            if (Instacne == null)
                Instacne = this;
            else
                Debug.LogError("more than one instance");
        }

        public GameObject[] troopPrefabArray;
        public Dictionary<int, GameObject> dic_TroopPrefab = new Dictionary<int, GameObject>();
        //加载这个dic_TroopPrefab的时候可以使用dataManager数据初始化
        //
        //public Dictionary<int, GameObject> dic_CityPrefab = new Dictionary<int, GameObject>();

        private void Start()
        {
            foreach (GameObject go in troopPrefabArray) {
               int prefabid = go.transform.GetComponent<Troop>().prefabid;
                dic_TroopPrefab.Add(prefabid, go);
            }
        }


    }
}
