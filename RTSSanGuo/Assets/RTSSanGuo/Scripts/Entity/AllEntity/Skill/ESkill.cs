﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSSanGuo.Data;
namespace RTSSanGuo.Entity
{
    public  class ESkill:EBase
    {       
        public override bool Init(DBase dBase) {
            this.dBase = dBase;
            return true;
        }               
    }
}
