﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ms.ApiModel
{
    public class ResponseRecord
    {
        public int ResponseRecordId { get; set; }
        public int RequirementId { get; set; }
        public int ResponserId { get; set; }
        public string Content { get; set; }
        public string ContactPhone { get; set; }
        public string ContactMan { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsFinalServeRecord { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}