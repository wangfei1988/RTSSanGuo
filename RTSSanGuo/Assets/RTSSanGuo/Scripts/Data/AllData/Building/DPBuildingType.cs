using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
namespace RTSSanGuo.Data
{

    //是否声明这个枚举不是很重要，指示方便后面各种操作。直接用int不是很方便
    public enum EPBuildingType
    {
        HeiShi = 1,//  黑市
        ShiChang,//市场
        YuShi, //    鱼市  钱粮同时加 靠睡
        QianZhuang, //钱庄
        BigShiChang,// 科技

        NongChang,//农场
        YangZhiChang,//养殖场 钱粮同时加  
        LiangCang,//粮仓
        JunTunLong,//军屯农  科技


        YangMaChang,//养马场

        ZhengBinSuo,//征兵所  自动征兵 在兵力未达上限之前  必须有人才行 无人不工作  工作时-民心
        JunYin,//军营 兵力上限
        Training,//训练场   士气上限

        BinQiFang,//兵器坊   
        GongNuFang,//
        JunXieSuo,//器械所
        ZaoChuangSuo,//造船所 有就行

        JianYu,// + 民心 上限
        ZhaoXiangSuo,// 招贤所
        JiYiSuo,//技艺坊  +科技点数 全局
        AnMingSuo//安民所 + 明心恢复速度  -钱 -money 
    }

    //这些对象不是new出来的，而是从csv表格加载进来的
    public class DPBuildingType:DGraphicBase
    {
        public EPBuildingType id;        
        public string imgName;//这个路径固定，所以只要名字         
        public int baseMoneyPerHpLimitPointAdd; //添加hp上限 每一点hp 多少money
        public int baseHpBuildPerDay; //每一级都一样，只是后面血条更长  修复NoNeedMoney    Build和UpGrade需要,一次性消耗 必须满血才可以Upgrade
        public int maxHP_lv1;
        public int baseAmount_lv1; //如果是百分比（粮仓 and 钱庄），则这个以100为基础，其他都是绝对值
        public int baseAmount2_lv1;

        public int tolv2NeedWorkingDay; //
        public int maxHP_lv2;
        public int baseAmount_lv2;
        public int baseAmount2_lv2;

        public int tolv3NeedWorkingDay; //
        public int maxHP_lv3;
        public int baseAmount_lv3;
        public int baseAmount3_lv3;

        public int tolv4NeedWorkingDay; //
        public int maxHP_lv4;
        public int baseAmount_lv4;
        public int baseAmount2_lv4;

        public int tolv5NeedWorkingDay; //
        public int maxHP_lv5;
        public int baseAmount_lv5;
        public int baseAmount2_lv5;



    }
}