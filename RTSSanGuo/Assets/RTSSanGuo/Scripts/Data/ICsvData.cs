using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTSSanGuo.Data
{
    public interface ICsvData
    {
        bool InitFrom(string[] values);
    }
}
