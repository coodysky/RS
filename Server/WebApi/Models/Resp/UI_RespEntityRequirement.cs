using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Models.Resp
{
    public class UI_RespEntityRequirement : UI_RespEntity
    {
        public UI_Requirement Requirement;

        public List<UI_Requirement> Requirements;
        public List<UI_Customer> Customers;
    }
}
