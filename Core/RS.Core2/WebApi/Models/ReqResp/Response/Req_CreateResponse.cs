using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.ReqResp.Response
{
    public class Req_CreateResponse
    {
        public int RequirementId { get; set; }
        public int ResponserId { get; set; }
        public string Title { get; set; }
        public decimal? Price { get; set; }
        public string Content { get; set; }
        public string ContactPhone { get; set; }
        public string ContactMan { get; set; }
    }
}
