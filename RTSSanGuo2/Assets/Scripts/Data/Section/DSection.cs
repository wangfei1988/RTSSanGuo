using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace RTSSanGuo
{
    [Serializable]
    public  class DSection:DataBase
    {
        public int id_leadperson;
        //public int id_clolor; faction color 就决定了
        //public bool isPlayer; 这个应该在存档里面设置
        public List<int> idlist_city = new List<int>(); //只保存一级子类
                                                        //public Dictionary<int, int> dic_factionrelation = new Dictionary<int, int>(); //<目标faction 关系> 只记录正数和负数0 不喜欢不讨厌 不需要记载


        public int parentid_faction;//一级父类---不是保存，而是逆向计算

    }
}
