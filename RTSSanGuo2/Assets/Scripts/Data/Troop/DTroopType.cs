using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace RTSSanGuo
{
    
    public enum ETroopKind { BuBing=1,QiBing,GongBing,ShuiBing,GongCheng}
    //不可以被new ，不可以被修改， 也就不需要父子关系了。 直接全局索引
    [Serializable]
    public  class DTroopType:DataBase
    {
        public ETroopKind kind;
        public int baseatk;
        public int basedef;
        public int baseremoteatkrange;
        public float basemovespeed;
        public float baseatkfrequency; //多长时间发起一次attack行为，标准速度下 建议标准2秒   快速情况下0.5秒一次 
        public int resid;
    }
}
