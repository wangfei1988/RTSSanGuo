using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
namespace RTSSanGuo.Data
{
    // 这个不需要Graphic ，Graphic 存储在Type
    public class DPBuilding:DBase{               
        public int id;
        public EPBuildingType type;
        public int level;
        public Vector3 pos;
        public int id_person;//子类索引 不是所有的都支持
        public int curHP;
        public int curWorkingDay; //  
    }


}