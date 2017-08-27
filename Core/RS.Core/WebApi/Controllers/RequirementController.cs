using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.ReqResp.Requirement;
using MySql.Data.MySqlClient;
using Dapper;
using System.Data;

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

        ///// <summary>
        ///// 通过需求Id获取单个需求
        ///// </summary>
        ///// <param name="requirementId"></param>
        ///// <param name="isNeedTags"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public UI_RespEntityRequirement GetRequirement(int requirementId, bool isNeedTags = false)
        //{
        //    if (requirementId <= 0)
        //    {
        //        return new UI_RespEntityRequirement() { Code = -1, Message = "传入参数错误" };
        //    }

        //    using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MsSqlCon"].ConnectionString))
        //    {
        //        List<UI_Requirement> requirements = null;

        //        DbModels.Requirement modelRequirement =
        //            conn.Query<DbModels.Requirement>(DbModels.Requirement.GetSqlForSelectByPrimaryKeys(requirementId))
        //                .FirstOrDefault();

        //        if (modelRequirement != null)
        //        {
        //            requirements = new List<UI_Requirement>();

        //            var requirement = buildApiModelForRequirement(modelRequirement);
        //            if (requirement != null)
        //            {
        //                List<DbModels.RequirementTag> modelTags =
        //                conn.Query<DbModels.RequirementTag>(
        //                    DbModels.RequirementTag.GetSqlForSelect(
        //                        string.Format("RequirementId = {0}", requirementId), null, 0)).ToList();

        //                List<UI_RequirementTag> tags = new List<UI_RequirementTag>();
        //                foreach (var modelTag in modelTags)
        //                {
        //                    UI_RequirementTag tag = buildApiModelForRequirementTag(modelTag);
        //                    if (tag != null)
        //                    {
        //                        tags.Add(tag);
        //                    }
        //                }

        //                if (tags != null && tags.Count > 0)
        //                    requirement.RequirementTags = tags;

        //                requirements.Add(requirement);
        //            }
        //        }

        //        return new UI_RespEntityRequirement() { Code = 1, Message = "", Requirements = requirements };
        //    }
        //}

        ///// <summary>
        ///// 通过Title搜索需求，like模糊查询
        ///// </summary>
        ///// <param name="Title"></param>
        ///// <param name="TopN"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public UI_RespEntityRequirement LoadRequirements(string Title, int TopN = 20)
        //{
        //    string title = Title.Trim();

        //    if (string.IsNullOrEmpty(title))
        //    {
        //        return new UI_RespEntityRequirement() { Code = -1, Message = "标题为空" };
        //    }

        //    using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MsSqlCon"].ConnectionString))
        //    {
        //        List<UI_Requirement> requirements = null;

        //        string where = "1=1";

        //        if (!string.IsNullOrEmpty(title.Trim()))
        //        {
        //            where += string.Format(" AND Title LIKE N'{0}'", title);
        //        }

        //        Dictionary<string, bool> orderByDic = new Dictionary<string, bool>();
        //        orderByDic.Add("RequirementId", false);

        //        string sqlForSelect = DbModels.Requirement.GetSqlForSelect(where, orderByDic, TopN);

        //        List<DbModels.Requirement> modelRequirements = conn.Query<DbModels.Requirement>(sqlForSelect).ToList();


        //        if (modelRequirements != null && modelRequirements.Count > 0)
        //        {
        //            requirements = new List<UI_Requirement>();
        //            foreach (var modelRequirement in modelRequirements)
        //            {
        //                var requirement = buildApiModelForRequirement(modelRequirement);

        //                if (requirement != null)
        //                {
        //                    requirements.Add(requirement);
        //                }
        //            }
        //        }

        //        return new UI_RespEntityRequirement() { Code = 1, Message = "", Requirements = requirements };
        //    }
        //}

        ///// <summary>
        ///// 通过经纬度查询附近多个需求
        ///// </summary>
        ///// <param name="Longitude"></param>
        ///// <param name="Latitude"></param>
        ///// <param name="TopN"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public UI_RespEntityRequirement LoadNearRequirements(decimal Longitude, decimal Latitude, int TopN = 20)
        //{
        //    using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MsSqlCon"].ConnectionString))
        //    {
        //        List<UI_Requirement> requirements = null;

        //        Dictionary<string, bool> orderBy = new Dictionary<string, bool>();
        //        orderBy.Add(string.Format("ABS(Longitude - {0}) + ABS(Latitude - {1})", Longitude, Latitude), true);

        //        string sqlForSelect = DbModels.Requirement.GetSqlForSelect("1=1", orderBy, TopN);

        //        List<DbModels.Requirement> modelRequirements = conn.Query<DbModels.Requirement>(sqlForSelect).ToList();

        //        if (modelRequirements != null && modelRequirements.Count > 0)
        //        {
        //            requirements = new List<UI_Requirement>();
        //            foreach (var modelRequirement in modelRequirements)
        //            {
        //                var requirement = buildApiModelForRequirement(modelRequirement);

        //                if (requirement != null)
        //                {
        //                    requirements.Add(requirement);
        //                }
        //            }
        //        }

        //        return new UI_RespEntityRequirement() { Code = 1, Message = "", Requirements = requirements };
        //    }
        //}

        #endregion

        #region build

        //private UI_Requirement buildApiModelForRequirement(DbModels.Requirement modelRequirement)
        //{
        //    if (modelRequirement == null)
        //    {
        //        return null;
        //    }

        //    UI_Requirement requirement = new UI_Requirement()
        //    {
        //        RequirementId = modelRequirement.RequirementId,
        //        CustomerId = modelRequirement.CustomerId,
        //        Title = modelRequirement.Title,
        //        Price = modelRequirement.Price,
        //        Content = modelRequirement.Content,
        //        Address = modelRequirement.Address,
        //        Longitude = modelRequirement.Longitude,
        //        Latitude = modelRequirement.Latitude,
        //        ContactPhone = modelRequirement.ContactPhone,
        //        ContactMan = modelRequirement.ContactMan,
        //        ReleaseDate = modelRequirement.ReleaseDate.ToStringDate(),
        //        CreateBy = modelRequirement.CreateBy,
        //        UpdateBy = modelRequirement.UpdateBy,
        //        CreateDate = modelRequirement.CreateDate.ToStringDate(),
        //        UpdateDate = modelRequirement.UpdateDate.ToStringDate()
        //    };

        //    return requirement;
        //}

        //private UI_RequirementTag buildApiModelForRequirementTag(DbModels.RequirementTag modelTag)
        //{
        //    if (modelTag == null)
        //    {
        //        return null;
        //    }

        //    UI_RequirementTag tag = new UI_RequirementTag()
        //    {
        //        RequirementTagId = modelTag.RequirementTagId,
        //        RequirementId = modelTag.RequirementId,
        //        Tag = modelTag.Tag,
        //        CreateBy = modelTag.CreateBy,
        //        UpdateBy = modelTag.UpdateBy,
        //        CreateDate = modelTag.CreateDate.ToStringDate(),
        //        UpdateDate = modelTag.UpdateDate.ToStringDate()
        //    };

        //    return tag;
        //}

        #endregion
    }
}
