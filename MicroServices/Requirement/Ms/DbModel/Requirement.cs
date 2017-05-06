using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ms.DbModel
{
    public class Requirement
    {
        public int RequirementId { get; set; }
        public int CustomerId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Address { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string ContactPhone { get; set; }
        public string ContactMan { get; set; }
        public string RequirementStatusCode { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }

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
    }
}
