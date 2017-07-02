using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Models.Resp
{
    public class RespEntityRequirement:RespEntity
    {
        public Requirement Requirement;

        public List<Requirement> Requirements;
        public List<Customer> Customers;
    }
}
