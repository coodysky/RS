﻿using System;
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

        public static string GetSqlForInsert(Customer customer)
        {
            string sql = string.Empty;

            Dictionary<string, string> dicNameValue = new Dictionary<string, string>();

            dicNameValue.Add("NickName", customer.NickName ?? "");
            dicNameValue.Add("RealName", customer.RealName ?? "");
            dicNameValue.Add("Password", customer.Password ?? "");
            dicNameValue.Add("MobilePhone", customer.MobilePhone ?? "");
            dicNameValue.Add("Email", customer.Email ?? "");
            dicNameValue.Add("CreateBy", customer.CreateBy ?? "");
            dicNameValue.Add("CreateDate", customer.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"));
            dicNameValue.Add("UpdateBy", customer.UpdateBy ?? "");
            dicNameValue.Add("UpdateDate", customer.UpdateDate.ToString("yyyy-MM-dd HH:mm:ss"));
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

        public static string GetSqlForSelect(string where,Dictionary<string,bool> orderByDic,int topN=0)
        {
            string topNStr = "";
            if (topN > 0)
            {
                topNStr += string.Format(" TOP {0} ", topN);
            }

            string orderByStr = "";
            if (orderByDic != null && orderByDic.Count > 0)
            {
                foreach (var keyVal in orderByDic)
                {
                    orderByStr += string.Format("{0} {1},", keyVal.Key, keyVal.Value ? "ASC" : "DESC");
                }
            }
            if (!string.IsNullOrEmpty(orderByStr))
            {
                orderByStr = "ORDER BY " + orderByStr.Trim(',');
            }

            string sqlStr = string.Format("SELECT {0} * FROM [Customer] WHERE {1} {2}", topNStr, where, orderByStr);

            return sqlStr;
        }

        public static string GetSqlForUpdate(string set, string where)
        {
            if (string.IsNullOrEmpty(set) || string.IsNullOrEmpty(where))
                return string.Empty;

            return string.Format("UPDATE [Customer] SET {0} WHERE {1}", set, where);
        }

        public static string GetSqlForDelete(string where)
        {
            if (string.IsNullOrEmpty(where))
                return string.Empty;

            return string.Format("DELETE FROM [Customer] WHERE {0}", where);
        }
    }
}
