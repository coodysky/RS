
using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using WebApi.Models.ReqResp.Customer;
using DbModel.Models;
using System.Linq;
using DbModel.Extension;

namespace WebApi.Controllers
{
    public class CustomerController : Controller
    {
        string connMysql = Startup.ConfigAppsetting["ConnectionStrings:Mysql"];

        #region 接口方法

        /// <summary>
        /// 创建客户
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Resp_CreateCustomer CreateCustomer(Req_CreateCustomer req)
        {
            if (req == null || string.IsNullOrEmpty(req.NickName) ||
                string.IsNullOrEmpty(req.RealName) || string.IsNullOrEmpty(req.Password) ||
                string.IsNullOrEmpty(req.MobilePhone) || string.IsNullOrEmpty(req.Email))
            {
                return new Resp_CreateCustomer() { Code = -1, Message = "传入参数错误" };
            }

            using (var conn = new MySqlConnection(connMysql))
            {
                #region sql

                string sql = @"
insert into Customer(
    NickName,
    RealName,
    Password,
    MobilePhone,
    Email,
    CreateBy,
    UpdateBy,
    CreateDate,
    UpdateDate)
select  @NickName,
        @RealName,
        @Password,
        @MobilePhone,
        @Email,
        `Self`,
        `Self`,
        unix_timestamp(),
        unix_timestamp()
";

                #endregion

                int result = conn.Execute(sql, new
                {
                    NickName = req.NickName,
                    RealName = req.RealName,
                    Password = req.Password,
                    MobilePhone = req.MobilePhone,
                    Email = req.Email
                });
            }

            return new Resp_CreateCustomer() { Code = 1, Message = "" };
        }

        /// <summary>
        /// 通过客户Id列表查询客户列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Resp_LoadCustomersByCustomerIds LoadCustomersByCustomerIds(Req_LoadCustomersByCustomerIds req)
        {
            if (req == null || req.CustomerIds == null || req.CustomerIds.Count == 0)
            {
                return new Resp_LoadCustomersByCustomerIds() { Code = -1, Message = "标题为空" };
            }

            Resp_LoadCustomersByCustomerIds resp = new Resp_LoadCustomersByCustomerIds();

            using (var conn = new MySqlConnection(connMysql))
            {
                #region sql

                string sql = @"
select  CustomerId,
        NickName,
        RealName,
        Password,
        MobilePhone,
        Email,
        CreateBy,
        UpdateBy,
        CreateDate,
        UpdateDate,
from Customer where CustomerId in @CustomerIds
";

                #endregion

                var dbCustomers = conn.Query<Customer>(sql, new { CustomerIds = req.CustomerIds }).ToList();

                if (dbCustomers != null && dbCustomers.Count > 0)
                {
                    foreach (var dbCustomer in dbCustomers)
                    {
                        var respCustomer = buildRespEntity(dbCustomer);

                        if (respCustomer != null)
                        {
                            resp.Customers.Add(respCustomer);
                        }
                    }
                }

                return resp;
            }
        }

        #endregion

        #region build

        private RespEntity_Customer buildRespEntity(Customer dbCustomer)
        {
            if (dbCustomer == null)
            {
                return null;
            }

            RespEntity_Customer respCustomer = new RespEntity_Customer()
            {
                CustomerId = dbCustomer.CustomerId,
                NickName = dbCustomer.NickName,
                RealName = dbCustomer.RealName,
                Password = dbCustomer.Password,
                MobilePhone = dbCustomer.MobilePhone,
                Email = dbCustomer.Email,
                CreateBy = dbCustomer.CreateBy,
                UpdateBy = dbCustomer.UpdateBy,
                CreateDate = dbCustomer.CreateDate.ToStringDateFromLong(),
                UpdateDate = dbCustomer.UpdateDate.ToStringDateFromLong()
            };

            return respCustomer;
        }

        #endregion

    }
}
