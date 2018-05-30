using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper.Demo.Tests.Models.Customer;

namespace Dapper.Demo.Tests
{
    public class CustomerDataService
    {
        private readonly string _connectionString;

        public CustomerDataService(string dbName)
        {
            _connectionString = $"Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;Initial Catalog={dbName}";
        }

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