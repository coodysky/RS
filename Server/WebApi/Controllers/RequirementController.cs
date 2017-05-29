﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dapper;
using WebApi.Models;
using DbModels=DbModel.Models;

namespace WebApi.Controllers
{
    public class RequirementController : ApiController
    {
        [HttpPost]
        public RespEntity CreateRequirement(Requirement requirement)
        {
            if (requirement == null || requirement.CustomerId <= 0 || string.IsNullOrEmpty(requirement.Title) ||
                string.IsNullOrEmpty(requirement.Content))
            {
                return new RespEntity() { Code = -1, Message = "传入参数错误" };
            }

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MsSqlCon"].ConnectionString))
            {
                DbModels.Customer customer =
                    conn.Query<DbModels.Customer>(DbModels.Customer.GetSqlForSelectByPrimaryKeys(requirement.CustomerId))
                        .FirstOrDefault();

                if (customer == null)
                {
                    return new RespEntity() { Code = -1, Message = "用户不存在" };
                }

                DbModels.Requirement modelRequirement = new DbModels.Requirement();
                modelRequirement.CustomerId = customer.CustomerId;
                modelRequirement.Title = requirement.Title;
                modelRequirement.Content = requirement.Content;
                modelRequirement.Address = requirement.Address;
                modelRequirement.Latitude = requirement.Latitude;
                modelRequirement.Longitude = requirement.Longitude;
                modelRequirement.ContactPhone = requirement.ContactPhone;
                modelRequirement.ContactMan = requirement.ContactMan;
                modelRequirement.RequirementStatusCode = "Init";
                modelRequirement.CreateBy = customer.NickName;
                modelRequirement.UpdateBy = customer.NickName;
                modelRequirement.CreateDate = DateTime.Now;
                modelRequirement.UpdateDate = DateTime.Now;

                string sqlForRequirementInsert = DbModels.Requirement.GetSqlForInsert(modelRequirement);

                conn.Execute(sqlForRequirementInsert);
            }

            return new RespEntity() { Code = 1, Message = "" };
        }
        
        [HttpPost]
        public RespEntity GetRequirement(int requirementId)
        {
            if (requirementId <= 0)
            {
                return new RespEntity() { Code = -1, Message = "传入参数错误" };
            }

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MsSqlCon"].ConnectionString))
            {
                List<Requirement> requirements = null;

                DbModels.Requirement modelRequirement =
                    conn.Query<DbModels.Requirement>(DbModels.Requirement.GetSqlForSelectByPrimaryKeys(requirementId))
                        .FirstOrDefault();

                if (modelRequirement != null)
                {
                    requirements = new List<Requirement>();

                    var requirement = buildFromModel(modelRequirement);
                    if (requirement != null)
                    {
                        requirements.Add(requirement);
                    }
                }

                return new RespEntity() { Code = 1, Message = "", Requirements = requirements };
            }
        }
        
        [HttpPost]
        public RespEntity Respond(ResponseRecord responseRecord)
        {
            if (responseRecord == null || responseRecord.RequirementId <= 0 || responseRecord.ResponserId <= 0 ||
                string.IsNullOrEmpty(responseRecord.Content))
            {
                return new RespEntity() { Code = -1, Message = "传入参数错误" };
            }

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MsSqlCon"].ConnectionString))
            {
                DbModels.Customer responser =
                    conn.Query<DbModels.Customer>(
                        DbModels.Customer.GetSqlForSelectByPrimaryKeys(responseRecord.ResponserId))
                        .FirstOrDefault();

                if (responser == null)
                {
                    return new RespEntity() { Code = -1, Message = "用户不存在" };
                }

                DbModels.Requirement requirement =
                    conn.Query<DbModels.Requirement>(
                        DbModels.Requirement.GetSqlForSelectByPrimaryKeys(responseRecord.RequirementId)).FirstOrDefault();

                if (requirement == null)
                {
                    return new RespEntity() { Code = -1, Message = "需求不存在" };
                }

                if (requirement.CustomerId == responser.CustomerId)
                {
                    return new RespEntity() { Code = -1, Message = "不能响应自己的需求" };
                }

                DbModels.ResponseRecord modelResponseRecord = new DbModels.ResponseRecord();
                modelResponseRecord.RequirementId = responseRecord.RequirementId;
                modelResponseRecord.ResponserId = responseRecord.ResponserId;
                modelResponseRecord.Content = responseRecord.Content;
                modelResponseRecord.ContactMan = responseRecord.ContactMan;
                modelResponseRecord.ContactPhone = responseRecord.ContactPhone;
                modelResponseRecord.CreateBy = responser.NickName;
                modelResponseRecord.UpdateBy = responser.NickName;
                modelResponseRecord.CreateDate = DateTime.Now;
                modelResponseRecord.UpdateDate = DateTime.Now;

                string sqlForResponseRecordInsert = DbModels.ResponseRecord.GetSqlForInsert(modelResponseRecord);

                conn.Execute(sqlForResponseRecordInsert);
            }

            return new RespEntity() { Code = 1, Message = "" };
        }
        
        [HttpPost]
        public RespEntity LoadRequirements(string title, int topN = 20)
        {
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MsSqlCon"].ConnectionString))
            {
                List<Requirement> requirements = null;

                string where = "1=1";

                if (!string.IsNullOrEmpty(title.Trim()))
                {
                    where += string.Format(" AND Title LIKE N'{0}'", title.Trim());
                }

                Dictionary<string, bool> orderByDic = new Dictionary<string, bool>();
                orderByDic.Add("RequirementId", false);

                string sqlForSelect = DbModels.Requirement.GetSqlForSelect(where, orderByDic, topN);

                List<DbModels.Requirement> modelRequirements = conn.Query<DbModels.Requirement>(sqlForSelect).ToList();


                if (modelRequirements != null && modelRequirements.Count > 0)
                {
                    requirements = new List<Requirement>();
                    foreach (var modelRequirement in modelRequirements)
                    {
                        var requirement = buildFromModel(modelRequirement);

                        if (requirement != null)
                        {
                            requirements.Add(requirement);
                        }
                    }
                }

                return new RespEntity() { Code = 1, Message = "", Requirements = requirements };
            }
        }

        private Requirement buildFromModel(DbModels.Requirement modelRequirement)
        {
            if (modelRequirement == null)
            {
                return null;
            }

            Requirement requirement = new Requirement()
            {
                RequirementId = modelRequirement.RequirementId,
                CustomerId = modelRequirement.CustomerId,
                Title = modelRequirement.Title,
                Content = modelRequirement.Content,
                Address = modelRequirement.Address,
                Longitude = modelRequirement.Longitude,
                Latitude = modelRequirement.Latitude,
                ContactPhone = modelRequirement.ContactPhone,
                ContactMan = modelRequirement.ContactMan,
                ReleaseDate =
                                modelRequirement.ReleaseDate.HasValue
                                    ? modelRequirement.ReleaseDate.Value.ToString("yyyy-MM-dd HH:mm:ss")
                                    : string.Empty,
                CreateBy = modelRequirement.CreateBy,
                UpdateBy = modelRequirement.UpdateBy,
                CreateDate = modelRequirement.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"),
                UpdateDate = modelRequirement.UpdateDate.ToString("yyyy-MM-dd HH:mm:ss")
            };

            return requirement;
        }
    }
}