using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace RTSSanGuo.Data
{
    
    public class DBeiDongToPbuildingSkill : DBase
    {
        public string imgName;
        public List<int> affectBuildingIDList = new List<int>();
        public float affectPercent = 0.1f;
    }
    

}
