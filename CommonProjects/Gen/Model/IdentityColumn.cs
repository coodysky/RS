using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gen.Model
{
    class IdentityColumn
    {
        public string Identity { set; get; }
        public int Seed { set; get; }
        public int Increment { set; get; }
    }
}
