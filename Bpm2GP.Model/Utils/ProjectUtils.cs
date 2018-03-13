using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bpm2GP.Model.Utils
{
    public class ProjectUtils
    {
        public static int DecimalPlaces(decimal d)
        {
            var res = 0;
            while (d != (long)d)
            {
                res++;
                d = d * 10;
            }
            return res;
        }
    }
}
