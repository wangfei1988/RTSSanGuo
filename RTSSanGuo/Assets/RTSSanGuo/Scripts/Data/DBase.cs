using System;
using System.Collections.Generic;
using System.Text;
using RtsFrameWork.Tool.FileTool;
namespace RTSSanGuo.Data
{
    public  class DBase:ICsvData
    {
        public int id;
        public string name;
        public string shortDesc;
        public string fullDesc;

        public virtual bool InitFrom(string[] values)
        {
            return false;
        }
    }
}
