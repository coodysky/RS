using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ms.DbModel
{
    public class CustomerAddress
    {
        public int CustomerAddressId { get; set; }
        public int CustomerId { get; set; }
        public string Address { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public bool IsDefault { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }

        public static string GetSqlForInsert(CustomerAddress customeraddress)
        {
            string sql = string.Empty;

            Dictionary<string, string> dicNameValue = new Dictionary<string, string>();

            dicNameValue.Add("CustomerId", customeraddress.CustomerId.ToString());
            dicNameValue.Add("Address", customeraddress.Address ?? "");
            dicNameValue.Add("Longitude", customeraddress.Longitude ?? "");
            dicNameValue.Add("Latitude", customeraddress.Latitude ?? "");
            dicNameValue.Add("IsDefault", customeraddress.IsDefault ? "1" : "0");
            dicNameValue.Add("CreateBy", customeraddress.CreateBy ?? "");
            dicNameValue.Add("CreateDate", customeraddress.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"));
            dicNameValue.Add("UpdateBy", customeraddress.UpdateBy ?? "");
            dicNameValue.Add("UpdateDate", customeraddress.UpdateDate.ToString("yyyy-MM-dd HH:mm:ss"));
            StringBuilder sql1 = new StringBuilder();
            StringBuilder sql2 = new StringBuilder();
            foreach (var nameValue in dicNameValue)
            {
                sql1.AppendFormat("[{0}],", nameValue.Key);
                sql2.AppendFormat("'{0}',", nameValue.Value);
            }
            
            if (!string.IsNullOrEmpty(sql1.ToString()) && !string.IsNullOrEmpty(sql2.ToString()))
            {
                sql = "INSERT INTO[CustomerAddress](";
                sql += sql1.ToString().Trim((',')) + ") VALUES(";
                sql += sql2.ToString().Trim((',')) + ")";
            }
            
            return sql;
        }
    }
}
