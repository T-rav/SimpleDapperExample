using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoApplication.Models;
using NUnit.Framework;

namespace Dapper.Demo.Tests
{
    [TestFixture]
    public class DapperDemoTest
    {
        private string _connectionString = "Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;Initial Catalog=DapperDemo";

        [Test]
        public void PopulateSimpleCustomerObject_WhenIssuingSelect_ShouldReturnCollectionOfCustomers()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                //---------------Arrange-------------------
                connection.Open();

                //---------------Act----------------------
                var sql = "select Id,Name,Email,Login,TimeZoneId,ExternalSystemId from dbo.Customers";
                var customers = connection.Query<SimpleCustomer>(sql);

                //---------------Assert-----------------------
                Assert.AreEqual(50, customers.Count());
                Assert.AreEqual(new SimpleCustomer(), customers.First());
                Assert.AreEqual(new SimpleCustomer(), customers.Last());
            }
        }

        [Test]
        public void PopulateComplexCustomerObject_WhenIssuingSelectWithJoin_ShouldReturnCollectionOfCustomersAndTheirCalls()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                //---------------Arrange-------------------
                connection.Open();

                //---------------Act----------------------
                var sql = "select Id, Name,Email, Login, TimeZoneId, ExternalSystemId from dbo.Customers";
                var customers = connection.Query<ComplexCustomer, Call, ComplexCustomer>(sql,
                    (customer, call) =>
                    {
                        //if (call.ExternalAccountId)
                        return null;
                    });

                //---------------Assert-----------------------
                Assert.AreEqual(50, customers.Count());

                // todo : check first for calls and last for calls
                Assert.AreEqual(new SimpleCustomer(), customers.First());
                Assert.AreEqual(new SimpleCustomer(), customers.Last());
            }
        }
    }
}
