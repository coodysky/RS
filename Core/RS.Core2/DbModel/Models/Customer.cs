using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbModel.Extension;

namespace DbModel.Models
{
    public class Customer
    {
        #region 属性

        public int CustomerId { get; set; }
        public string NickName { get; set; }
        public string RealName { get; set; }
        public string Password { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public string CreateBy { get; set; }
        public long CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public long UpdateDate { get; set; }

        #endregion
    }
}
