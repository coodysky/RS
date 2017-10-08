using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.ReqResp.Requirement
{
    public class Resp_GetRequirementById : RespBase
    {
        public RespEntity_Requirement Requirement { get; set; }
    }
}
