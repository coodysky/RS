using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbModel.Extension;

namespace DbModel.Models
{
    public class Requirement
    {
        #region 属性

        public int RequirementId { get; set; }
        public int CustomerId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public decimal? Price { get; set; }
        public string Address { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public string ContactPhone { get; set; }
        public string ContactMan { get; set; }
        public string RequirementStatusCode { get; set; }
        public long? ReleaseDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public long CreateDate { get; set; }
        public long UpdateDate { get; set; }

        #endregion
    }
}
