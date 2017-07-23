using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dapper;
using DbModel.Extension;
using WebApi.Models;
using WebApi.Models.Resp;
using DbModels = DbModel.Models;
using DbCusttomModels=DbModel.CustomModels;

namespace WebApi.Controllers
{
    public class ResponseController : ApiController
    {
        #region 接口方法

        /// <summary>
        /// 创建响应
        /// </summary>
        /// <param name="responseRecord"></param>
        /// <returns></returns>
        [HttpPost]
        public UI_RespEntity CreateResponse(UI_ResponseRecord responseRecord)
        {
            if (responseRecord == null || responseRecord.RequirementId <= 0 || responseRecord.ResponserId <= 0 ||
                string.IsNullOrEmpty(responseRecord.Title)|| string.IsNullOrEmpty(responseRecord.Content))
            {
                return new UI_RespEntity() { Code = -1, Message = "传入参数错误" };
            }

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MsSqlCon"].ConnectionString))
            {
                DbModels.Customer responser =
                    conn.Query<DbModels.Customer>(
                        DbModels.Customer.GetSqlForSelectByPrimaryKeys(responseRecord.ResponserId))
                        .FirstOrDefault();

                if (responser == null)
                {
                    return new UI_RespEntity() { Code = -1, Message = "用户不存在" };
                }

                DbModels.Requirement requirement =
                    conn.Query<DbModels.Requirement>(
                        DbModels.Requirement.GetSqlForSelectByPrimaryKeys(responseRecord.RequirementId)).FirstOrDefault();

                if (requirement == null)
                {
                    return new UI_RespEntity() { Code = -1, Message = "需求不存在" };
                }

                if (requirement.CustomerId == responser.CustomerId)
                {
                    return new UI_RespEntity() { Code = -1, Message = "不能响应自己的需求" };
                }

                DbModels.ResponseRecord modelResponseRecord = new DbModels.ResponseRecord();
                modelResponseRecord.RequirementId = responseRecord.RequirementId;
                modelResponseRecord.ResponserId = responseRecord.ResponserId;
                modelResponseRecord.Content = responseRecord.Content;
                modelResponseRecord.ContactMan = responseRecord.ContactMan;
                modelResponseRecord.ContactPhone = responseRecord.ContactPhone;
                modelResponseRecord.Price = responseRecord.Price;
                modelResponseRecord.Title = responseRecord.Title;
                modelResponseRecord.CreateBy = responser.NickName;
                modelResponseRecord.UpdateBy = responser.NickName;
                modelResponseRecord.CreateDate = DateTime.Now;
                modelResponseRecord.UpdateDate = DateTime.Now;

                string sqlForResponseRecordInsert = DbModels.ResponseRecord.GetSqlForInsert(modelResponseRecord);

                conn.Execute(sqlForResponseRecordInsert);
            }

            return new UI_RespEntity() { Code = 1, Message = "" };
        }

        /// <summary>
        /// 查询用户最近的响应记录
        /// </summary>
        /// <param name="customerId">用户Id</param>
        /// <param name="TopN">查询N条记录</param>
        /// <returns></returns>
        [HttpPost]
        public UI_RespEntityResponseRecord LoadResponseRecordsByCustomerId(int customerId, int TopN = 20)
        {
            if (customerId <= 0)
            {
                return new UI_RespEntityResponseRecord() { Code = -1, Message = "参数错误" };
            }

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MsSqlCon"].ConnectionString))
            {
                List<UI_ResponseRecord> records = null;

                string sqlQuery = string.Format(@"
                    SELECT TOP {0}
                            RR.* ,
                            R.Title AS RequirementTitle
                    FROM    dbo.ResponseRecord RR WITH ( NOLOCK )
                            INNER JOIN dbo.Requirement R WITH ( NOLOCK ) ON RR.RequirementId = R.RequirementId
                    WHERE   RR.ResponserId = @ResponserId
                    ", TopN);

                var modelRecords = conn.Query<DbModel.CustomModels.CustomResponseRecord>(sqlQuery,
                    new {ResponserId = customerId}).ToList();

                if (modelRecords != null && modelRecords.Count > 0)
                {
                    records = new List<UI_ResponseRecord>();
                    foreach (var modelRecord in modelRecords)
                    {
                        var record = buildApiModelForResponse(null, modelRecord);

                        if (record != null)
                        {
                            records.Add(record);
                        }
                    }
                }

                return new UI_RespEntityResponseRecord() {Code = 1, Message = "", ResponseRecords = records};
            }
        }

        /// <summary>
        /// 查询指定需求对应的相应记录
        /// </summary>
        /// <param name="requirementId">需求Id</param>
        /// <param name="TopN">查询N条记录</param>
        /// <returns></returns>
        [HttpPost]
        public UI_RespEntityResponseRecord LoadResponseRecordsByRequirementId(int requirementId, int TopN = 20)
        {
            if (requirementId <= 0)
            {
                return new UI_RespEntityResponseRecord() { Code = -1, Message = "参数错误" };
            }

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MsSqlCon"].ConnectionString))
            {
                List<UI_ResponseRecord> records = null;

                string where = string.Format("RequirementId = {0}", requirementId);


                Dictionary<string, bool> orderByDic = new Dictionary<string, bool>();
                orderByDic.Add("ResponseRecordId", false);

                string sqlForSelect = DbModel.Models.ResponseRecord.GetSqlForSelect(where, orderByDic, TopN);

                List<DbModel.Models.ResponseRecord> modelRecordss = conn.Query<DbModel.Models.ResponseRecord>(sqlForSelect).ToList();


                if (modelRecordss != null && modelRecordss.Count > 0)
                {
                    records = new List<UI_ResponseRecord>();
                    foreach (var modelRecord in modelRecordss)
                    {
                        var record = buildApiModelForResponse(modelRecord, null);

                        if (record != null)
                        {
                            records.Add(record);
                        }
                    }
                }

                return new UI_RespEntityResponseRecord() { Code = 1, Message = "", ResponseRecords = records };
            }
        }

        #endregion

        #region build

        /// <summary>
        /// 传入的对象只能传其一，其余传null
        /// </summary>
        /// <param name="modelRecord1">ResponseRecord单表对象</param>
        /// <param name="modelRecord2">ResponseRecord单表对象，及Requirement</param>
        /// <returns></returns>
        private UI_ResponseRecord buildApiModelForResponse(DbModels.ResponseRecord modelRecord1, DbCusttomModels.CustomResponseRecord modelRecord2)
        {
            UI_ResponseRecord record = null;

            if (modelRecord1 != null)
            {
                record = new UI_ResponseRecord()
                {
                    ResponseRecordId = modelRecord1.ResponseRecordId,
                    RequirementId = modelRecord1.RequirementId,
                    ResponserId = modelRecord1.ResponserId,
                    Content = modelRecord1.Content,
                    ContactPhone = modelRecord1.ContactPhone,
                    ContactMan = modelRecord1.ContactMan,
                    IsDeleted = modelRecord1.IsDeleted,
                    IsFinalServeRecord = modelRecord1.IsFinalServeRecord,
                    CreateBy = modelRecord1.CreateBy,
                    CreateDate = modelRecord1.CreateDate.ToStringDate(),
                    UpdateBy = modelRecord1.UpdateBy,
                    UpdateDate = modelRecord1.UpdateDate.ToStringDate()
                };
            }
            else if (modelRecord2 != null)
            {
                record = new UI_ResponseRecord()
                {
                    ResponseRecordId = modelRecord2.ResponseRecordId,
                    RequirementId = modelRecord2.RequirementId,
                    ResponserId = modelRecord2.ResponserId,
                    Content = modelRecord2.Content,
                    ContactPhone = modelRecord2.ContactPhone,
                    ContactMan = modelRecord2.ContactMan,
                    IsDeleted = modelRecord2.IsDeleted,
                    IsFinalServeRecord = modelRecord2.IsFinalServeRecord,
                    CreateBy = modelRecord2.CreateBy,
                    CreateDate = modelRecord2.CreateDate.ToStringDate(),
                    UpdateBy = modelRecord2.UpdateBy,
                    UpdateDate = modelRecord2.UpdateDate.ToStringDate(),

                    RequirementTitle = modelRecord2.RequirementTitle
                };
            }


            return record;
        }

        #endregion
    }
}
