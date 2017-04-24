using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ms.DbModel
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string NickName { get; set; }
        public string RealName { get; set; }
        public string Password { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }

        public string GetSqlForInsert()
        {
            string sql = string.Empty;

            Dictionary<string, string> dicNameValue = new Dictionary<string, string>();

            dicNameValue.Add("NickName", this.NickName ?? "");
            dicNameValue.Add("RealName", this.RealName ?? "");
            dicNameValue.Add("Password", this.Password ?? "");
            dicNameValue.Add("MobilePhone", this.MobilePhone ?? "");
            dicNameValue.Add("Email", this.Email ?? "");
            dicNameValue.Add("CreateBy", this.CreateBy ?? "");
            dicNameValue.Add("CreateDate", this.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"));
            dicNameValue.Add("UpdateBy", this.UpdateBy ?? "");
            dicNameValue.Add("UpdateDate", this.UpdateDate.ToString("yyyy-MM-dd HH:mm:ss"));
            StringBuilder sql1 = new StringBuilder();
            StringBuilder sql2 = new StringBuilder();
            foreach (var nameValue in dicNameValue)
            {
                sql1.AppendFormat("[{0}],", nameValue.Key);
                sql2.AppendFormat("'{0}',", nameValue.Value);
            }
            
            if (!string.IsNullOrEmpty(sql1.ToString()) && !string.IsNullOrEmpty(sql2.ToString()))
            {
                sql = "INSERT INTO[Customer](";
                sql += sql1.ToString().Trim((',')) + ") VALUES(";
                sql += sql2.ToString().Trim((',')) + ")";
            }
            
            return sql;
        }
        

        public static string GetSqlForSelectByPrimaryKeys(int CustomerId)
        {
            return string.Format("SELECT TOP 1 * FROM [Customer] WITH(NOLOCK) WHERE CustomerId = N'{0}'", CustomerId);
        }        
    }
}
