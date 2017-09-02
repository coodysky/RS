using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.ReqResp.Response
{
    public class RespEntity_ResponseRecord
    {
        public int ResponseRecordId { get; set; }
        public int RequirementId { get; set; }
        public int ResponserId { get; set; }
        public string Title { get; set; }
        public decimal? Price { get; set; }
        public string Content { get; set; }
        public string ContactPhone { get; set; }
        public string ContactMan { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsFinalServeRecord { get; set; }
        public string CreateBy { get; set; }
        public string CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public string UpdateDate { get; set; }
    }
}
