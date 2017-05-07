using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Ms.ApiModel;
using Dapper;

namespace Ms
{
    public class RequirementController : ApiController
    {
        [Route("Requirement/CreateRequirement")]
        [HttpPost]
        public RespEntity CreateRequirement(Requirement requirement)
        {
            if (requirement == null || requirement.CustomerId <= 0 || string.IsNullOrEmpty(requirement.Title) ||
                string.IsNullOrEmpty(requirement.Content))
            {
                return new RespEntity() {Code = -1, Message = "传入参数错误"};
            }

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MsSqlCon"].ConnectionString))
            {
                DbModel.Customer customer =
                    conn.Query<DbModel.Customer>(DbModel.Customer.GetSqlForSelectByPrimaryKeys(requirement.CustomerId))
                        .FirstOrDefault();

                if (customer == null)
                {
                    return new RespEntity() {Code = -1, Message = "用户不存在"};
                }

                DbModel.Requirement modelRequirement = new DbModel.Requirement();
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

                string sqlForRequirementInsert = DbModel.Requirement.GetSqlForInsert(modelRequirement);

                conn.Execute(sqlForRequirementInsert);
            }
            
            return new RespEntity() {Code = 1, Message = ""};
        }

        [Route("Requirement/GetRequirement")]
        [HttpPost]
        public RespEntity GetRequirement(int requirementId)
        {
            if (requirementId <= 0)
            {
                return new RespEntity() {Code = -1, Message = "传入参数错误"};
            }

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MsSqlCon"].ConnectionString))
            {
                List<Requirement> requirements = null;

                DbModel.Requirement requirement =
                    conn.Query<DbModel.Requirement>(DbModel.Requirement.GetSqlForSelectByPrimaryKeys(requirementId))
                        .FirstOrDefault();

                if (requirement != null)
                {
                    requirements = new List<Requirement>()
                    {
                        new Requirement()
                        {
                            RequirementId = requirement.RequirementId,
                            CustomerId = requirement.CustomerId,
                            Title = requirement.Title,
                            Content = requirement.Content,
                            Address = requirement.Address,
                            Longitude = requirement.Longitude,
                            Latitude = requirement.Latitude,
                            ContactPhone = requirement.ContactPhone,
                            ContactMan = requirement.ContactMan,
                            ReleaseDate =
                                requirement.ReleaseDate.HasValue
                                    ? requirement.ReleaseDate.Value.ToString("yyyy-MM-dd HH:mm:ss")
                                    : string.Empty,
                            CreateBy = requirement.CreateBy,
                            UpdateBy = requirement.UpdateBy,
                            CreateDate = requirement.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"),
                            UpdateDate = requirement.UpdateDate.ToString("yyyy-MM-dd HH:mm:ss")
                        }
                    };
                }

                return new RespEntity() {Code = 1, Message = "", Requirements = requirements};
            }
        }

        [Route("Requirement/Respond")]
        [HttpPost]
        public RespEntity Respond(ResponseRecord responseRecord)
        {
            if (responseRecord == null || responseRecord.RequirementId <= 0 || responseRecord.ResponserId <= 0 ||
                string.IsNullOrEmpty(responseRecord.Content))
            {
                return new RespEntity() {Code = -1, Message = "传入参数错误"};
            }

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MsSqlCon"].ConnectionString))
            {
                DbModel.Customer responser =
                    conn.Query<DbModel.Customer>(
                        DbModel.Customer.GetSqlForSelectByPrimaryKeys(responseRecord.ResponserId))
                        .FirstOrDefault();

                if (responser == null)
                {
                    return new RespEntity() {Code = -1, Message = "用户不存在"};
                }

                DbModel.Requirement requirement =
                    conn.Query<DbModel.Requirement>(
                        DbModel.Requirement.GetSqlForSelectByPrimaryKeys(responseRecord.RequirementId)).FirstOrDefault();

                if (requirement == null)
                {
                    return new RespEntity() {Code = -1, Message = "需求不存在"};
                }

                if (requirement.CustomerId == responser.CustomerId)
                {
                    return new RespEntity() {Code = -1, Message = "不能响应自己的需求"};
                }

                DbModel.ResponseRecord modelResponseRecord = new DbModel.ResponseRecord();
                modelResponseRecord.RequirementId = responseRecord.RequirementId;
                modelResponseRecord.ResponserId = responseRecord.ResponserId;
                modelResponseRecord.Content = responseRecord.Content;
                modelResponseRecord.ContactMan = responseRecord.ContactMan;
                modelResponseRecord.ContactPhone = responseRecord.ContactPhone;
                modelResponseRecord.CreateBy = responser.NickName;
                modelResponseRecord.UpdateBy = responser.NickName;
                modelResponseRecord.CreateDate = DateTime.Now;
                modelResponseRecord.UpdateDate = DateTime.Now;

                string sqlForResponseRecordInsert = DbModel.ResponseRecord.GetSqlForInsert(modelResponseRecord);

                conn.Execute(sqlForResponseRecordInsert);
            }

            return new RespEntity() {Code = 1, Message = ""};
        }
    }
}
