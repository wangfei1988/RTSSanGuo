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

        #region wrap data
        //本身
        public override int ID
        {
            get { return data.id; }
        }

        public override string Alias
        {
            get { return data.alias; }
        }

        public override string ShortDesc
        {
            get { return data.shortdesc; }
        }
        public override string FullDesc
        {
            get { return data.fulldesc; }
        }

        //一级子对象（记录在data）+多级子对象


        //一级父对象（没有记录在data，但是加载data时反向推算） +多级父对象   删除时需要删除本身及一级父对象的子对象索引
        public Faction ParentFaction {
            get {
                if (!DataMgr.Instacne.dataPrepared)
                    LogTool.LogError("data not prepared");
                int factionid = data.parentid_faction;
                if (factionid != -1 && EntityMgr.Instacne.dic_Section.ContainsKey(factionid))
                {
                    return EntityMgr.Instacne.dic_Faction[factionid];
                }
                else
                {
                    LogTool.LogError("can not find faction " + factionid);
                    return null;
                }
            }
        }

        #endregion
        public Person leaderPerson;
        //public Color sectionColor;
        public bool isPlayer;

        //子
        public Dictionary<int, CityBuilding> dic_section = new Dictionary<int, CityBuilding>();
        //父
        
    }
}
