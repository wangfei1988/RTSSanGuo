﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;
namespace RTSSanGuo
{
   
   //用户放置Building
   public  class BuildingPlaceMgr:MonoBehaviour
   {
        public static BuildingPlaceMgr Instance = null;
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
