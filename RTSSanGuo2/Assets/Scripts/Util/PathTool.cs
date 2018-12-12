using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
namespace RTSSanGuo
{
    public class PathTool
    {
        public static string AppRootFold {
            get { return Application.dataPath; }
        }
        public static string DataFileRootFold
        {
            get { return AppRootFold +"/data"; }
        }
        public static string LogFileRootFold
        {
            get { return AppRootFold +"/log"; }
        }
        public static string ResRootFold
        {
            get { return AppRootFold + "/res"; }
        }
        public static string BundleRootFold
        {
            get { return AppRootFold + "/bundle"; ; }
        }          
    }
}
