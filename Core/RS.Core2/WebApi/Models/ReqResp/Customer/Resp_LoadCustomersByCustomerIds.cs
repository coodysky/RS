using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.ReqResp.Customer
{
    public class Resp_LoadCustomersByCustomerIds : RespBase
    {
        public List<RespEntity_Customer> Customers { get; set; } = new List<RespEntity_Customer>();
    }
}
