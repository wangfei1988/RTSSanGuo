using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace RTSSanGuo
{
    //存档1个csv文件 ,说明所有存档信息。每一行有一个同名文件夹
    //剧本1个csv文件，说明所有剧本信息。 每一行有一个同名文件夹

    //在entity层
    [Serializable]
    public  class DScenerialData:DataBase
    {        
        public int year;
        public int month;
        public int day;
        public int season;
        public string subfold;
    }
}
