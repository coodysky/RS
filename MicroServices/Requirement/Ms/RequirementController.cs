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
                    conn.Query<DbModel.Customer>("SELECT * FROM Customer WITH(NOLOCK) WHERE CustomerId = @CustomerId",
                        new {CustomerId = requirement.CustomerId}).FirstOrDefault();

                if (customer == null)
                {
                    return new RespEntity() { Code = -1, Message = "用户不存在" };
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
                return new RespEntity() { Code = -1, Message = "传入参数错误" };
            }

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MsSqlCon"].ConnectionString))
            {
                List<Requirement> requirements = null;

                DbModel.Requirement requirement =
                    conn.Query<DbModel.Requirement>(
                        "SELECT TOP 1 * FROM Requirement WITH(NOLOCK) WHERE RequirementId = @RequirementId",
                        new {RequirementId = requirementId}).FirstOrDefault();

                if (requirement != null)
                {
                    requirements = new List<Requirement>()
                    {
                        new Requirement()
                        {
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
    }
}
