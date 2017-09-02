using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.ReqResp.Requirement;
using MySql.Data.MySqlClient;
using Dapper;
using System.Data;
using DbModel.Extension;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class RequirementController : Controller
    {
        string connMysql = "读取数据库连接字符串";

        #region 接口方法

        /// <summary>
        /// 创建需求
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Resp_CreateRequirement CreateRequirement(Req_CreateRequirement req)
        {
            if (req == null || req.CustomerId <= 0 ||
                string.IsNullOrEmpty(req.Title) ||
                string.IsNullOrEmpty(req.Content))
            {
                return new Resp_CreateRequirement() { Code = -1, Message = "传入参数错误" };
            }

            Resp_CreateRequirement resp = new Resp_CreateRequirement();

            using (var conn = new MySqlConnection(connMysql))
            {
                #region sqlCustomer

                string sqlCustomer = @"select NickName from Customer where CustomerId = @CustomerId";

                #endregion

                string customerNickName = conn.ExecuteScalar<string>(sqlCustomer, new { CustomerId = req.CustomerId });

                if (string.IsNullOrEmpty(customerNickName))
                {
                    return new Resp_CreateRequirement() { Code = -1, Message = "用户不存在" };
                }
                
                //添加事务
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                IDbTransaction tran = conn.BeginTransaction();

                #region sqlRequirement

                string sqlRequirement = @"
insert into Requirement
    (
        CustomerId,
        Title,
        Price,
        Content,
        Address,
        Latitude,
        Longitude,
        ContactPhone,
        ContactMan,
        RequirementStatusCode,
        CreateBy,
        UpdateBy,
        CreateDate,
        UpdateDate
    )
    select  @CustomerId,
            @Title,
            @Price,
            @Content,
            @Address,
            @Latitude,
            @Longitude,
            @ContactPhone,
            @ContactMan,
            @RequirementStatusCode,
            @NickName,
            @NickName,
            unix_timestamp(),
            unix_timestamp();

select @@IDENTITY as RequirementId;

";

                #endregion

                int requirementId = conn.ExecuteScalar<int>(sqlRequirement, new
                {
                    CustomerId = req.CustomerId,
                    Title = req.Title,
                    Price = req.Price,
                    Content = req.Content,
                    Address = req.Address,
                    Latitude = req.Latitude,
                    Longitude = req.Longitude,
                    ContactPhone = req.ContactPhone,
                    ContactMan = req.ContactMan,
                    RequirementStatusCode = "Init",
                    NickName = customerNickName
                }, transaction: tran);

                if (requirementId > 0)
                {
                    resp.RequirementId = requirementId;

                    #region 插入Tag

                    if (req.Tags != null && req.Tags.Count > 0)
                    {
                        foreach (var tag in req.Tags)
                        {
                            if (string.IsNullOrEmpty(tag))
                                continue;

                            #region sqlTag

                            string sqlTag = @"
insert into RequirementTag
    (
        RequirementId,
        Tag,
        CreateBy,
        UpdateBy,
        CreateDate,
        UpdateDate
    )
    select  @RequirementId,
            @Tag,
            @NickName,
            @NickName,
            unix_timestamp(),
            unix_timestamp()
";

                            #endregion

                            conn.Execute(sqlTag, new
                            {
                                RequirementId = requirementId,
                                Tag = tag,
                                NickName = customerNickName
                            }, transaction: tran);
                        }
                    }

                    #endregion
                }

                tran.Commit();

                return resp;
            }
        }

        /// <summary>
        /// 通过需求Id获取单个需求
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Resp_GetRequirementById GetRequirementById(Req_GetRequirementById req)
        {
            if (req == null || req.RequirementId <= 0)
            {
                return new Resp_GetRequirementById() { Code = -1, Message = "传入参数错误" };
            }

            Resp_GetRequirementById resp = new Resp_GetRequirementById();

            using (var conn = new MySqlConnection(connMysql))
            {
                #region sqlRequirement

                string sqlRequirement = @"
select 
    RequirementId,
    CustomerId,
    Title,
    Content,
    Price,
    Address,
    Longitude,
    Latitude,
    ContactPhone,
    ContactMan,
    RequirementStatusCode,
    ReleaseDate,
    CreateBy,
    UpdateBy,
    CreateDate,
    UpdateDate
from Requirement
where RequirementId = @RequirementId
";

                #endregion

                var dbRequirement = conn.Query<DbModel.Models.Requirement>(sqlRequirement, new
                {
                    RequirementId = req.RequirementId
                }).FirstOrDefault();

                if (dbRequirement != null)
                {
                    var requirement = buildRespEntity(dbRequirement);
                    if (requirement != null && req.IsNeedTags)
                    {
                        #region sqlRequirementTag

                        string sqlRequirementTag = @"
select Tag
from RequirementTag
where RequirementId = @RequirementId
";

                        #endregion
                        
                        var tags = conn.Query<string>(sqlRequirementTag, new
                        {
                            RequirementId = req.RequirementId
                        }).ToList();

                        if (tags != null && tags.Count > 0)
                        {
                            requirement.Tags = tags;
                        }

                        resp.Requirement = requirement;
                    }
                }

                return resp;
            }
        }

        /// <summary>
        /// 通过Title搜索需求，like模糊查询
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Resp_LoadRequirementsByTitle LoadRequirementsByTitle(Req_LoadRequirementsByTitle req)
        {
            if (req == null || string.IsNullOrEmpty(req.Title.Trim()))
            {
                return new Resp_LoadRequirementsByTitle() { Code = -1, Message = "传入参数错误" };
            }

            string titleInput = req.Title.Trim();

            Resp_LoadRequirementsByTitle resp = new Resp_LoadRequirementsByTitle();

            using (var conn = new MySqlConnection(connMysql))
            {
                #region sql

                string sql = @"
create temporary table temp_requirement_ids
(     
    requirement_id int NOT NULL   
);

insert into temp_requirement_ids
(
    requirement_id
)
select 
    RequirementId
from Requirement
where Title like @Title
limit @TopN;

select
    RequirementId,
    CustomerId,
    Title,
    Content,
    Price,
    Address,
    Longitude,
    Latitude,
    ContactPhone,
    ContactMan,
    RequirementStatusCode,
    ReleaseDate,
    CreateBy,
    UpdateBy,
    CreateDate,
    UpdateDate
from Requirement r
inner join temp_requirement_ids ris on r.RequirementId = ris.requirement_id;

select
    rt.RequirementId,
    rt.Tag
from Requirement r
inner join temp_requirement_ids ris on r.RequirementId = ris.requirement_id
inner join RequirementTag rt on r.RequirementId = rt.RequirementId;
";

                #endregion

                var result = conn.QueryMultiple(sql, new
                {
                    Title = titleInput,
                    TopN = @req.TopN
                });

                var dbRequirements = result.Read<DbModel.Models.Requirement>().ToList();
                var dbTags = result.Read<DbModel.Models.RequirementTag>().ToList();

                if (dbRequirements != null && dbRequirements.Count > 0)
                {
                    foreach (var dbRequirement in dbRequirements)
                    {
                        var respRequirement = buildRespEntity(dbRequirement);
                        if (respRequirement != null)
                        {
                            if (dbTags != null && dbTags.Count > 0)
                            {
                                foreach (var dbTag in dbTags)
                                {
                                    if (dbTag.RequirementId == respRequirement.RequirementId)
                                    {
                                        respRequirement.Tags.Add(dbTag.Tag);
                                    }
                                }
                            }

                            resp.Requirements.Add(respRequirement);
                        }
                    }
                }

                return resp;
            }
        }

        /// <summary>
        /// 通过经纬度查询附近多个需求
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Resp_LoadNearRequirements LoadNearRequirements(Req_LoadNearRequirements req)
        {
            Resp_LoadNearRequirements resp = new Resp_LoadNearRequirements();

            using (var conn = new MySqlConnection(connMysql))
            {
                #region sql

                string sql = @"
create temporary table temp_requirement_ids
(     
    requirement_id int NOT NULL   
);

insert into temp_requirement_ids
(
    requirement_id
)
select 
    RequirementId
from Requirement
order by ABS(Longitude - @Longitude) + ABS(Latitude - @Latitude) asc
limit @TopN;

select
    RequirementId,
    CustomerId,
    Title,
    Content,
    Price,
    Address,
    Longitude,
    Latitude,
    ContactPhone,
    ContactMan,
    RequirementStatusCode,
    ReleaseDate,
    CreateBy,
    UpdateBy,
    CreateDate,
    UpdateDate
from Requirement r
inner join temp_requirement_ids ris on r.RequirementId = ris.requirement_id;

select
    rt.RequirementId,
    rt.Tag
from Requirement r
inner join temp_requirement_ids ris on r.RequirementId = ris.requirement_id
inner join RequirementTag rt on r.RequirementId = rt.RequirementId;
";

                #endregion

                var result = conn.QueryMultiple(sql, new
                {
                    Longitude = req.Longitude,
                    Latitude = req.Latitude,
                    TopN = req.TopN
                });

                var dbRequirements = result.Read<DbModel.Models.Requirement>().ToList();
                var dbTags = result.Read<DbModel.Models.RequirementTag>().ToList();

                if (dbRequirements != null && dbRequirements.Count > 0)
                {
                    foreach (var dbRequirement in dbRequirements)
                    {
                        var respRequirement = buildRespEntity(dbRequirement);
                        if (respRequirement != null)
                        {
                            if (dbTags != null && dbTags.Count > 0)
                            {
                                foreach (var dbTag in dbTags)
                                {
                                    if (dbTag.RequirementId == respRequirement.RequirementId)
                                    {
                                        respRequirement.Tags.Add(dbTag.Tag);
                                    }
                                }
                            }

                            resp.Requirements.Add(respRequirement);
                        }
                    }
                }

                return resp;
            }
        }

        #endregion

        #region build

        private RespEntity_Requirement buildRespEntity(DbModel.Models.Requirement dbRequirement)
        {
            if (dbRequirement == null)
            {
                return null;
            }

            RespEntity_Requirement respRequirement = new RespEntity_Requirement()
            {
                RequirementId = dbRequirement.RequirementId,
                CustomerId = dbRequirement.CustomerId,
                Title = dbRequirement.Title,
                Price = dbRequirement.Price,
                Content = dbRequirement.Content,
                Address = dbRequirement.Address,
                Longitude = dbRequirement.Longitude,
                Latitude = dbRequirement.Latitude,
                ContactPhone = dbRequirement.ContactPhone,
                ContactMan = dbRequirement.ContactMan,
                ReleaseDate = dbRequirement.ReleaseDate.ToStringDateFromLong(),
                CreateBy = dbRequirement.CreateBy,
                UpdateBy = dbRequirement.UpdateBy,
                CreateDate = dbRequirement.CreateDate.ToStringDateFromLong(),
                UpdateDate = dbRequirement.UpdateDate.ToStringDateFromLong()
            };

            return respRequirement;
        }

        #endregion
    }
}
