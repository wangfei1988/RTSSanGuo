#define Test
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
namespace RTSSanGuo
{
    public enum EResType {Troop=1,Pbuilding,Wbuilding}
    public class ResMgr :MonoBehaviour
    {
        public static ResMgr Instacne;
        private void Awake()
        {
            if (Instacne == null)
                Instacne = this;
            else
                Debug.LogError("more than one instance");
        }
        
        public Dictionary<int, GameObject> dic_TroopPrefab = new Dictionary<int, GameObject>();
        public int[] troopPrefabIDArray;
        public GameObject[] troopPrefabArray;
        private void Start()
        {
            for (int i = 0; i < troopPrefabIDArray.Length; i++) {
                dic_TroopPrefab.Add(troopPrefabIDArray[i], troopPrefabArray[i]);
            }             
            StartCoroutine(LoadResData());
        }

        public bool hasInitAllData = false;
        private CSVFile csvfile=null;
        private IEnumerator LoadResData() {
            string filePath = PathTool.DataFileRootFold + "/common/Res.csv";
            csvfile = new CSVFile();
            csvfile.ReadCsv(filePath);
            foreach (string[] arr in csvfile.valueLines) {
                if (arr.Length != 7) continue;
                int id = int.Parse(arr[0]);
                string alias =arr[1];
                EResType type = (EResType) int.Parse(arr[2]);
                bool inInspector = bool.Parse(arr[3]);
                string respath=arr[4];  // Resouce.load 时路径
                string bundlepath=arr[5];
                string inbundlepath=arr[6];
                if (inInspector) continue;//已经在Inspector,不需要加载
            }
            hasInitAllData = true;
            yield return null;
        }

    }
}
