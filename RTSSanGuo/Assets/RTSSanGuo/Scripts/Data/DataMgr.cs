using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;
namespace RTSSanGuo.Data
{
   
   public  class DataMgr
    {
        //这个其实不需要继承MonoBehavior，但是为了和GameObjManager统一，还是继承
        //对性能不会有多大影响
        public static DataMgr _instance = new DataMgr(); //这些在Awake之前执行
        public static DataMgr Instance
        {
            get
            {
                return _instance;
            }
        }



      
         
        public Dictionary<int, DBeiDongToPbuildingSkill> dic_BeiDongToPbuildingSkill = new Dictionary<int, DBeiDongToPbuildingSkill>();
        //所有被动内政技能列表
        public Dictionary<int, DBeiDongToTroopSkill> dic_BeiDongToTroopSkill = new Dictionary<int, DBeiDongToTroopSkill>();
        //所有的主动技能列表(战法)
        public Dictionary<int, DZhuDongSkill> dic_ZhuDongSkill = new Dictionary<int, DZhuDongSkill>();
       // //所有的主动技能列表(计谋)
       // public Dictionary<int, Data_PZJiMouSkill> dic_PZJiMouSkill = new Dictionary<int, Data_PZJiMouSkill>();
       // //所有战法列表
       // public Dictionary<int, Data_ZhanFa> dic_ZhanFa = new Dictionary<int, Data_ZhanFa>();
       // public Dictionary<int, Data_TroopNAction> dic_DataTroopNAction = new Dictionary<int, Data_TroopNAction>();
        
       // //不可以被Player在Play的过程中改变
       // public Dictionary<EPBuildingType, Data_PBuildingType> dic_PbuildingType = new Dictionary<EPBuildingType, Data_PBuildingType>();
       // public Dictionary<EWBuildingType, Data_WBuildingType> dic_WbuildingType = new Dictionary<EWBuildingType, Data_WBuildingType>();
       //// public Dictionary<ENormalZhanFaType, List<int>> dic_NormalZhanFaDic = new Dictionary<ENormalZhanFaType, List<int>>();
       // public Dictionary<int, Data_TroopType> dic_TroopType = new Dictionary<int, Data_TroopType>();
       // public Dictionary<int, string> dic_LoadingTips = new Dictionary<int, string>();

       // //Application.dataPath
       // //Unity Editor: <path to project folder>/Assets
       // //Win/Linux player: <path to executablename_Data folder> (note that most Linux installations will be case-sensitive!)
       // public IEnumerator InitCommonData() {
       //     //加载顺序无关紧要
       //     dic_PBBatSkill = Data_PBBatSkill.LoadFromCsv(Application.dataPath+@"\DataFile\CommonData");
       //     yield return null;
       //     dic_PBNeiSkill = Data_PBNeiSkill.LoadFromCsv(Application.dataPath + @"\DataFile\CommonData");
       //     yield return null;
       //     dic_PZSuperAttackSkill = Data_PZSuperAttackSkill.LoadFromCsv(Application.dataPath + @"\DataFile\CommonData");
       //     yield return null;
       //     dic_PZJiMouSkill = Data_PZJiMouSkill.LoadFromCsv(Application.dataPath + @"\DataFile\CommonData");
       //     yield return null;
       //     dic_ZhanFa = Data_ZhanFa.LoadFromCsv(Application.dataPath + @"\DataFile\CommonData");
       //     yield return null;
       //     dic_DataTroopNAction =  Data_TroopNAction.LoadFromCsv(Application.dataPath + @"\DataFile\CommonData");
       //     yield return null;
       //     dic_PbuildingType = Data_PBuildingType.LoadFromCsv(Application.dataPath + @"\DataFile\CommonData");
       //     yield return null;
       //     dic_WbuildingType = Data_WBuildingType.LoadFromCsv(Application.dataPath + @"\DataFile\CommonData");
       //     yield return null;
       //     dic_TroopType = Data_TroopType.LoadFromCsv(Application.dataPath + @"\DataFile\CommonData");
       //     yield return null;
       //     string csvFilePath = Application.dataPath + @"\DataFile\CommonData\Dic_LoadingTips.csv";
       //     if (File.Exists(csvFilePath))
       //     {
       //         using (StreamReader streamReader = new StreamReader(csvFilePath, System.Text.Encoding.UTF8))
       //         {
       //             using (CsvReader csvReader = new CsvReader(streamReader))
       //             {
       //                 csvReader.Configuration.IsHeaderCaseSensitive = false;
       //                 //标题行匹配忽略大小写
       //                 csvReader.Configuration.AllowComments = true;
       //                 csvReader.Configuration.IgnoreReferences = true;
       //                 // 关联映射很复杂，建议取消
       //                 while (csvReader.Read())
       //                 {
       //                     int id = csvReader.GetField<int>("ID");
       //                     string content = csvReader.GetField<string>("Content");
       //                     dic_LoadingTips.Add(id, content);
       //                 }
       //             }
       //         }
       //     }
       //     else
       //     {
       //         Debug.LogError("Not Exists" + csvFilePath);
       //     }
       //     yield return null;
       // }

       // //最子类的
       // public Dictionary<int, Data_Person> dic_person = new Dictionary<int, Data_Person>();
       // public Dictionary<int, Dictionary<int,int>> p2pRelation = new Dictionary<int, Dictionary<int, int>>(); // <personIndexID, relation> 0 的不记录
       // //友好度  不是相互的  双向 所以要写两份
       // public Dictionary<int, List<int>> p2pBrother = new Dictionary<int, List<int>>();
       // //是相互的 双向 一份
       // public Dictionary<int, Dictionary<int, int>> p2fRelation = new Dictionary<int, Dictionary<int, int>>(); // <factionIndexID, relation> 0 的不记录
       // //单向 只有p2f  一份
       // public Dictionary<int, Dictionary<int, int>> f2fRelation = new Dictionary<int, Dictionary<int, int>>();
       // //友好度  不是相互的  双向  要写双份
       // public Dictionary<int, Data_Troop> dic_troop = new Dictionary<int, Data_Troop>();
       // public Dictionary<int, Data_PBuilding> dic_pbuilding = new Dictionary<int, Data_PBuilding>();
       // //player的过程中会new
       // public Dictionary<int, Data_WBuilding> dic_wbuilding = new Dictionary<int, Data_WBuilding>();
       // //player的过程中不会new        
       // public Dictionary<int, Data_City> dic_city = new Dictionary<int, Data_City>();
       // public Dictionary<int, Data_Section> dic_section = new Dictionary<int, Data_Section>();
       // public Dictionary<int, Data_Faction> dic_faction = new Dictionary<int, Data_Faction>();

       // public void LoadScenerial(string Scencefold , Action onLoadComplete = null) {
       //    StartCoroutine(InitData(Scencefold ,onLoadComplete));
       // }


       // public IEnumerator InitData(string fold,Action onLoadComplete=null) {
       //     //hasInitData = false;
       //     Win_Loading loading = UIManager.Instance.GetUI<Win_Loading>(EUIID.Win_Loading);
       //     //加载顺序很重要，否则Init方法会失败
       //     dic_person = Data_Person.LoadFromCsv(Application.dataPath + fold);
       //     yield return null;
       //     foreach (Data_Person person in dic_person.Values.ToList<Data_Person>()) {
       //         person.InitData();
       //     }
       //     loading.UpdatePer(0.15f);
       //     yield return null;
       //     string csvFilePath = Application.dataPath + fold + @"\p2pRelation.csv";
       //     if (File.Exists(csvFilePath))
       //     {
       //         using (StreamReader streamReader = new StreamReader(csvFilePath, System.Text.Encoding.UTF8))
       //         {
       //             using (CsvReader csvReader = new CsvReader(streamReader))
       //             {
       //                 csvReader.Configuration.IsHeaderCaseSensitive = false;
       //                 //标题行匹配忽略大小写
       //                 csvReader.Configuration.AllowComments = true;
       //                 csvReader.Configuration.IgnoreReferences = true;
       //                 // 关联映射很复杂，建议取消
       //                 while (csvReader.Read())
       //                 {
       //                     int idFrom = csvReader.GetField<int>("idFrom");
       //                     int idTo = csvReader.GetField<int>("idTo");
       //                     int value = csvReader.GetField<int>("value");
       //                     if (!p2pRelation.ContainsKey(idFrom)) {
       //                         p2pRelation.Add(idFrom, new Dictionary<int, int>());
       //                     }
       //                     Dictionary<int, int> relation = p2pRelation[idFrom];
       //                     if (!relation.ContainsKey(idTo))
       //                     {
       //                         relation.Add(idTo, value);
       //                     }
       //                     else {
       //                         relation[idTo] = value;
       //                     }
       //                 }
       //             }
       //         }
       //     }
       //     else
       //     {
       //         Debug.LogError("Not Exists" + csvFilePath);
       //     }
       //     yield return null;
       //     csvFilePath = Application.dataPath + fold + @"\p2pBrother.csv";
       //     if (File.Exists(csvFilePath))
       //     {
       //         using (StreamReader streamReader = new StreamReader(csvFilePath, System.Text.Encoding.UTF8))
       //         {
       //             using (CsvReader csvReader = new CsvReader(streamReader))
       //             {
       //                 csvReader.Configuration.IsHeaderCaseSensitive = false;
       //                 //标题行匹配忽略大小写
       //                 csvReader.Configuration.AllowComments = true;
       //                 csvReader.Configuration.IgnoreReferences = true;
       //                 // 关联映射很复杂，建议取消
       //                 while (csvReader.Read())
       //                 {
       //                     int id1 = csvReader.GetField<int>("id1");
       //                     int id2 = csvReader.GetField<int>("id2");

       //                     if (!p2pBrother.ContainsKey(id1))
       //                     {
       //                         p2pBrother.Add(id1, new List<int>());
       //                     }
       //                     List<int> brotherList = p2pBrother[id1];
       //                     if (!brotherList.Contains(id2))
       //                     {
       //                         brotherList.Add(id2);
       //                     }

       //                     if (!p2pBrother.ContainsKey(id2))
       //                     {
       //                         p2pBrother.Add(id2, new List<int>());
       //                     }
       //                     List<int> brotherList2 = p2pBrother[id2];
       //                     if (!brotherList2.Contains(id1))
       //                     {
       //                         brotherList2.Add(id1);
       //                     }

       //                 }
       //             }
       //         }
       //     }
       //     else
       //     {
       //         Debug.LogError("Not Exists" + csvFilePath);
       //     }
       //     yield return null;
       //     csvFilePath = Application.dataPath + fold + @"\p2fRelation.csv";
       //     if (File.Exists(csvFilePath))
       //     {
       //         using (StreamReader streamReader = new StreamReader(csvFilePath, System.Text.Encoding.UTF8))
       //         {
       //             using (CsvReader csvReader = new CsvReader(streamReader))
       //             {
       //                 csvReader.Configuration.IsHeaderCaseSensitive = false;
       //                 //标题行匹配忽略大小写
       //                 csvReader.Configuration.AllowComments = true;
       //                 csvReader.Configuration.IgnoreReferences = true;
       //                 // 关联映射很复杂，建议取消
       //                 while (csvReader.Read())
       //                 {
       //                     int idFrom = csvReader.GetField<int>("idFrom");
       //                     int idTo = csvReader.GetField<int>("idTo");
       //                     int value = csvReader.GetField<int>("value");
       //                     if (!p2fRelation.ContainsKey(idFrom))
       //                     {
       //                         p2fRelation.Add(idFrom, new Dictionary<int, int>());
       //                     }
       //                     Dictionary<int, int> relation = p2fRelation[idFrom];
       //                     if (!relation.ContainsKey(idTo))
       //                     {
       //                         relation.Add(idTo, value);
       //                     }
       //                     else
       //                     {
       //                         relation[idTo] = value;
       //                     }
       //                 }
       //             }
       //         }
       //     }
       //     else
       //     {
       //         Debug.LogError("Not Exists" + csvFilePath);
       //     }
       //     yield return null;
       //     csvFilePath = Application.dataPath + fold + @"\f2fRelation.csv";
       //     if (File.Exists(csvFilePath))
       //     {
       //         using (StreamReader streamReader = new StreamReader(csvFilePath, System.Text.Encoding.UTF8))
       //         {
       //             using (CsvReader csvReader = new CsvReader(streamReader))
       //             {
       //                 csvReader.Configuration.IsHeaderCaseSensitive = false;
       //                 //标题行匹配忽略大小写
       //                 csvReader.Configuration.AllowComments = true;
       //                 csvReader.Configuration.IgnoreReferences = true;
       //                 // 关联映射很复杂，建议取消
       //                 while (csvReader.Read())
       //                 {
       //                     int idFrom = csvReader.GetField<int>("idFrom");
       //                     int idTo = csvReader.GetField<int>("idTo");
       //                     int value = csvReader.GetField<int>("value");
       //                     if (!f2fRelation.ContainsKey(idFrom))
       //                     {
       //                         f2fRelation.Add(idFrom, new Dictionary<int, int>());
       //                     }
       //                     Dictionary<int, int> relation = f2fRelation[idFrom];
       //                     if (!relation.ContainsKey(idTo))
       //                     {
       //                         relation.Add(idTo, value);
       //                     }
       //                     else
       //                     {
       //                         relation[idTo] = value;
       //                     }
       //                 }
       //             }
       //         }
       //     }
       //     else
       //     {
       //         Debug.LogError("Not Exists" + csvFilePath);
       //     }
       //     loading.UpdatePer(0.2f);
       //     yield return null;
       //     dic_troop = Data_Troop.LoadFromCsv(Application.dataPath + fold);
       //     foreach (Data_Troop troop in dic_troop.Values.ToList<Data_Troop>())
       //     {
       //         troop.InitData();
       //     }
       //     yield return null;
       //     dic_pbuilding = Data_PBuilding.LoadFromCsv(Application.dataPath + fold);
       //     foreach (Data_PBuilding building in dic_pbuilding.Values.ToList<Data_PBuilding>()) {
       //         building.InitData();
       //     }
       //     yield return null;
       //     dic_wbuilding = Data_WBuilding.LoadFromCsv(Application.dataPath + fold);
       //     foreach (Data_WBuilding building in dic_wbuilding.Values.ToList<Data_WBuilding>())
       //     {
       //         building.InitData();
       //     }
       //     yield return null;
       //     dic_city = Data_City.LoadFromCsv(Application.dataPath + fold);
       //     foreach (Data_City city in dic_city.Values.ToList<Data_City>()) {
       //         city.InitData();
       //     }
       //     yield return null;
       //     dic_section = Data_Section.LoadFromCsv(Application.dataPath + fold);
       //     foreach (Data_Section section in dic_section.Values.ToList<Data_Section>())
       //     {
       //         section.InitData();
       //     }
       //     yield return null;
       //     dic_faction = Data_Faction.LoadFromCsv(Application.dataPath + fold);
       //     foreach (Data_Faction faction in dic_faction.Values.ToList<Data_Faction>())
       //     {
       //         faction.InitData();
       //     }
       //     loading.UpdatePer(0.3f);
       //     yield return null;           
       //     GameObjManager.Instance.LoadAllGameObj(); //这个不会执行效果            
       //     if (onLoadComplete != null)
       //         onLoadComplete();
       // }
       // //存档所在文件夹
       // [HideInInspector]
       // public San11Date hisDate; //历史时间
       // [HideInInspector]
       // public int turnCount;
       // [HideInInspector]
       // public string saveTime; //UnityEngine.Time  其实我们只需要string
       // [HideInInspector]
       // public int playerFactionId;
       // [HideInInspector]
       // public int playerLeaderId;
       // [HideInInspector]
       // public string info;
       // [HideInInspector]
       // public int cityCount;
       // [HideInInspector]
       // public bool complete;//存档是否完整

       // public void LoadSaveData(string fold) {
       //     FileIniDataParser parseConfig = new FileIniDataParser();
       //     parseConfig.Parser.Configuration.CommentString = "#";
       //     //Parse the ini file
       //     IniData saveData = parseConfig.ReadFile(fold + @"\save.ini");
       //     string  dateStr =saveData["Save"]["hisDate"].Trim();
       //     string[] strs = dateStr.Split('&');
       //     int year = int.Parse(strs[0]);
       //     int month = int.Parse(strs[1]);
       //     int day = int.Parse(strs[2]);
       //     hisDate = new San11Date(year, month, day);
       //     turnCount = int.Parse(saveData["Save"]["turnCount"].Trim());
       //     saveTime = saveData["Save"]["saveTime"].Trim();
       //     playerFactionId = int.Parse(saveData["Save"]["playerFactionId"].Trim());
       //     playerLeaderId = int.Parse(saveData["Save"]["playerLeaderId"].Trim());
       //     info =  saveData["Save"]["info"].Trim();
       //     cityCount = int.Parse(saveData["Save"]["cityCount"].Trim());
       //     complete = bool.Parse(saveData["Save"]["complete"].Trim());            
       // }

        

    }
}
