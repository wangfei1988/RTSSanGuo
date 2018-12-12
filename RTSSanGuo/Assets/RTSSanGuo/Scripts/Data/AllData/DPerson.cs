using System.Collections.Generic;
using System;
using System.IO;
namespace RTSSanGuo.Data
{
    public class DPerson
    {   
        public string firstName;//赵
        public string secondName;//云
        public string thirdName;//子龙        
        public int headImgId; //通用id
        public bool man;       
        public int birthYear;
        public int auldtYear;
        public int dieYear;
        public List<int> id_BeiDongToPbuildingSkillList = new List<int>();// 
        public List<int> id_BeiDongToTroopSkillList = new List<int>();//      
        public List<int> id_ZhuDongSkillList = new List<int>(); // 

        public int atrr_tong;
        public int atrr_wu;
        public int atrr_zhi;
        public int atrr_zhen;
        public int atrr_mei;

        public int shiXing_Bu; // 
        public int shiXing_Qi; //弓骑兵--算gong
        public int shiXing_Gong;
        public int shiXing_Gongcheng;
        public int shiXing_Water;        
        public int curLeftExp;// 时间 +  委任加速+  职位加速+  （重大事件+  战斗胜利+几百 ）
 
    } 
}
