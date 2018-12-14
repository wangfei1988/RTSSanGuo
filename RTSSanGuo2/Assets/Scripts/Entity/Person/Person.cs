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
        private DPerson data;
        public DPerson Data { set { data = value; } }

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

        public  int Tong
        {
            get { return data.tong; }
        }
        public int Wu
        {
            get { return data.wu; }
        }
        public int Zhi
        {
            get { return data.zhi; }
        }
        public int Zhen
        {
            get { return data.zhen; }
        }
        public int Mei
        {
            get { return data.mei; }
        }
        public int Level_BuBing
        {
            get { return data.level_bubing; }
        }
        public int Level_QiBing
        {
            get { return data.level_qibing; }
        }
        public int Level_GongBing
        {
            get { return data.level_gongbing; }
        }
        public int Level_GongChen
        {
            get { return data.level_gongcheng; }
        }
        public int Level_ShuiBing
        {
            get { return data.level_shuibing; }
        }

        /******一级子对象列表（id直接记录在data）+ 多级子对象（通过级联获取）******/


        /******一级父对象（id在子对象有，但是是反向推算） +多级父对象（通过级联获取）******/
        public CityBuilding ParentCity
        {
            get
            {
                if (!DataMgr.Instacne.dataPrepared)
                    LogTool.LogError("data not prepared"); //修改父子关系在父类 idlist修改，而不是子类修改
                int city = data.parentid_city;
                if (city != -1 && EntityMgr.Instacne.dic_Section.ContainsKey(city))
                {
                    return EntityMgr.Instacne.dic_City[city];
                }
                else
                {
                    LogTool.LogError("can not find faction " + city);
                    return null;
                }
            }
        }

        public Section ParentSection
        {
            get
            {
                if (!DataMgr.Instacne.dataPrepared)
                    LogTool.LogError("data not prepared"); //修改父子关系在父类 idlist修改，而不是子类修改
                return ParentCity.ParentSection;
            }
        }
        public Faction ParentFaction
        {
            get
            {
                if (!DataMgr.Instacne.dataPrepared)
                    LogTool.LogError("data not prepared"); //修改父子关系在父类 idlist修改，而不是子类修改
                return ParentSection.ParentFaction;
            }
        }
        public Troop ParentTroop
        {
            get
            {
                if (!DataMgr.Instacne.dataPrepared)
                    LogTool.LogError("data not prepared"); //修改父子关系在父类 idlist修改，而不是子类修改
                int troopid = data.parentid_troop;
                if (troopid != -1 && EntityMgr.Instacne.dic_Section.ContainsKey(troopid))
                {
                    return EntityMgr.Instacne.dic_Troop[troopid];
                }
                else
                {
                    LogTool.LogError("can not find faction " + troopid);
                    return null;
                }
            }
        }

        /******其他Wrap******/

        #endregion
        
    }
}
