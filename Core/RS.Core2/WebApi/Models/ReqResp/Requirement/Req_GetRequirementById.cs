using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.ReqResp.Requirement
{
    public class Req_GetRequirementById
    {
        public int RequirementId { get; set; }

        public bool IsNeedTags { get; set; } = false;
    }
}
