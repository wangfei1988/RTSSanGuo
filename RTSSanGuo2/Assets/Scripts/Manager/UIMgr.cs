using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace RTSSanGuo
{
    //
    public class UIMgr :MonoBehaviour
    {
        public static UIMgr Instacne;
        private void Awake()
        {
            if (Instacne == null)
                Instacne = this;
            else
                Debug.LogError("more than one instance");
        }

       

    }
}
