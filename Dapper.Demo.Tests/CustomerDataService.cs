using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper.Demo.Tests.Models.Customer;

namespace Dapper.Demo.Tests
{
    public class CustomerDataService
    {
        private string _connectionString = "Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;Initial Catalog=DapperDemo";

        public IEnumerable<Customer> GetCustomers()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT Id,Name,Email,Login,TimeZoneId,ExternalSystemId FROM dbo.Customers";
                var customers = connection.Query<Customer>(sql);
                return customers;
            }
        }
    }
}