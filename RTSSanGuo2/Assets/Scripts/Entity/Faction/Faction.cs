using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace RTSSanGuo
{
    //所有功能指令都在这Entity ，如果写到其他Component 也要这里Wrap 一下
    public  class Faction:EntityBase
    {
        public Person leaderPerson;
        public Color factionColor;
        public bool isPlayer;

        //子
        public Dictionary<int, Section> dic_section = new Dictionary<int, Section>();
    }
}
