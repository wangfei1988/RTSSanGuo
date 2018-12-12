using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace RTSSanGuo
{
    //有些不需要继承monobehavior 但是为了统一全部继承
    public class BuldingPlacementMgr :MonoBehaviour
    {
        public static BuldingPlacementMgr Instacne;
        private void Awake()
        {
            if (Instacne == null)
                Instacne = this;
            else
                Debug.LogError("more than one instance");
        }
    }
}
