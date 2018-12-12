using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
namespace RTSSanGuo
{
    public class DataMgr :MonoBehaviour
    {
        public static DataMgr Instacne;
        private void Awake()
        {
            if (Instacne == null)
                Instacne = this;
            else
                Debug.LogError("more than one instance");
            StartCoroutine(LoadEsscentialData());
        }

        //存档和Sceneria数据
        public CSVFile saveFile = null;
        public Dictionary<int, DSaveData> dic_Save = new Dictionary<int, DSaveData>();
        public CSVFile scenerialFile = null;
        public Dictionary<int, DScenerialData> dic_Scenerial = new Dictionary<int, DScenerialData>();

        //先加载Type数据，也就是Prefab数据，不会再Play过程中改变的
        public CSVFile troopTypeFile = null;
        public Dictionary<int, DTroopType> dic_TroopType = new Dictionary<int, DTroopType>();



        public Dictionary<int, DTroop> dic_Troop = new Dictionary<int, DTroop>();
        private CSVFile cityFile = null;
        public Dictionary<int, DCityBuilding> dic_City = new Dictionary<int, DCityBuilding>();
        private CSVFile sectionFile = null;
        public Dictionary<int, DSection> dic_Section = new Dictionary<int, DSection>();
        private CSVFile factionFile = null;
        public Dictionary<int, DFaction> dic_Faction = new Dictionary<int, DFaction>();

        public bool hasInitAllData = false;// 初始化基础数据
        public IEnumerator LoadEsscentialData() {

            //首先加载不可变更数据
            string filePath = PathTool.DataFileRootFold + "/common/troopType.csv";
            troopTypeFile = new CSVFile();
            troopTypeFile.ReadCsv(filePath);
            dic_TroopType.Clear();
            foreach (string[] arr in troopTypeFile.valueLines)
            {
                if (arr.Length != 9) continue;
                DTroopType troopType = new DTroopType();
                troopType.id = int.Parse(arr[0]);
                troopType.alias = arr[1];
                troopType.shortdesc = arr[2];
                troopType.fulldesc = arr[3];
                troopType.baseatk = int.Parse(arr[4]);
                troopType.basedef = int.Parse(arr[5]);
                troopType.baseremoteatkrange = int.Parse(arr[6]);
                troopType.basemovespeed = float.Parse(arr[7]);
                troopType.resid = int.Parse(arr[8]);
                dic_TroopType.Add(troopType.id, troopType);
            }
            yield return null;

            filePath = PathTool.DataFileRootFold + "/save/save.csv";
            saveFile = new CSVFile();
            saveFile.ReadCsv(filePath);
            dic_Save.Clear();
            foreach (string[] arr in saveFile.valueLines)
            {
                if (arr.Length != 12) continue;
                DSaveData savedata = new DSaveData();
                savedata.id = int.Parse(arr[0]);
                savedata.alias = arr[1];
                savedata.shortdesc = arr[2];
                savedata.fulldesc = arr[3];
                savedata.id_playerFaction = int.Parse(arr[4]);
                savedata.id_playerSection = int.Parse(arr[5]);
                savedata.year = int.Parse(arr[6]);
                savedata.month = int.Parse(arr[7]);
                savedata.day = int.Parse(arr[8]);
                savedata.season = int.Parse(arr[9]);
                savedata.savetime =  arr[10];
                savedata.subfold = arr[11];
                dic_Save.Add(savedata.id, savedata);
            }
            yield return null;
            filePath = PathTool.DataFileRootFold + "/scenerial/scenerial.csv";
            scenerialFile = new CSVFile();
            scenerialFile.ReadCsv(filePath);
            dic_Scenerial.Clear();
            foreach (string[] arr in scenerialFile.valueLines)
            {
                if (arr.Length != 9) continue;
                DScenerialData scedata = new DScenerialData();
                scedata.id = int.Parse(arr[0]);
                scedata.alias = arr[1];
                scedata.shortdesc = arr[2];
                scedata.fulldesc = arr[3];                
                scedata.year = int.Parse(arr[4]);
                scedata.month = int.Parse(arr[5]);
                scedata.day = int.Parse(arr[6]);
                scedata.season = int.Parse(arr[7]);
                scedata.subfold = arr[8];
                dic_Scenerial.Add(scedata.id, scedata);
            }
            yield return null;
            hasInitAllData = true;
        }

        public int loadPercent = 0;
        public bool dataPrepared = false;
        public DSaveData selSaveData = null;
        public bool LoadSaveData(int id) {
            dataPrepared = false;
            if (dic_Save.ContainsKey(id))
            {
                selSaveData = dic_Save[id];
                string fold = PathTool.DataFileRootFold+"/save/"+ selSaveData.subfold;
                StartCoroutine(LoadSaveData(fold));
                return true;
            }
            else {
                loadPercent = 0;
                LogTool.LogError("can not find save id" +id);
                return false;
            }           
        }

        //Data 直接保存的是一级子对象id  以及反向推算出父对象id 
        // 子对象的子对象 以及父对象的父对象没有记录，只能通过间接获取
        private IEnumerator LoadSaveData(string fold) {
            LoadTroop(fold);
            loadPercent = 5;
            yield return null;
            LoadCity(fold);
            loadPercent = 10;
            yield return null;
            LoadSection(fold);
            loadPercent = 15;
            yield return null;
            LoadFaction(fold);
            loadPercent = 20;
            yield return null;
            loadPercent = 100;

            dataPrepared = true;
            yield return null;
        }


        private void LoadTroop(string fold) {

        }


        private void LoadCity(string fold )
        {
            string filePath = fold + "/city.csv";
            cityFile = new CSVFile();
            cityFile.ReadCsv(filePath);
            dic_City.Clear();
            foreach (string[] arr in cityFile.valueLines)
            {
                if (arr.Length != 16) {
                    LogTool.LogError("city arr.length" + arr.Length);
                    continue; }
                DCityBuilding city = new DCityBuilding();
                city.id = int.Parse(arr[0]);
                city.alias = arr[1];
                city.shortdesc = arr[2];
                city.fulldesc = arr[3];
                city.food = int.Parse(arr[4]);
                city.money = int.Parse(arr[5]);
                city.population = int.Parse(arr[6]);
                city.curhp = int.Parse(arr[7]);
                city.curtotalsoldiernum = int.Parse(arr[8]);
                city.mingxin = int.Parse(arr[9]);
                city.zhian = int.Parse(arr[10]);
                city.id_leadperson = int.Parse(arr[11]);
                city.idlist_pbuilding = CommonUtil.StringToListInt(arr[12],'#');
                city.idlist_troop = CommonUtil.StringToListInt(arr[13], '#'); //只保存一级子类
                foreach (int troopid in city.idlist_troop) //子对象的一级父类在这初始化
                {
                    if (troopid != -1 && dic_Troop.ContainsKey(troopid)) {
                        DTroop troop = DataMgr.Instacne.dic_Troop[troopid];
                        troop.parentid_city = city.id;
                    } 
                }
                city.idlist_person = CommonUtil.StringToListInt(arr[14], '#');
                city.idlist_freeperson = CommonUtil.StringToListInt(arr[15], '#');
                dic_City.Add(city.id, city);               
            }
            EntityMgr.Instacne.InitAllCityData();
        }

        private void LoadSection(string fold) {
            string filePath = fold + "/section.csv";
            sectionFile = new CSVFile();
            sectionFile.ReadCsv(filePath);
            dic_Section.Clear();
            foreach (string[] arr in sectionFile.valueLines)
            {
                if (arr.Length != 6) {
                    LogTool.LogError("section arr.length" + arr.Length);
                    continue;
                } 
                DSection dsection = new DSection();
                dsection.id = int.Parse(arr[0]);
                dsection.alias = arr[1];
                dsection.shortdesc = arr[2];
                dsection.fulldesc = arr[3];
                dsection.id_leadperson = int.Parse(arr[4]);
                dsection.idlist_city = CommonUtil.StringToListInt(arr[5], '#'); //只保存一级子类      
                foreach (int cityid in dsection.idlist_city) //子对象的一级父类在这初始化
                {
                    if (cityid != -1 && dic_City.ContainsKey(cityid))
                    {
                        DCityBuilding city = dic_City[cityid];
                        city.parentid_section = dsection.id;
                    }
                    else {
                        LogTool.LogError("can not find city id" + cityid);
                    }
                }                
                dic_Section.Add(dsection.id, dsection);                
                EntityMgr.Instacne.AddSectionFromData(dsection.id);
            }
        }

        private void LoadFaction(string fold) {
            string filePath = fold + "/faction.csv";
            factionFile = new CSVFile();
            factionFile.ReadCsv(filePath);
            dic_Faction.Clear();
            foreach (string[] arr in factionFile.valueLines)
            {
                if (arr.Length != 7)
                {
                    LogTool.LogError("faction arr.length" + arr.Length);
                    continue;
                }
                DFaction faction = new DFaction();
                faction.id = int.Parse(arr[0]);
                faction.alias = arr[1];
                faction.shortdesc = arr[2];
                faction.fulldesc = arr[3];
                faction.id_leadperson = int.Parse(arr[4]);
                faction.idlist_section = CommonUtil.StringToListInt(arr[5], '#'); //只保存一级子类
                foreach (int sectionid in faction.idlist_section) //子对象的一级父类在这初始化
                {
                    if (sectionid != -1 && dic_Section.ContainsKey(sectionid))
                    {
                        DSection section = dic_Section[sectionid];
                        section.parentid_faction = faction.id;
                    }
                }
                faction.idlist_wbuilding = CommonUtil.StringToListInt(arr[6], '#');
                dic_Faction.Add(faction.id, faction);
                EntityMgr.Instacne.AddFactionFromData(faction.id);
            }
        }


        public DTroop AddNewTroopData(int trooptypeid) {
            if (dic_TroopType.ContainsKey(trooptypeid))
            {
                DTroop troop = new DTroop();
                int id = 1;
                for (int i = 1; i < 9999; i++)
                {
                    if (dic_Troop.ContainsKey(i))
                        id = i;
                }
                troop.id = id;
                troop.id_trooptype = trooptypeid;
                dic_Troop.Add(id, troop);
                return troop;
            }
            else {
                LogTool.LogError("can not find typeid" + trooptypeid);
                return null;
            }

        }
    }
}
