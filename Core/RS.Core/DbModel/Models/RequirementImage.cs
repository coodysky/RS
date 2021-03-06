﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbModel.Extension;

namespace DbModel.Models
{
    public class RequirementImage
    {
        #region 属性

        public int RequirementImageId { get; set; }
        public int RequirementId { get; set; }
        public string ImageType { get; set; }
        public string ImageUrl { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }

        #endregion
    }
}
