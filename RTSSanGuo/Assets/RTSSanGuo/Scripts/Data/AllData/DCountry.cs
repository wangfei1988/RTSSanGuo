using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTSSanGuo.Data
{
    public class DCountry:DBase
    {       
        public int leftTechPoint;//未试用tech点
        public int shenwang;// 声望  全局--事件影响   造反 忠诚
        public List<int> haveTechIDList = new List<int>();
        public int id_leaderPerson;  //
        public List<int> id_sectionList = new List<int>(); //至少要有一个
        public string shortDesc;
        public string fullDesc;
    }
}
