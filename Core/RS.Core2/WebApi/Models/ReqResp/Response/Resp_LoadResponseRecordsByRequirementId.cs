using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.ReqResp.Response
{
    public class Resp_LoadResponseRecordsByRequirementId : RespBase
    {
        public RespEntity_Requirement Requirement { get; set; }
        public List<RespEntity_ResponseRecord> Records { get; set; } = new List<RespEntity_ResponseRecord>();
    }
}
