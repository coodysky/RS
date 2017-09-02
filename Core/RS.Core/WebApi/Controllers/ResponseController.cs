using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.ReqResp.Response;
using MySql.Data.MySqlClient;
using Dapper;
using System.Data;
using DbModel.Extension;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ResponseController : Controller
    {
        string connMysql = "读取数据库连接字符串";

        #region 接口方法

        /// <summary>
        /// 创建响应
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Resp_CreateResponse CreateResponse(Req_CreateResponse req)
        {
            if (req == null || req.RequirementId <= 0 || req.ResponserId <= 0 ||
                string.IsNullOrEmpty(req.Title)|| string.IsNullOrEmpty(req.Content))
            {
                return new Resp_CreateResponse() { Code = -1, Message = "传入参数错误" };
            }

            Resp_CreateResponse resp = new Resp_CreateResponse();

            using (var conn = new MySqlConnection(connMysql))
            {
                #region sqlCustomer

                string sqlCustomer = @"select NickName from Customer where CustomerId = @CustomerId";

                #endregion

                string customerNickName = conn.ExecuteScalar<string>(sqlCustomer, new { CustomerId = req.ResponserId });

                if (string.IsNullOrEmpty(customerNickName))
                {
                    return new Resp_CreateResponse() { Code = -1, Message = "用户不存在" };
                }

                #region sqlRequirement

                string sqlRequirement = @"select CustomerId from Requirement where RequirementId = @RequirementId";

                #endregion

                int requirementCustomerId = conn.ExecuteScalar<int>(sqlRequirement, new { RequirementId = req.RequirementId });

                if (requirementCustomerId<=0)
                {
                    return new Resp_CreateResponse() { Code = -1, Message = "需求不存在" };
                }
                if (requirementCustomerId == req.ResponserId)
                {
                    return new Resp_CreateResponse() { Code = -1, Message = "不能响应自己的需求" };
                }

                #region sqlRecord

                string sqlRecord = @"
insert into ResponseRecord
    (
        RequirementId,
        ResponserId,
        Content,
        ContactMan,
        ContactPhone,
        Price,
        Title,
        CreateBy,
        UpdateBy,
        CreateDate,
        UpdateDate
    )
    select  @RequirementId,
            @ResponserId,
            @Content,
            @ContactMan,
            @ContactPhone,
            @Price,
            @Title,
            @NickName,
            @NickName,
            unix_timestamp(),
            unix_timestamp()
";

                #endregion

                conn.Execute(sqlRecord, new
                {
                    RequirementId = req.RequirementId,
                    ResponserId = req.ResponserId,
                    Title = req.Title,
                    Content=req.Content,
                    Price=req.Price,
                    ContactMane=req.ContactMan,
                    ContactPhone=req.ContactPhone,
                    NickName=customerNickName
                });
            }

            return resp;
        }

        /// <summary>
        /// 查询用户最近的响应记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Resp_LoadResponseRecordsByCustomerId LoadResponseRecordsByCustomerId(Req_LoadResponseRecordsByCustomerId req)
        {
            if (req == null || req.CustomerId <= 0)
            {
                return new Resp_LoadResponseRecordsByCustomerId() { Code = -1, Message = "参数错误" };
            }

            Resp_LoadResponseRecordsByCustomerId resp = new Resp_LoadResponseRecordsByCustomerId();

            using (var conn = new MySqlConnection(connMysql))
            {
                #region sql

                string sql = @"
create temporary table temp_requirement
(     
    requirement_id int not null,
    title varchar(500) not null,
);

insert into temp_requirement
(
    requirement_id,
    title
)
select
    r.RequirementId,
    r.Title
from Requirement r
inner join ResponseRecord rr on r.RequirementId = rr.RequirementId
where rr.ResponserId = @ResponserId
limit @TopN;

select
    requirement_id as RequirementId,
    title as Title
from temp_requirement;

select  
    rr.ResponseRecordId,
    rr.RequirementId,
    rr.ResponserId,
    rr.Title,
    rr.Price,
    rr.Content,
    rr.ContactPhone,
    rr.ContactMan,
    rr.IsDeleted,
    rr.IsFinalServeRecord,
    rr.CreateBy,
    rr.CreateDate
from temp_requirement t
inner join ResponseRecord rr on t.requirement_id = rr.RequirementId
";

                #endregion

                var result = conn.QueryMultiple(sql, new
                {
                    ResponserId = req.CustomerId,
                    TopN = @req.TopN
                });

                var dbRequirements = result.Read<DbModel.Models.Requirement>().ToList();
                var dbRecords = result.Read<DbModel.Models.ResponseRecord>().ToList();

                if (dbRequirements != null&&dbRequirements.Count>0)
                {
                    foreach (var dbRequirement in dbRequirements)
                    {
                        var respEntityRequirement = buildRespEntityRequirement(dbRequirement);
                        if (respEntityRequirement != null)
                        {
                            Resp_LoadResponseRecordsByCustomerId_Entity entity = new Resp_LoadResponseRecordsByCustomerId_Entity();
                            entity.Requirement = respEntityRequirement;

                            if (dbRecords != null && dbRecords.Count > 0)
                            {
                                foreach (var dbRecord in dbRecords)
                                {
                                    if (dbRecord.RequirementId == respEntityRequirement.RequirementId)
                                    {
                                        var respEntityRecord = buildRespEntityRecord(dbRecord);
                                        if (respEntityRecord != null)
                                        {
                                            entity.Records.Add(respEntityRecord);
                                        }
                                    }
                                }
                            }

                            resp.Entitys.Add(entity);
                        }
                    }
                }
            }

            return resp;
        }

        /// <summary>
        /// 查询指定需求对应的相应记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Resp_LoadResponseRecordsByRequirementId LoadResponseRecordsByRequirementId(Req_LoadResponseRecordsByRequirementId req)
        {
            if (req.RequirementId <= 0)
            {
                return new Resp_LoadResponseRecordsByRequirementId() { Code = -1, Message = "参数错误" };
            }

            Resp_LoadResponseRecordsByRequirementId resp = new Resp_LoadResponseRecordsByRequirementId();

            using (var conn = new MySqlConnection(connMysql))
            {
                #region sql

                string sql = @"
select
    r.RequirementId,
    r.Title
from Requirement r
where r.RequirementId = @RequirementId;

select  
    rr.ResponseRecordId,
    rr.RequirementId,
    rr.ResponserId,
    rr.Title,
    rr.Price,
    rr.Content,
    rr.ContactPhone,
    rr.ContactMan,
    rr.IsDeleted,
    rr.IsFinalServeRecord,
    rr.CreateBy,
    rr.CreateDate
from Requirement r
inner join ResponseRecord rr on r.RequirementId = rr.RequirementId
where r.RequirementId = @RequirementId;
";

                #endregion

                var result = conn.QueryMultiple(sql, new
                {
                    RequirementId = req.RequirementId
                });

                var dbRequirement = result.Read<DbModel.Models.Requirement>().FirstOrDefault();
                var dbRecords = result.Read<DbModel.Models.ResponseRecord>().ToList();

                if (dbRequirement != null)
                {
                    var respEntityRequirement = buildRespEntityRequirement(dbRequirement);
                    if (respEntityRequirement != null)
                    {
                        resp.Requirement = respEntityRequirement;

                        if (dbRecords != null && dbRecords.Count > 0)
                        {
                            foreach (var dbRecord in dbRecords)
                            {
                                var respEntityRecord = buildRespEntityRecord(dbRecord);
                                if (respEntityRecord != null)
                                {
                                    resp.Records.Add(respEntityRecord);
                                }
                            }
                        }
                    }
                }
            }

            return resp;
        }

        #endregion

        #region build

        private RespEntity_Requirement buildRespEntityRequirement(DbModel.Models.Requirement dbRequirement)
        {
            if (dbRequirement == null)
                return null;

            RespEntity_Requirement resp = new RespEntity_Requirement();
            resp.RequirementId = dbRequirement.RequirementId;
            resp.CustomerId = dbRequirement.CustomerId;
            resp.Title = dbRequirement.Title;
            resp.Content = dbRequirement.Content;
            resp.Price = dbRequirement.Price;
            resp.Address = dbRequirement.Address;
            resp.Longitude = dbRequirement.Longitude;
            resp.Latitude = dbRequirement.Latitude;
            resp.ContactPhone = dbRequirement.ContactPhone;
            resp.ContactMan = dbRequirement.ContactMan;
            resp.RequirementStatusCode = dbRequirement.RequirementStatusCode;
            resp.ReleaseDate = dbRequirement.ReleaseDate.ToStringDateFromLong();
            resp.CreateBy = dbRequirement.CreateBy;
            resp.UpdateBy = dbRequirement.UpdateBy;
            resp.CreateDate = dbRequirement.CreateDate.ToStringDateFromLong();
            resp.UpdateDate = dbRequirement.UpdateDate.ToStringDateFromLong();

            return resp;
        }

        private RespEntity_ResponseRecord buildRespEntityRecord(DbModel.Models.ResponseRecord dbRecord)
        {
            if (dbRecord == null)
                return null;

            RespEntity_ResponseRecord resp = new RespEntity_ResponseRecord();
            resp.ResponseRecordId = dbRecord.ResponseRecordId;
            resp.RequirementId = dbRecord.RequirementId;
            resp.ResponserId = dbRecord.ResponserId;
            resp.Title = dbRecord.Title;
            resp.Price = dbRecord.Price;
            resp.Content = dbRecord.Content;
            resp.ContactPhone = dbRecord.ContactPhone;
            resp.ContactMan = dbRecord.ContactMan;
            resp.IsDeleted = dbRecord.IsDeleted;
            resp.IsFinalServeRecord = dbRecord.IsFinalServeRecord;
            resp.CreateBy = dbRecord.CreateBy;
            resp.CreateDate = dbRecord.CreateDate.ToStringDateFromLong();
            resp.UpdateBy = dbRecord.UpdateBy;
            resp.UpdateDate = dbRecord.UpdateDate.ToStringDateFromLong();

            return resp;
        }

        #endregion
    }
}
