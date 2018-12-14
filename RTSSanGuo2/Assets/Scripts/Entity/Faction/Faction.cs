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

        #region wrap data

        private DFaction data;
        public DFaction Data {
            set { data = value; }
        }
        /*******wrap basic ************/
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
        public Dictionary<int, Section> Dic_Section {
            get {
                if (!DataMgr.Instacne.dataPrepared)
                    LogTool.LogError("data not prepared");
                Dictionary<int, Section> dic = new Dictionary<int, Section>();
                foreach(int sectionid in data.idlist_section){
                    if (EntityMgr.Instacne.dic_Section.ContainsKey(sectionid)) {
                        dic.Add(sectionid, EntityMgr.Instacne.dic_Section[sectionid]);
                    }
                }
                return dic;
            }
        }

        /******一级父对象（id在子对象有，但是是反向推算） +多级父对象（通过级联获取）******/

        /******其他Wrap******/

        #endregion

        public bool isPlayer;
    }
}
