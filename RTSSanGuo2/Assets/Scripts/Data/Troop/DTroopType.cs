﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace RTSSanGuo
{
    //不可以被new ，不可以被修改， 也就不需要父子关系了。 直接全局索引
    [Serializable]
    public  class DTroopType:DataBase
    {
        public int baseatk;
        public int basedef;
        public int baseremoteatkrange;
        public int basemovespeed;      
       //public int parentid_city; //  //不可以被new ，也就不需要父子关系了。 直接全局索引

    }
}
