#define Testtt

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace RTSSanGuo
{
    public  class EntityBase:MonoBehaviour
    {
#region wrap data
        /*******wrap basic ************/
        public virtual  int ID {
            get  { return -1; }
        }
        public virtual string  Alias
        {
            get { return ""; }
        }
        public virtual  string ShortDesc
        {
            get { return ""; }
        }
        public virtual string FullDesc
        {
            get { return ""; }
        }
#endregion
    }
}
