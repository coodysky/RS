using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Ms
{
    public class ExampleController : ApiController
    {
        [Route("KKK/FindExample")]
        [HttpGet]
        public string FindExample()
        {
            return "Example";
        }
    }
}
