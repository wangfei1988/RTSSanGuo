using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace RTSSanGuo
{
    //所有功能指令都在这Entity ，如果写到其他Component 也要这里Wrap 一下
    public  class Person:EntityBase
    {

        //父
        public Faction parentFaction;
        public Section parentSection;
        public CityBuilding parentCity;
        public ProductionBuilding parentPBuilding; //这2个父互斥 只能有一个非null
        public Troop parentTroop;

    }
}
