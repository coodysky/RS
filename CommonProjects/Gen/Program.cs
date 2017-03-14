using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gen.Model;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Dapper;

namespace Gen
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //string[] folders = Directory.GetDirectories("../MicroServices");
            string[] folders = Directory.GetDirectories("D:/Coody/Projects/RS/MicroServices");

            foreach (var folder in folders)
            {
                string fileName = folder + "/Ms/conf.yaml";
                if (File.Exists(fileName))
                {
                    string yamlText = File.ReadAllText(fileName);

                    var input = new StringReader(yamlText);

                    var deserializer = new DeserializerBuilder()
                        .WithNamingConvention(new CamelCaseNamingConvention())
                        .Build();

                    var confYaml = deserializer.Deserialize<ConfYaml>(input);



                    foreach (string tableName in confYaml.Tables)
                    {
                        using (var conn = new SqlConnection(confYaml.ConnectionString))
                        {
                            string sql = string.Format("EXEC SP_HELP '{0}'", tableName);
                            var s = conn.QueryMultiple(sql);
                            s.Read(); //第一个结果忽略不要
                            List<TableDesc> columns = s.Read<TableDesc>().ToList();
                        }
                    }

                }
            }

            Console.ReadLine();
        }
    }
}
