using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.ReqResp.Requirement
{
    public class Resp_LoadRequirementsByTitle: RespBase
    {
        public List<RespEntity_Requirement> Requirements { get; set; } = new List<RespEntity_Requirement>();
    }
}
