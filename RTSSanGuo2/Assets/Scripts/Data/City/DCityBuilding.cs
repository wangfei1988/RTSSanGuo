using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace RTSSanGuo
{
    [Serializable]
    public  class DCityBuilding:DataBase
    {
        public int food;
        public int money;
        public int population;
        public int curhp;
        public int curtotalsoldiernum;// 当前所有soldier 数量    
        public int mingxin;
        public int zhian;
        public int id_leadperson;
        
        public List<int> idlist_pbuilding = new List<int>(); //只保存一级子类
        public List<int> idlist_troop = new List<int>();
        public List<int> idlist_person = new List<int>();
        public List<int> idlist_freeperson = new List<int>(); //没有入职





        public int parentid_section;//一级父类---不是保存，而是逆向计算

    }
}
