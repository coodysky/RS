using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.ReqResp.Requirement
{
    public class Req_CreateRequirement
    {
        public int CustomerId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public decimal? Price { get; set; }
        public string ImageUrl { get; set; }
        public string Address { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public string ContactPhone { get; set; }
        public string ContactMan { get; set; }

        /// <summary>
        /// 需求打上的标签
        /// </summary>
        public List<string> Tags { get; set; }
    }
}
