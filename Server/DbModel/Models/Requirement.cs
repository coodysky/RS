using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DbModel.Models
{
    public class Requirement
    {
        #region 属性

        public int RequirementId { get; set; }
        public int CustomerId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Address { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public string ContactPhone { get; set; }
        public string ContactMan { get; set; }
        public string RequirementStatusCode { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }

        #endregion

        #region 方法


        public static string GetSqlForInsert(Requirement requirement)
        {
            string sql = string.Empty;

            Dictionary<string, string> dicNameValue = new Dictionary<string, string>();

            dicNameValue.Add("CustomerId", requirement.CustomerId.ToString());
            dicNameValue.Add("Title", requirement.Title ?? "");
            dicNameValue.Add("Content", requirement.Content ?? "");
            if (requirement.Address != null)
            {
                dicNameValue.Add("Address", requirement.Address.ToString());
            }
            if (requirement.Longitude != null)
            {
                dicNameValue.Add("Longitude", requirement.Longitude.ToString());
            }
            if (requirement.Latitude != null)
            {
                dicNameValue.Add("Latitude", requirement.Latitude.ToString());
            }
            if (requirement.ContactPhone != null)
            {
                dicNameValue.Add("ContactPhone", requirement.ContactPhone.ToString());
            }
            if (requirement.ContactMan != null)
            {
                dicNameValue.Add("ContactMan", requirement.ContactMan.ToString());
            }
            dicNameValue.Add("RequirementStatusCode", requirement.RequirementStatusCode ?? "");
            if (requirement.ReleaseDate != null)
            {
                dicNameValue.Add("ReleaseDate", requirement.ReleaseDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            dicNameValue.Add("CreateBy", requirement.CreateBy ?? "");
            dicNameValue.Add("CreateDate", requirement.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"));
            dicNameValue.Add("UpdateBy", requirement.UpdateBy ?? "");
            dicNameValue.Add("UpdateDate", requirement.UpdateDate.ToString("yyyy-MM-dd HH:mm:ss"));
            StringBuilder sql1 = new StringBuilder();
            StringBuilder sql2 = new StringBuilder();
            foreach (var nameValue in dicNameValue)
            {
                sql1.AppendFormat("[{0}],", nameValue.Key);
                sql2.AppendFormat("'{0}',", nameValue.Value);
            }
            
            if (!string.IsNullOrEmpty(sql1.ToString()) && !string.IsNullOrEmpty(sql2.ToString()))
            {
                sql = "INSERT INTO[Requirement](";
                sql += sql1.ToString().Trim((',')) + ") VALUES(";
                sql += sql2.ToString().Trim((',')) + ")";
            }
            
            return sql;
        }


        public static string GetSqlForSelectByPrimaryKeys(int RequirementId)
        {
            return string.Format("SELECT TOP 1 * FROM [Requirement] WITH(NOLOCK) WHERE RequirementId = N'{0}'", RequirementId);
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

            string sqlStr = string.Format("SELECT {0} * FROM [Requirement] WHERE {1} {2}", topNStr, where, orderByStr);

            return sqlStr;
        }

        public static string GetSqlForUpdate(string set, string where)
        {
            if (string.IsNullOrEmpty(set) || string.IsNullOrEmpty(where))
                return string.Empty;

            return string.Format("UPDATE [Requirement] SET {0} WHERE {1}", set, where);
        }

        public static string GetSqlForDelete(string where)
        {
            if (string.IsNullOrEmpty(where))
                return string.Empty;

            return string.Format("DELETE FROM [Requirement] WHERE {0}", where);
        }

        #endregion
    }
}
