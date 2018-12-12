using System;
using System.Collections.Generic;
using System.Text;
using RtsFrameWork.Tool.FileTool;
namespace RTSSanGuo.Data
{
    public  class DGraphicBase:DBase
    {
        public string resPath;//prefab在Resource路径  有多个状态只能成为子物体，
        public string bundlePath;
        public string inBundlePath;
    }
}
