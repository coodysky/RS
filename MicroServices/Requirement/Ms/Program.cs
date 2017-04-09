using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroService4Net;

namespace MS
{
    class Program
    {
        public static void Main(string[] args)
        {
            var microService = new MicroService(port: 14002);
            microService.Run(args);
        }
    }
}
