using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.ReqResp.Response
{
    public class Resp_LoadResponseRecordsByCustomerId : RespBase
    {
        public List<Resp_LoadResponseRecordsByCustomerId_Entity> Entitys { get; set; } = new List<Resp_LoadResponseRecordsByCustomerId_Entity>();
    }

    public class Resp_LoadResponseRecordsByCustomerId_Entity
    {
        public RespEntity_Requirement Requirement { get; set; }
        public List<RespEntity_ResponseRecord> Records { get; set; } = new List<RespEntity_ResponseRecord>();
    }
}
