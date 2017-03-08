using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace GenDbModel
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var conn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionStringForSqlServer"]))
            {
                string sql = "EXEC SP_HELP 'Package'";
                var s = conn.QueryMultiple(sql);
                s.Read();//第一个结果忽略不要
                List<ColumnsDef> columns = s.Read<ColumnsDef>().ToList();
            }
        }
    }

    internal class ColumnsDef
    {
        public string Column_name { set; get; }
        public string Type { set; get; }
        public int Length { set; get; }
        public string Nullable { set; get; }
    }
}
