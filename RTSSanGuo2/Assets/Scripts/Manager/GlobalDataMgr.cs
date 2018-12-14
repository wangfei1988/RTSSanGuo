#define TEestttt

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace RTSSanGuo
{
         
    //为什么不放到DataMagaer --只是方便阅读和查看
    public class GlobalDataMgr : MonoBehaviour
    {
        public static GlobalDataMgr Instacne;
        private void Awake()
        {
            if (Instacne == null)
                Instacne = this;
            else
                Debug.LogError("more than one instance");
        }

        public Dictionary<string, int> factionRelation = new Dictionary<string, int>();
        //key  faction1_faction2
        public void LoadSave(string fold) {

        }

 

    }
}
