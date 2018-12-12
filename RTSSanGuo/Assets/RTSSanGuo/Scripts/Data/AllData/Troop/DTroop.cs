using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace RTSSanGuo.Data
{
    public class DTroop:DBase
    {               
        public int typeID;
        public int origSoldierNum;
        public int wondedSoldierNum;
        public int curSoldierNum;
       // public int fromCityID;
        public int money;
        public int food;
        public int siqi;//士气        
        public int id_Hero1;
        public int id_Hero2;
        public int id_Hero3;        
        public float skill1CoolTime;
        public float skill2CoolTime;
        public float skill3CoolTime;
        public int id_skill1;
        public int id_skill2;
        public int id_skill3;
        public bool _isHiden;
        
        //AI相关
        public int targetTroopID;
        public int temptargetTroopID;//临时切换的
        public int targetWBuildingID;
        public int targetPBuildingID;

        //public int targetCityID;
        //public int targetSectionID;//-1代表没有 这些没办法attack 只能由上级决定
        //public int targetSectionID;//-1代表没有
        //public int hasTagetPos;//下面无法用-1代表没有，不得添加一个新的变量
        //public int tagetHexX;
        //public int targetHexY;

         
    }
    

}
