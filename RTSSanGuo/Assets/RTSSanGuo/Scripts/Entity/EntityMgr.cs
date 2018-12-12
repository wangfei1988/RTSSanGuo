using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;
namespace RTSSanGuo
{
   
   public  class EntityMgr:MonoBehaviour
    {
        public static EntityMgr Instance = null; //其实这个不需要继承Monobehavior ，但是唯一统一，还是继承。几十个Monobehavior 不会影响啥性能
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            if (Instance == null)
                Instance = this;
            else
                Debug.LogError("more than one instance");
        }


    }
}
