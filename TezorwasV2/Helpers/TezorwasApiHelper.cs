using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TezorwasV2.Helpers
{
    public class TezorwasApiHelper
    {
        public readonly string ApiUrl;
        public TezorwasApiHelper(string apiUrl)
        {
            ApiUrl = apiUrl;
        }
    }
}
