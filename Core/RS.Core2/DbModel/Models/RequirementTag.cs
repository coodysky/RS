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
    }
}
