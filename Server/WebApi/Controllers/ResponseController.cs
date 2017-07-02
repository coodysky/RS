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

namespace WebApi.Controllers
{
    public class ResponseController : ApiController
    {
        #region 接口方法

        /// <summary>
        /// 查询用户最近的响应记录
        /// </summary>
        /// <param name="customerId">用户Id</param>
        /// <param name="TopN">查询N条记录</param>
        /// <returns></returns>
        [HttpPost]
        public RespEntityResponseRecord LoadResponseRecordsByCustomerId(int customerId, int TopN = 20)
        {
            if (customerId <= 0)
            {
                return new RespEntityResponseRecord() { Code = -1, Message = "参数错误" };
            }

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MsSqlCon"].ConnectionString))
            {
                List<ResponseRecord> records = null;

                string where = string.Format("ResponserId = {0}", customerId);


                Dictionary<string, bool> orderByDic = new Dictionary<string, bool>();
                orderByDic.Add("ResponseRecordId", false);

                string sqlForSelect = DbModel.Models.ResponseRecord.GetSqlForSelect(where, orderByDic, TopN);

                List<DbModel.Models.ResponseRecord> modelRecordss = conn.Query<DbModel.Models.ResponseRecord>(sqlForSelect).ToList();


                if (modelRecordss != null && modelRecordss.Count > 0)
                {
                    records = new List<ResponseRecord>();
                    foreach (var modelRecord in modelRecordss)
                    {
                        var record = buildApiModelForResponse(modelRecord);

                        if (record != null)
                        {
                            records.Add(record);
                        }
                    }
                }

                return new RespEntityResponseRecord() { Code = 1, Message = "", ResponseRecords = records };
            }
        }

        /// <summary>
        /// 查询指定需求对应的相应记录
        /// </summary>
        /// <param name="requirementId">需求Id</param>
        /// <param name="TopN">查询N条记录</param>
        /// <returns></returns>
        [HttpPost]
        public RespEntityResponseRecord LoadResponseRecordsByRequirementId(int requirementId, int TopN = 20)
        {
            if (requirementId <= 0)
            {
                return new RespEntityResponseRecord() { Code = -1, Message = "参数错误" };
            }

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MsSqlCon"].ConnectionString))
            {
                List<ResponseRecord> records = null;

                string where = string.Format("RequirementId = {0}", requirementId);


                Dictionary<string, bool> orderByDic = new Dictionary<string, bool>();
                orderByDic.Add("ResponseRecordId", false);

                string sqlForSelect = DbModel.Models.ResponseRecord.GetSqlForSelect(where, orderByDic, TopN);

                List<DbModel.Models.ResponseRecord> modelRecordss = conn.Query<DbModel.Models.ResponseRecord>(sqlForSelect).ToList();


                if (modelRecordss != null && modelRecordss.Count > 0)
                {
                    records = new List<ResponseRecord>();
                    foreach (var modelRecord in modelRecordss)
                    {
                        var record = buildApiModelForResponse(modelRecord);

                        if (record != null)
                        {
                            records.Add(record);
                        }
                    }
                }

                return new RespEntityResponseRecord() { Code = 1, Message = "", ResponseRecords = records };
            }
        }

        #endregion

        #region build

        private ResponseRecord buildApiModelForResponse(DbModels.ResponseRecord modelRecord)
        {
            if (modelRecord == null)
            {
                return null;
            }

            ResponseRecord record = new ResponseRecord()
            {
                ResponseRecordId = modelRecord.ResponseRecordId,
                RequirementId = modelRecord.RequirementId,
                ResponserId = modelRecord.ResponserId,
                Content = modelRecord.Content,
                ContactPhone = modelRecord.ContactPhone,
                ContactMan = modelRecord.ContactMan,
                IsDeleted = modelRecord.IsDeleted,
                IsFinalServeRecord = modelRecord.IsFinalServeRecord,
                CreateBy = modelRecord.CreateBy,
                CreateDate = modelRecord.CreateDate.ToStringDate(),
                UpdateBy = modelRecord.UpdateBy,
                UpdateDate = modelRecord.UpdateDate.ToStringDate()
            };

            return record;
        }

        #endregion
    }
}
