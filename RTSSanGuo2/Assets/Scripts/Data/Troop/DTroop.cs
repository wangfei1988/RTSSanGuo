using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace RTSSanGuo
{

    //可以在play过程中被new ， new 就需要模板，末班类还得定义。 这里和Person不一样，Person 可以被修改，但是不可以new，所以不需要模板
    [Serializable]
    public  class DTroop:DataBase
    {
        public int id_trooptype;
        public int food;
        public int money;
        public int origsoldiernum;
        public int cursoldiernum;

        public int id_person1;
        public int id_person2;
        public int id_person3;


        public int parentid_city; //city 没有之后自动切换为captial

    }
}
