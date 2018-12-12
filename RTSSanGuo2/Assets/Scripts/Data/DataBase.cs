using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace RTSSanGuo
{
    [Serializable] //加上这个关键字之后可以被Inspector识别
    //只是存储数据，不存储其他的。 而且只存储一级子对象的id索引
    public  class DataBase
    {
        public int id;  //同一类的ID不可重复 id用int ，查询速度更快
        public string alias;
        public string shortdesc;
        public string fulldesc; 
    }
}
