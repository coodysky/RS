using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.ReqResp
{
    public abstract class RespBase
    {
        /// <summary>
        /// 1:成功，-1:失败
        /// </summary>
        public int Code { get; set; } = 1;
        /// <summary>
        /// 返回的消息内容，一般成功的话，会返回空，不成功才会返回值
        /// 不过判断成功与否用Code判断
        /// </summary>
        public string Message { get; set; } = "";
    }
}
