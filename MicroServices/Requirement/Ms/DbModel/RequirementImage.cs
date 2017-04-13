using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ms.DbModel
{
    public class RequirementImage
    {
        public int RequirementImageId { get; set; }
        public int RequirementId { get; set; }
        public string ImageType { get; set; }
        public string ImageUrl { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }

        public static string GetSqlForInsert(RequirementImage requirementimage)
        {
            string sql = string.Empty;

            Dictionary<string, string> dicNameValue = new Dictionary<string, string>();

            dicNameValue.Add("RequirementId", requirementimage.RequirementId.ToString());
            dicNameValue.Add("ImageType", requirementimage.ImageType ?? "");
            dicNameValue.Add("ImageUrl", requirementimage.ImageUrl ?? "");
            dicNameValue.Add("CreateBy", requirementimage.CreateBy ?? "");
            dicNameValue.Add("CreateDate", requirementimage.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"));
            dicNameValue.Add("UpdateBy", requirementimage.UpdateBy ?? "");
            dicNameValue.Add("UpdateDate", requirementimage.UpdateDate.ToString("yyyy-MM-dd HH:mm:ss"));
            StringBuilder sql1 = new StringBuilder();
            StringBuilder sql2 = new StringBuilder();
            foreach (var nameValue in dicNameValue)
            {
                sql1.AppendFormat("[{0}],", nameValue.Key);
                sql2.AppendFormat("'{0}',", nameValue.Value);
            }
            
            if (!string.IsNullOrEmpty(sql1.ToString()) && !string.IsNullOrEmpty(sql2.ToString()))
            {
                sql = "INSERT INTO[RequirementImage](";
                sql += sql1.ToString().Trim((',')) + ") VALUES(";
                sql += sql2.ToString().Trim((',')) + ")";
            }
            
            return sql;
        }
    }
}
