using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace RTSSanGuo
{
    //所有功能指令都在这Entity ，如果写到其他Component 也要这里Wrap 一下
    public  class Section:EntityBase
    {
        public DSection data;

        public Person leaderPerson;
        //public Color sectionColor;
        public bool isPlayerControl;

        //子
        public Dictionary<int, CityBuilding> dic_section = new Dictionary<int, CityBuilding>();
        //父
        public Faction parentFaction;
    }
}
