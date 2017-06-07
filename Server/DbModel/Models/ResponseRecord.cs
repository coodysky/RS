using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbModel.Extension;

namespace DbModel.Models
{
    public class ResponseRecord
    {
        #region 属性

        public int ResponseRecordId { get; set; }
        public int RequirementId { get; set; }
        public int ResponserId { get; set; }
        public string Content { get; set; }
        public string ContactPhone { get; set; }
        public string ContactMan { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsFinalServeRecord { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }

        #endregion

        #region 方法


        public static string GetSqlForInsert(ResponseRecord responserecord)
        {
            string sql = string.Empty;

            Dictionary<string, string> dicNameValue = new Dictionary<string, string>();

            dicNameValue.Add("RequirementId", responserecord.RequirementId.ToString());
            dicNameValue.Add("ResponserId", responserecord.ResponserId.ToString());
            dicNameValue.Add("Content", responserecord.Content ?? "");
            dicNameValue.Add("ContactPhone", responserecord.ContactPhone ?? "");
            dicNameValue.Add("ContactMan", responserecord.ContactMan ?? "");
            dicNameValue.Add("IsDeleted", responserecord.IsDeleted ? "1" : "0");
            dicNameValue.Add("IsFinalServeRecord", responserecord.IsFinalServeRecord ? "1" : "0");
            dicNameValue.Add("CreateBy", responserecord.CreateBy ?? "");
            dicNameValue.Add("CreateDate", responserecord.CreateDate.ToStringDate());
            dicNameValue.Add("UpdateBy", responserecord.UpdateBy ?? "");
            dicNameValue.Add("UpdateDate", responserecord.UpdateDate.ToStringDate());
            StringBuilder sql1 = new StringBuilder();
            StringBuilder sql2 = new StringBuilder();
            foreach (var nameValue in dicNameValue)
            {
                sql1.AppendFormat("[{0}],", nameValue.Key);
                sql2.AppendFormat("'{0}',", nameValue.Value);
            }
            
            if (!string.IsNullOrEmpty(sql1.ToString()) && !string.IsNullOrEmpty(sql2.ToString()))
            {
                sql = "INSERT INTO[ResponseRecord](";
                sql += sql1.ToString().Trim((',')) + ") VALUES(";
                sql += sql2.ToString().Trim((',')) + ")";
            }
            
            return sql;
        }


        public static string GetSqlForSelectByPrimaryKeys(int ResponseRecordId)
        {
            return string.Format("SELECT TOP 1 * FROM [ResponseRecord] WITH(NOLOCK) WHERE ResponseRecordId = N'{0}'", ResponseRecordId);
        }

        /// <summary>
        /// 获得条件查询sql
        /// </summary>
        /// <param name="where">必填，查询条件，不能包含where关键字</param>
        /// <param name="orderByDic">排序字典，key为排序条件，ke中不能包含asc、desc关键字，value值true为asc，false为desc</param>
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

            string sqlStr = string.Format("SELECT {0} * FROM [ResponseRecord] WHERE {1} {2}", topNStr, where, orderByStr);

            return sqlStr;
        }

        public static string GetSqlForUpdate(string set, string where)
        {
            if (string.IsNullOrEmpty(set) || string.IsNullOrEmpty(where))
                return string.Empty;

            return string.Format("UPDATE [ResponseRecord] SET {0} WHERE {1}", set, where);
        }

        public static string GetSqlForDelete(string where)
        {
            if (string.IsNullOrEmpty(where))
                return string.Empty;

            return string.Format("DELETE FROM [ResponseRecord] WHERE {0}", where);
        }

        #endregion
    }
}
