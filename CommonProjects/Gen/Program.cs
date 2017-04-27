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
            try
            {
                //string[] folders = Directory.GetDirectories("../MicroServices");
                string[] folders = Directory.GetDirectories("D:/Coody/Projects/RS/MicroServices");

                foreach (var folder in folders)
                {
                    string fileName = folder + "/Ms/conf.yaml";

                    Console.WriteLine(string.Format("conf.yaml[{0}]", fileName));

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
                                List<ColumnDesc> columns = s.Read<ColumnDesc>().ToList();
                                List<IdentityColumn> identityColumns = s.Read<IdentityColumn>().ToList();
                                s.Read();
                                s.Read();
                                s.Read();
                                List<Constraint> constraints = s.Read<Constraint>().ToList();

                                if (columns.Count > 0)
                                {
                                    StringBuilder sb = new StringBuilder();
                                    sb.AppendFormat("using System;\n");
                                    sb.AppendFormat("using System.Collections.Generic;\n");
                                    sb.AppendFormat("using System.Linq;\n");
                                    sb.AppendFormat("using System.Text;\n");
                                    sb.AppendFormat("\n");
                                    sb.AppendFormat("namespace {0}\n",
                                        string.IsNullOrEmpty(confYaml.NameSpace) ? "Ms.DbModel" : confYaml.NameSpace);
                                    sb.AppendFormat("{{\n");
                                    sb.AppendFormat("    public class {0}\n", tableName);
                                    sb.AppendFormat("    {{\n");
                                    foreach (ColumnDesc column in columns)
                                    {
                                        string type = getType(column);

                                        sb.AppendFormat("        public {0} {1} {{ get; set; }}\n", type,
                                            column.Column_name);
                                    }

                                    getSqlForInsert(sb, tableName, columns, identityColumns);
                                    sb.AppendFormat("        \n");
                                    getSqlForSelectPrimaryKeys(sb, constraints, columns, tableName);
                                    sb.AppendFormat("        \n");

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

                                Console.WriteLine(string.Format("生成Table[{0}]完成", tableName));
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            //Console.WriteLine("请按任意键退出");
            //Console.ReadKey();
        }

        private static string getType(ColumnDesc column)
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

            return type;
        }

        private static void getSqlForInsert(StringBuilder sb, string tableName, List<ColumnDesc> columns, List<IdentityColumn> identityColumns)
        {
            if (columns != null && columns.Count > 0)
            {
                sb.AppendFormat("\n        public static string GetSqlForInsert({0} {1})", tableName, tableName.ToLower());
                sb.AppendFormat("\n        {{");
                sb.AppendFormat("\n            string sql = string.Empty;\n");
                sb.AppendFormat("\n            Dictionary<string, string> dicNameValue = new Dictionary<string, string>();\n");

                foreach (ColumnDesc column in columns)
                {
                    bool isExist = false;
                    if (identityColumns != null && identityColumns.Count > 0)
                    {
                        foreach (IdentityColumn identityColumn in identityColumns)
                        {
                            if (column.Column_name.Equals(identityColumn.Identity, StringComparison.OrdinalIgnoreCase))
                            {
                                isExist = true;
                                break;
                            }
                        }
                    }

                    if (!isExist)
                    {
                        if (column.Nullable.ToLower() == "yes")
                        {
                            sb.AppendFormat("\n            if ({0}.{1} != null)", tableName.ToLower(), column.Column_name);
                            sb.AppendFormat("\n            {{");

                            if (getType(column) == "bool?")
                            {
                                sb.AppendFormat("\n                dicNameValue.Add(\"{0}\", {1}.{2} ? \"1\" : \"0\");", column.Column_name, tableName.ToLower(), column.Column_name);
                            }
                            else if (getType(column) == "DateTime?")
                            {
                                sb.AppendFormat("\n                dicNameValue.Add(\"{0}\", {1}.{2}.Value.ToString(\"yyyy-MM-dd HH:mm:ss\"));", column.Column_name, tableName.ToLower(), column.Column_name);
                            }
                            else
                            {
                                sb.AppendFormat("\n                dicNameValue.Add(\"{0}\", {1}.{2}.ToString());", column.Column_name, tableName.ToLower(), column.Column_name);
                            }

                            sb.AppendFormat("\n            }}");
                        }
                        else
                        {
                            if (getType(column) == "string")
                            {
                                sb.AppendFormat("\n            dicNameValue.Add(\"{0}\", {1}.{2} ?? \"\");", column.Column_name, tableName.ToLower(), column.Column_name);
                            }
                            else if (getType(column) == "bool")
                            {
                                sb.AppendFormat("\n            dicNameValue.Add(\"{0}\", {1}.{2} ? \"1\" : \"0\");", column.Column_name, tableName.ToLower(), column.Column_name);
                            }
                            else if (getType(column) == "DateTime")
                            {
                                sb.AppendFormat("\n            dicNameValue.Add(\"{0}\", {1}.{2}.ToString(\"yyyy-MM-dd HH:mm:ss\"));", column.Column_name, tableName.ToLower(), column.Column_name);
                            }
                            else
                            {
                                sb.AppendFormat("\n            dicNameValue.Add(\"{0}\", {1}.{2}.ToString());", column.Column_name, tableName.ToLower(), column.Column_name);
                            }
                        }
                    }
                }

                sb.AppendFormat("\n            StringBuilder sql1 = new StringBuilder();");
                sb.AppendFormat("\n            StringBuilder sql2 = new StringBuilder();");
                sb.AppendFormat("\n            foreach (var nameValue in dicNameValue)");
                sb.AppendFormat("\n            {{");
                sb.AppendFormat("\n                sql1.AppendFormat(\"[{{0}}],\", nameValue.Key);");
                sb.AppendFormat("\n                sql2.AppendFormat(\"'{{0}}',\", nameValue.Value);");
                sb.AppendFormat("\n            }}");
                sb.AppendFormat("\n            ");
                sb.AppendFormat("\n            if (!string.IsNullOrEmpty(sql1.ToString()) && !string.IsNullOrEmpty(sql2.ToString()))");
                sb.AppendFormat("\n            {{");
                sb.AppendFormat("\n                sql = \"INSERT INTO[{0}](\";", tableName);
                sb.AppendFormat("\n                sql += sql1.ToString().Trim((',')) + \") VALUES(\";");
                sb.AppendFormat("\n                sql += sql2.ToString().Trim((',')) + \")\";");
                sb.AppendFormat("\n            }}");
                sb.AppendFormat("\n            ");
                sb.AppendFormat("\n            return sql;");

                sb.AppendFormat("\n        }}\n");
            }
        }

        private static void getSqlForSelectPrimaryKeys(StringBuilder sb, List<Constraint> constraints, List<ColumnDesc> columns, string tableName)
        {
            if (constraints == null || constraints.Count == 0)
            {
                return;
            }
            
            var constraint = constraints.Find(x => x.constraint_type.ToLower().StartsWith("primary key"));

            if (constraint == null)
            {
                return;
            }

            Dictionary<string, string> primaryKeysNameType = new Dictionary<string, string>();

            foreach (var constraint_key in constraint.constraint_keys.Split(','))
            {
                var column = columns.Find(x => x.Column_name.ToLower() == constraint_key.Trim().ToLower());
                string type = getType(column);
                string name = column.Column_name;
                primaryKeysNameType.Add(name, type);
            }

            if (primaryKeysNameType.Count > 0)
            {
                string sqlForParams = "";
                string sqlForQuery = "";
                string sqlForQueryValue = "";
                int i = 0;
                foreach (var nameType in primaryKeysNameType)
                {
                    sqlForParams += string.Format("{0} {1},", nameType.Value, nameType.Key);
                    sqlForQuery += string.Format("{0} = N'{{{1}}}' AND", nameType.Key, i++);
                    sqlForQueryValue += string.Format("{0},", nameType.Key);
                }
                if (sqlForParams != "")
                    sqlForParams = sqlForParams.Trim(',');
                if (sqlForQuery != "")
                    sqlForQuery = sqlForQuery.Substring(0, sqlForQuery.Length - " AND".Length);
                if (sqlForQueryValue != "")
                    sqlForQueryValue = sqlForQueryValue.Trim(',');

                sb.AppendFormat("\n        public static string GetSqlForSelectByPrimaryKeys({0})", sqlForParams);
                sb.AppendFormat("\n        {{");
                sb.AppendFormat("\n            return string.Format(\"SELECT TOP 1 * FROM [{0}] WITH(NOLOCK) WHERE {1}\", {2});",tableName, sqlForQuery, sqlForQueryValue);
                sb.AppendFormat("\n        }}");
            }
            
        }
    }
}
