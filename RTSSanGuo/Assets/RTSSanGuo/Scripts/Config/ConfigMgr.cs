using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RTSSanGuo
{
    public class ConfigMgr     {

        public static  ConfigMgr _instance = new ConfigMgr(); //这些在Awake之前执行
        public ConfigMgr Instance {
            get {
                return _instance;
            }
        }       
        public ConfigMgr() { }              
 
    }
}