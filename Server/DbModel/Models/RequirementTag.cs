using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbModel.Extension;

namespace DbModel.Models
{
    public class RequirementTag
    {
        #region 属性

        public int RequirementTagId { get; set; }
        public int RequirementId { get; set; }
        public string Tag { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }

        #endregion

        #region 方法


        public static string GetSqlForInsert(RequirementTag requirementtag)
        {
            string sql = string.Empty;

            Dictionary<string, string> dicNameValue = new Dictionary<string, string>();

            dicNameValue.Add("RequirementId", requirementtag.RequirementId.ToString());
            dicNameValue.Add("Tag", requirementtag.Tag ?? "");
            dicNameValue.Add("CreateBy", requirementtag.CreateBy ?? "");
            dicNameValue.Add("CreateDate", requirementtag.CreateDate.ToStringDate());
            dicNameValue.Add("UpdateBy", requirementtag.UpdateBy ?? "");
            dicNameValue.Add("UpdateDate", requirementtag.UpdateDate.ToStringDate());
            StringBuilder sql1 = new StringBuilder();
            StringBuilder sql2 = new StringBuilder();
            foreach (var nameValue in dicNameValue)
            {
                sql1.AppendFormat("[{0}],", nameValue.Key);
                sql2.AppendFormat("'{0}',", nameValue.Value);
            }
            
            if (!string.IsNullOrEmpty(sql1.ToString()) && !string.IsNullOrEmpty(sql2.ToString()))
            {
                sql = "INSERT INTO[RequirementTag](";
                sql += sql1.ToString().Trim((',')) + ") VALUES(";
                sql += sql2.ToString().Trim((',')) + ")";
            }
            
            return sql;
        }


        public static string GetSqlForInsertAndQueryIdentity(RequirementTag requirementtag)
        {
            string sql = string.Empty;

            Dictionary<string, string> dicNameValue = new Dictionary<string, string>();

            dicNameValue.Add("RequirementId", requirementtag.RequirementId.ToString());
            dicNameValue.Add("Tag", requirementtag.Tag ?? "");
            dicNameValue.Add("CreateBy", requirementtag.CreateBy ?? "");
            dicNameValue.Add("CreateDate", requirementtag.CreateDate.ToStringDate());
            dicNameValue.Add("UpdateBy", requirementtag.UpdateBy ?? "");
            dicNameValue.Add("UpdateDate", requirementtag.UpdateDate.ToStringDate());
            StringBuilder sql1 = new StringBuilder();
            StringBuilder sql2 = new StringBuilder();
            foreach (var nameValue in dicNameValue)
            {
                sql1.AppendFormat("[{0}],", nameValue.Key);
                sql2.AppendFormat("'{0}',", nameValue.Value);
            }
            
            if (!string.IsNullOrEmpty(sql1.ToString()) && !string.IsNullOrEmpty(sql2.ToString()))
            {
                sql = "INSERT INTO[RequirementTag](";
                sql += sql1.ToString().Trim((',')) + ") VALUES(";
                sql += sql2.ToString().Trim((',')) + ")";
                sql += "\n";
                sql += "SELECT  SCOPE_IDENTITY()";
            }
            
            return sql;
        }


        public static string GetSqlForSelectByPrimaryKeys(int RequirementTagId)
        {
            return string.Format("SELECT TOP 1 * FROM [RequirementTag] WITH(NOLOCK) WHERE RequirementTagId = N'{0}'", RequirementTagId);
        }

        /// <summary>
        /// 获得条件查询sql
        /// </summary>
        /// <param name="where">必填，查询条件，不能包含where关键字</param>
        /// <param name="orderByDic">排序字典，key为排序条件，key中不能包含asc、desc关键字，value值true为asc，false为desc</param>
        /// <param name="topN">查询结果前N条</param>
        /// <returns>返回条件查询的sql</returns>
        public static string GetSqlForSelect(string where,Dictionary<string,bool> orderByDic,int topN)
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

            string sqlStr = string.Format("SELECT {0} * FROM [RequirementTag] WHERE {1} {2}", topNStr, where, orderByStr);

            return sqlStr;
        }

        public static string GetSqlForUpdate(string set, string where)
        {
            if (string.IsNullOrEmpty(set) || string.IsNullOrEmpty(where))
                return string.Empty;

            return string.Format("UPDATE [RequirementTag] SET {0} WHERE {1}", set, where);
        }

        public static string GetSqlForDelete(string where)
        {
            if (string.IsNullOrEmpty(where))
                return string.Empty;

            return string.Format("DELETE FROM [RequirementTag] WHERE {0}", where);
        }

        #endregion
    }
}
