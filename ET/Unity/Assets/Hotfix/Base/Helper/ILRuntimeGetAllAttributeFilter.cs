using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETHotfix
{
    public class ILRuntimeGetAllAttributeFilter
    {
        public static bool IsContinue(Type type)
        {
            if (type.Name.Contains("<")) return true;
            if (type.Name.Contains("[")) return true;
            if (type.Name.Contains("`")) return true;
            if (type.Name.Contains("&")) return true;
            return false;
        }
    }
}
