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

        /*******wrap basic本身 ************/        
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

        /******一级子对象（id直接记录在data）+ 多级子对象（通过级联获取）******/
        public Dictionary<int, CityBuilding> Dic_City
        {
            get
            {
                if (!DataMgr.Instacne.dataPrepared)
                    LogTool.LogError("data not prepared");
                Dictionary<int, CityBuilding> dic = new Dictionary<int, CityBuilding>();
                foreach (int cityid in data.idlist_city)
                {
                    if (EntityMgr.Instacne.dic_City.ContainsKey(cityid))
                    {
                        dic.Add(cityid, EntityMgr.Instacne.dic_City[cityid]);
                    }
                }
                return dic;
            }
        }

        /******一级父对象（id在子对象有，但是是反向推算） +多级父对象（通过级联获取）******/
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

        /******其他Wrap******/

        #endregion
        //初始化设置的
        public bool isPlayer;

        
        
    }
}
