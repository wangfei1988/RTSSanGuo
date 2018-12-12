using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace RTSSanGuo
{
    //所有功能指令都在这Entity ，如果写到其他Component 也要这里Wrap 一下
    public  class ProductionBuilding:Building
    {
        //Audio: 这些都是一眼搞得，还是放全局
        //public AudioClip SelectionAudio; //Audio played when the building is selected.
        //public AudioClip UpgradeLaunchedAudio; //When the building upgrade starts.
        //public AudioClip UpgradeCompletedAudio; //When the building has been upgraded.

        public Person leaderPerson;

        //父
       // public Faction parentFaction;
       // public Section parentSection;
        public CityBuilding parentCity;

    }
}
