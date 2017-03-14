using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace Gen.Model
{
    public class ConfYaml
    {
        [YamlMember(Alias = "db", ApplyNamingConventions = false)]
        public string Db { get; set; }
        [YamlMember(Alias = "connectionString", ApplyNamingConventions = false)]
        public string ConnectionString { get; set; }
        [YamlMember(Alias = "tables", ApplyNamingConventions = false)]
        public List<string> Tables { get; set; }
    }
}
