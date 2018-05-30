using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper.Demo.Tests.Models;
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

        public IEnumerable<InvoiceCustomer> GetInvoiceCustomers()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT *" +
                          " FROM dbo.Customers c LEFT JOIN dbo.PhysicalAddresses pa" +
                          " ON c.Id = pa.CustomerId";
                var customers = connection.Query<InvoiceCustomer, InvoiceAddress, InvoiceCustomer>(sql,
                    (customer, address) =>
                    {
                        customer.Address = address;
                        return customer;
                    }, splitOn: "Id");

                return customers;
            }
        }

        public IEnumerable<BillingCustomer> GetBillingCustomers()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT *" +
                          " FROM dbo.Calls ca LEFT JOIN dbo.Customers c" +
                          " ON ca.ExternalSystemId = c.ExternalSystemId";
                var customers = connection.Query<BillingCustomer, Call, BillingCustomer>(sql,
                    (customer, call) =>
                    {
                        customer.Calls.Add(call);
                        return customer;
                    }, splitOn: "Id");

                return customers;
            }
        }
    }
}