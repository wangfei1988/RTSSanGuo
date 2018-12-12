using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSSanGuo.Data;
using UnityEngine;
namespace RTSSanGuo.Entity
{
    //Entiy 尽量写完所有可操作行为Command ，部分复杂Command 可再写一个Componnet ，但是统一在这Wraper一下
    public  class EBase:MonoBehaviour
    {
        public int id;

        public virtual bool Load(int id) {
            return false;
        }
        public virtual bool SaveTo(int id) {

            return false;
        }

    }
}
