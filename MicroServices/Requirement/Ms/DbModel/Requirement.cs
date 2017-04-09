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
    }
}
