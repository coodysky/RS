using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class UI_Requirement
    {
        public int? RequirementId { get; set; }
        public int? CustomerId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public decimal? Price { get; set; }
        public string ImageUrl { get; set; }
        public string Address { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public string ContactPhone { get; set; }
        public string ContactMan { get; set; }
        public string ReleaseDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public string CreateDate { get; set; }
        public string UpdateDate { get; set; }
        public List<UI_RequirementTag> RequirementTags { get; set; }
    }
}
