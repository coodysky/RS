using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.ReqResp.Requirement
{
    public class Req_LoadRequirementsByTitle
    {
        public string Title { get; set; }
        /// <summary>
        /// 查询前N条数据
        /// </summary>
        public int TopN { get; set; } = 20;
        /// <summary>
        /// 是否需要查询标签
        /// </summary>
        public bool IsNeedTags { get; set; } = false;
    }
}
