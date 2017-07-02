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
using DbModels=DbModel.Models;

namespace WebApi.Controllers
{
    public class CustomerController : ApiController
    {
        [HttpPost]
        public RespEntityCustomer CreateCustomer(Customer customer)
        {
            if (customer == null || string.IsNullOrEmpty(customer.NickName) ||
                string.IsNullOrEmpty(customer.RealName) || string.IsNullOrEmpty(customer.Password) ||
                string.IsNullOrEmpty(customer.MobilePhone) || string.IsNullOrEmpty(customer.Email))
            {
                return new RespEntityCustomer() { Code = -1, Message = "传入参数错误" };
            }

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MsSqlCon"].ConnectionString))
            {
                DbModels.Customer modelCustomer = new DbModels.Customer();
                modelCustomer.NickName = customer.NickName;
                modelCustomer.RealName = customer.RealName;
                modelCustomer.Password = customer.Password;
                modelCustomer.MobilePhone = customer.MobilePhone;
                modelCustomer.Email = customer.Email;
                modelCustomer.CreateBy = "Self";
                modelCustomer.UpdateBy = "Self";
                modelCustomer.CreateDate = DateTime.Now;
                modelCustomer.UpdateDate = DateTime.Now;

                string sqlForCustomerInsert = DbModels.Customer.GetSqlForInsert(modelCustomer);

                conn.Execute(sqlForCustomerInsert);
            }

            return new RespEntityCustomer() { Code = 1, Message = "" };
        }
        
        [HttpPost]
        public RespEntityCustomer LoadCustomers(List<int> customerIds )
        {
            if (customerIds==null || customerIds.Count==0)
            {
                return new RespEntityCustomer() {Code = -1, Message = "标题为空"};
            }

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MsSqlCon"].ConnectionString))
            {
                List<Customer> customers = null;
                
                string sqlForSelect = DbModels.Customer.GetSqlForSelect("CustomerId in @CustomerIds", null, 0);

                List<DbModels.Customer> modelCustomers = conn.Query<DbModels.Customer>(sqlForSelect,new { CustomerIds =customerIds}).ToList();


                if (modelCustomers != null && modelCustomers.Count > 0)
                {
                    customers = new List<Customer>();
                    foreach (var modelCustomer in modelCustomers)
                    {
                        var customer = buildFromModel(modelCustomer);

                        if (customer != null)
                        {
                            customers.Add(customer);
                        }
                    }
                }

                return new RespEntityCustomer() { Code = 1, Message = "", Customers = customers };
            }
        }

        private Customer buildFromModel(DbModels.Customer modelCustomer)
        {
            if (modelCustomer == null)
            {
                return null;
            }

            Customer customer = new Customer()
            {
                CustomerId = modelCustomer.CustomerId,
                NickName = modelCustomer.NickName,
                RealName = modelCustomer.RealName,
                Password = modelCustomer.Password,
                MobilePhone = modelCustomer.MobilePhone,
                Email = modelCustomer.Email,
                CreateBy = modelCustomer.CreateBy,
                UpdateBy = modelCustomer.UpdateBy,
                CreateDate = modelCustomer.CreateDate.ToStringDate(),
                UpdateDate = modelCustomer.UpdateDate.ToStringDate()
            };

            return customer;
        }
    }
}
