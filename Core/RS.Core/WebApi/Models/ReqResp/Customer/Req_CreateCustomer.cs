using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.ReqResp.Customer
{
    public class Req_CreateCustomer
    {
        public string NickName { get; set; }
        public string RealName { get; set; }
        public string Password { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
    }
}
