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
#if Test
        public int[] troopPrefabIDArray;
        public GameObject[] troopPrefabArray;
#endif
        private void Start()
        {
#if Test 
            for (int i = 0; i < troopPrefabIDArray.Length; i++) {
                dic_TroopPrefab.Add(troopPrefabIDArray[i], troopPrefabArray[i]);
            }
#endif
            StartCoroutine(LoadResData());
            
        }

        public bool hasInitAllData = false;
        private CSVFile resCsvfile=null;
        private IEnumerator LoadResData() {
            string filePath = PathTool.DataFileRootFold + "/common/Res.csv";
            resCsvfile = new CSVFile();
            resCsvfile.ReadCsv(filePath);
            foreach (string[] arr in resCsvfile.valueLines) {
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
