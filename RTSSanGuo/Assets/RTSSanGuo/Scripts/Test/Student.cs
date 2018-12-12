using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTSSanGuo.Data;
namespace ConsoleApp1
{
     public class Student :ICsvData
    {
        public int id;
        public string name;    
        
        public bool  InitFrom(string[] values)
        {
            if (values.Length != 2) return false;
            try
            {
                this.id = int.Parse(values[0]);
                this.name = values[1];
            }
            catch (Exception e) {
                return false;
            }
            return true;
        }
    }
}
