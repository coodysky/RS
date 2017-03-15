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

                            if (columns.Count > 0)
                            {
                                StringBuilder sb = new StringBuilder();
                                sb.AppendFormat("using System;\n");
                                sb.AppendFormat("using System.Collections.Generic;\n");
                                sb.AppendFormat("using System.Linq;\n");
                                sb.AppendFormat("using System.Text;\n");
                                sb.AppendFormat("\n");
                                sb.AppendFormat("namespace {0}\n", string.IsNullOrEmpty(confYaml.NameSpace) ? "Ms.DbModel": confYaml.NameSpace);
                                sb.AppendFormat("{{\n");
                                sb.AppendFormat("    public class {0}\n", tableName);
                                sb.AppendFormat("    {{\n");
                                foreach (TableDesc column in columns)
                                {
                                    string type = "";

                                    if (column.Type.ToLower() == "varchar" || column.Type.ToLower() == "nvarchar" ||
                                        column.Type.ToLower() == "char")
                                    {
                                        type += "string";
                                    }
                                    else
                                    {
                                        switch (column.Type.ToLower())
                                        {
                                            case "int":
                                                type += "int";
                                                break;
                                            case "bigint":
                                                type += "long";
                                                break;
                                            case "float":
                                                type += "float";
                                                break;
                                            case "double":
                                                type += "double";
                                                break;
                                            case "datetime":
                                                type += "DateTime";
                                                break;
                                            case "money":
                                            case "decimal":
                                                type += "decimal";
                                                break;
                                            case "bit":
                                                type += "bool";
                                                break;
                                        }

                                        if (column.Nullable.ToLower() == "yes")
                                        {
                                            type += "?";
                                        }
                                    }

                                    sb.AppendFormat("        public {0} {1} {{ get; set; }}\n", type, column.Column_name);
                                }
                                sb.AppendFormat("    }}\n");
                                sb.AppendFormat("}}\n");

                                string dbModelPath = string.Format("{0}/{1}", folder, confYaml.DbModelPath);
                                if (!Directory.Exists(dbModelPath))
                                {
                                    Directory.CreateDirectory(dbModelPath);
                                }

                                string modelFileName = string.Format("{0}/{1}.cs", dbModelPath, tableName);
                                File.WriteAllText(modelFileName, sb.ToString());
                            }
                        }
                    }

                }
            }

            Console.ReadLine();
        }
    }
}
