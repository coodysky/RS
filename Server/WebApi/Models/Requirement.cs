﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Requirement
    {
        public int RequirementId { get; set; }
        public int CustomerId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public string Address { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string ContactPhone { get; set; }
        public string ContactMan { get; set; }
        public string ReleaseDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public string CreateDate { get; set; }
        public string UpdateDate { get; set; }
    }
}