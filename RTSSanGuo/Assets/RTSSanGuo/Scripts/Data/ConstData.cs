using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace RTSSanGuo.Data
{
    public class ConstData
    {
        public static string AppRootFold {
            get { return ""; }
        }
        public static string DataFileRootFold
        {
            get { return ""; }
        }
        public static string ResRootFold
        {
            get { return ""; }
        }
        public static string BundleRootFold
        {
            get { return ""; }
        } 
        
        //用户设置文件的存储文件夹
        public static string SettingFileFold {
            get { return AppRootFold+"/config/setting" ; }
        }
        // 通用设置路径 对所有剧本存档有效
        public static string CommonFileFold
        {
            get { return AppRootFold + "/config/setting"; }
        }

        //剧本路径
        public static string ScenerialFileFold
        {
            get { return AppRootFold + "/config/scenerial"; }
        }

        //存档路径
        public static string SaveFileFold
        {
            get { return AppRootFold + "/config/save"; }
        }

    }
}
