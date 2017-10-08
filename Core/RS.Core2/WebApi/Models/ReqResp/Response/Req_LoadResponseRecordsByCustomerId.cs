using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.ReqResp.Response
{
    public class Req_LoadResponseRecordsByCustomerId
    {
        public int CustomerId { get; set; }
        public int TopN { get; set; } = 20;
    }
}
