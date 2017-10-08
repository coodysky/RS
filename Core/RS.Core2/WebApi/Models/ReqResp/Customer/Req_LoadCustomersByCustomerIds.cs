using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.ReqResp.Customer
{
    public class Req_LoadCustomersByCustomerIds
    {
        public List<int> CustomerIds { get; set; }
    }
}
