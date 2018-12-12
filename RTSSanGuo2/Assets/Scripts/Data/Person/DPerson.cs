using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace RTSSanGuo
{
    public enum ETroopType { BuBing=1,QiBing,GongBing,GongChen,ShuiBing,None } //None 没有任何武器

    [Serializable]
    public  class DPerson:DataBase
    {
        public string firstname;
        public string secondname;
        public string thirdname;
        public int bornyear;
        public int bornmonth;
        public int borndate; //未成年之前 不会四处移动，而且是free people
        public bool isfreeperson;//没有任何faction 雇佣
        public bool canhire; //蛮族不可以
        public int curleftexp; //当前剩下exp

        public int tong;
        public int wu;
        public int zhi;
        public int zhen;
        public int mei;

        public int level_bubing;
        public int level_qibing;
        public int level_gongbing;
        public int level_shuibing;
        public int level_gongcheng;       
        
       // public List<int> idlist_ 太多内容了，先展示取消这一快

       
        public int parentid_city;//一级父类---不是保存，而是逆向计算.这里有三个一级父类。 其中parentid_troop  parentid_pbuilding互斥
        public int parentid_troop;
        public int parentid_pbuilding;


    }
}
