using System;
using System.Collections.Generic;
using System.Text;

namespace RTSSanGuo
{
    public class CommonUtil
    {
        public static List<int> StringToListInt(string str ,char sep) {
            List<int> list = new List<int>();
            if (str.Trim()=="") return list;

            string[] strarr= str.Split(sep);
            foreach (string strdata in strarr) {
                list.Add(int.Parse(strdata.Trim()));
            }
            return list;
        }
        

        //合并去除重复的
        public static List<int> Combine(List<int> list1, List<int> list2)
        {
            List<int> list = list1;
            if (list2 == null || list2.Count < 1)
                return list;
            foreach (int i in list2)
            {
                if (!list.Contains(i))
                    list.Add(i);
            }
            return list;
        }
        


    }
}
