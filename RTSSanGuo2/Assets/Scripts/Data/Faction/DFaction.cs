using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace RTSSanGuo
{
    [Serializable]
    public  class DFaction:DataBase
    {
       
        public int id_clolor;
        public int techpoint;

        public int id_leadperson;

        //子类
        public List<int> idlist_section = new List<int>(); //只保存一级子类
        public List<int> idlist_wbuilding = new List<int>(); //这个精确到faction 就可以，不用确认是哪个city


        public Dictionary<int, int> dic_factionrelation = new Dictionary<int, int>(); //<目标faction 关系> 只记录正数和负数0 不喜欢不讨厌 不需要记载





    }
}
