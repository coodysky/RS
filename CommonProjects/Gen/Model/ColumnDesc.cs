using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gen.Model
{
    class ColumnDesc
    {
        public string Column_name { set; get; }
        public string Type { set; get; }
        public int Length { set; get; }
        public string Nullable { set; get; }
    }
}
