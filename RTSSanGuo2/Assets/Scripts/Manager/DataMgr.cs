using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace RTSSanGuo
{
    public class DataMgr :MonoBehaviour
    {
        public static DataMgr Instacne;
        private void Awake()
        {
            if (Instacne == null)
                Instacne = this;
            else
                Debug.LogError("more than one instance");
        }
        

        public Dictionary<int, DTroop> dic_Troop = new Dictionary<int, DTroop>();
        public Dictionary<int, DTroopType> dic_TroopType = new Dictionary<int, DTroopType>();
        public Dictionary<int, DCityBuilding> dic_City = new Dictionary<int, DCityBuilding>();
        public Dictionary<int, DSection> dic_Section = new Dictionary<int, DSection>();
        public Dictionary<int, DFaction> dic_Faction = new Dictionary<int, DFaction>();


    }
}
