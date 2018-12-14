#define TEestttt

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace RTSSanGuo
{
         
    //为什么不放到DataMagaer --只是方便阅读和查看
    public class DateMgr : MonoBehaviour
    {
        public static DateMgr Instacne;
        private void Awake()
        {
            if (Instacne == null)
                Instacne = this;
            else
                Debug.LogError("more than one instance");
        }

        public int year;
        public int month;
        public int day;


    }
}
