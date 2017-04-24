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

        public string GetSqlForInsert()
        {
            string sql = string.Empty;

            Dictionary<string, string> dicNameValue = new Dictionary<string, string>();

            dicNameValue.Add("CustomerId", this.CustomerId.ToString());
            dicNameValue.Add("Title", this.Title ?? "");
            dicNameValue.Add("Content", this.Content ?? "");
            if (this.Address != null)
            {
                dicNameValue.Add("Address", this.Address.ToString());
            }
            if (this.Longitude != null)
            {
                dicNameValue.Add("Longitude", this.Longitude.ToString());
            }
            if (this.Latitude != null)
            {
                dicNameValue.Add("Latitude", this.Latitude.ToString());
            }
            if (this.ContactPhone != null)
            {
                dicNameValue.Add("ContactPhone", this.ContactPhone.ToString());
            }
            if (this.ContactMan != null)
            {
                dicNameValue.Add("ContactMan", this.ContactMan.ToString());
            }
            dicNameValue.Add("RequirementStatusCode", this.RequirementStatusCode ?? "");
            if (this.ReleaseDate != null)
            {
                dicNameValue.Add("ReleaseDate", this.ReleaseDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            }
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
