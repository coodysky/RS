﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ms.DbModel
{
    public class ResponseRecord
    {
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
            dicNameValue.Add("CreateDate", responserecord.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"));
            dicNameValue.Add("UpdateBy", responserecord.UpdateBy ?? "");
            dicNameValue.Add("UpdateDate", responserecord.UpdateDate.ToString("yyyy-MM-dd HH:mm:ss"));
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
    }
}
