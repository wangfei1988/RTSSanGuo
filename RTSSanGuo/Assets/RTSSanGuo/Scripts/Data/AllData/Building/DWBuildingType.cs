using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace RTSSanGuo.Data
{

    //是否声明这个枚举不是很重要，指示方便后面各种操作。直接用int不是很方便
    public enum EWBuildingType
    {
        TuHao = 1,// 土壕
        
    }

    //这些对象不是new出来的，而是从csv表格加载进来的
    public class DWBuildingType
    {
        public EWBuildingType id;        
        public string imgName;//这个路径固定，所以只要名字       
        public int baseMoneyPerHpLimitPointAdd; //添加hp上限 每一点hp 多少money
        public int baseHpBuildPerDay; //每一级都一样，只是后面血条更长  修复NoNeedMoney  修复时不能做其他操作
        public int maxHP;
        public int atk; // 
        public int def; //
        public int range;//
        
    }
}