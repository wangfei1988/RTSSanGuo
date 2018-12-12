using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace RTSSanGuo
{
    [Serializable]
    public  class DColor:DataBase  //Color 每个阵营固定颜色 不太好，还是实际运行时生成，相邻的颜色差距大一些好.但是这样更复杂，还是剧本就写好算了
    {
        public int leaderPerson;
        public Color factionColor;
        public bool isPlayer;

        //子
        public Dictionary<int, Section> dic_section = new Dictionary<int, Section>();
    }
}
