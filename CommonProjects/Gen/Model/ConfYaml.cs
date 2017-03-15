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
        [YamlMember(Alias = "dbModelPath", ApplyNamingConventions = false)]
        public string DbModelPath { get; set; }
        [YamlMember(Alias = "nameSpace", ApplyNamingConventions = false)]
        public string NameSpace { get; set; }
        [YamlMember(Alias = "tables", ApplyNamingConventions = false)]
        public List<string> Tables { get; set; }
    }
}
