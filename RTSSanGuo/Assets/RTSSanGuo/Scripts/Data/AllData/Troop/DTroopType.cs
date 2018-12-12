using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
namespace RTSSanGuo.Data
{
    public enum ETroopTypeType {
        ALL=1,
        NoWeapon ,
        BuBing,
        QiBing,
        GongBing,
        NuBing,
        GongCheng,
        ShuiBing,
    }

    //这些都是从csv表格加载，不可以new
    public class DTroopType:DGraphicBase
    {
        public ETroopTypeType kind;
        public List<int> needBuildingIDList = new List<int>();
        public int needTechID;
        public List<int> needCityIDList = new List<int>();        
        public int baseAtk;
        public int baseDef;
        public int baseMoveSpeed;// 
        public int baseRemoteAtkDis;//远程攻击距离
        //都是正面效果
        public Dictionary<ETroopTypeType, float> dic_XiangKe = new Dictionary<ETroopTypeType, float>();        

    }
    

}
