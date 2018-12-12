
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
namespace RTSSanGuo.Data
{
    public enum ECityTypeEnum
    {
        NorMalCity = 1,
        GuanKou,
        HarBor
    }

    public class Dcity:DGraphicBase
    {        
        public ECityTypeEnum type;
        public int id_taiShou; //太守
        public List<int> id_allPersonList = new List<int>();
        public List<int> id_outPersonList = new List<int>();//出征人物列表
        public List<int> id_PbuildingList = new List<int>();
        public List<int> id_WbuildingList = new List<int>();
        public Vector3 pos;//其实这个应该在Map上，所以这个值，没啥用  City Prefab  不考虑扩建

        public int curminxin;//持续上升，有上限.  下降 兵役所工作时  不工作时不下降   
        public int population; //
        //int soldierpercent  兵役人口比例 ----太守+科技综合作用
        public int food;
        public int money;       
        public int  soldierNum;//  小于军营  + 小于 population*soldierpercent   军营和首府都可以出兵，首府只能出民兵
        //public Dictionary<WeaponTypeEnum, float> allWeaponDic = new Dictionary<WeaponTypeEnum, float>();
        // 兵器没有数量 不同种类消耗money不一样
        //船之类的可以有0.1个
        //兵器坊 养马场  弓弩坊  器械所        
        public int inCitySoliderNum;//
        public List<int> outTroopIDList = new List<int>();  
        public bool nearRiver;//靠近河海

       



    }

}
