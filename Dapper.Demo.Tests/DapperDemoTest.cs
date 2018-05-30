using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Dapper.Demo.Tests.Models;
using Dapper.Demo.Tests.Models.Customer;
using FluentAssertions;
using NUnit.Framework;

namespace Dapper.Demo.Tests
{
    [TestFixture]
    public class DapperDemoTest
    {
        private string _connectionString = "Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;Initial Catalog=DapperDemo";

        [Test]
        public void PopulateCustomer_WhenIssuingSelect_ShouldReturnCollectionOfCustomers()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                //---------------Arrange-------------------
                connection.Open();

                //---------------Act----------------------
                var sql = "SELECT Id,Name,Email,Login,TimeZoneId,ExternalSystemId FROM dbo.Customers";
                var customers = connection.Query<Customer>(sql);
                var actualFirstAndLast = new List<Customer>
                {
                    customers.First(),
                    customers.Last()
                };
                //---------------Assert-----------------------
                var expectedFirstAndLast = new List<Customer>
                {
                    new Customer
                    {
                        Id = 1,
                        Name = "ReshetCall Ltd",
                        Email = "gal@reshetcall.co.il,v@bizvoip.co.za",
                        Login = "gal@reshetcall.co.il",
                        ExternalSystemId = 53580,
                        TimeZoneId = 370
                    },
                    new Customer
                    {
                        Id = 50,
                        Name = "Air Hub Customer1",
                        Email = "litc@live.co.za",
                        Login = null,
                        ExternalSystemId = 55596,
                        TimeZoneId = 113
                    }
                };
                Assert.AreEqual(50, customers.Count());
                actualFirstAndLast.Should().BeEquivalentTo(expectedFirstAndLast);
            }
        }

        [Test]
        [Ignore("wip")]
        public void PopulateBillingCustomer_WhenPerformingOneToOneMapping_ShouldReturnCustomersAndTheirPhysicalAddress()
        {
            // todo : join data
            using (var connection = new SqlConnection(_connectionString))
            {
                //---------------Arrange-------------------
                connection.Open();

                //---------------Act----------------------
                var sql = "SELECT c.Id, Name,Email, Login, TimeZoneId, ExternalSystemId," +
                          " AddressLine1, AddressLine2, City, Province, PostCode" +
                          " FROM dbo.Customers c JOIN dbo.PhysicalAddresses pa" +
                          " ON c.Id = pa.CustomerId";
                var customers = connection.Query<ReportingCustomer, CustomerPhysicalAddress, ReportingCustomer>(sql,
                    (customer, address) =>
                    {
                        customer.Address = address;
                        return customer;
                    }, splitOn: "Id,CustomerId");
                var actualFirstAndLast = new List<ReportingCustomer>
                {
                    customers.First(),
                    customers.Last()
                };
                //---------------Assert-----------------------
                var expectedFirstAndLast = new List<ReportingCustomer>
                {
                    new ReportingCustomer
                    {
                        Id = 1,
                        Name = "ReshetCall Ltd",
                        Email = "gal@reshetcall.co.il,v@bizvoip.co.za",
                        Login = "gal@reshetcall.co.il",
                        ExternalSystemId = 53580,
                        TimeZoneId = 370,
                        Address = new CustomerPhysicalAddress
                        {
                            AddressLine1 = "8218 Valentina Divide",
                            AddressLine2 = "171 Leif Landing",
                            City = "New Missouriport",
                            Province = "Mpumalanga",
                            PostCode = "8484"
                        }
                    },
                    new ReportingCustomer
                    {
                        Id = 50,
                        Name = "Air Hub Customer1",
                        Email = "litc@live.co.za",
                        Login = null,
                        ExternalSystemId = 55596,
                        TimeZoneId = 113,
                        Address = new CustomerPhysicalAddress
                        {
                            AddressLine1 = "6789 Verla Field",
                            AddressLine2 = "83553 Peggie Fords",
                            City = "New Noraport",
                            Province = "Limpopo",
                            PostCode = "9543"
                        }
                    }
                };
                Assert.AreEqual(50, customers.Count());
                actualFirstAndLast.Should().BeEquivalentTo(expectedFirstAndLast);
            }
        }

        [Test]
        [Ignore("wip")]
        public void PopulateReportingCustomer_WhenPerformingOneToManyMapping_ShouldReturnCustomersAndTheirCalls()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                //---------------Arrange-------------------
                connection.Open();

                //---------------Act----------------------
                var sql = "select Id, Name,Email, Login, TimeZoneId, ExternalSystemId from dbo.Customers";
                var customers = connection.Query<BillingCustomer, Call, ReportingCustomer>(sql,
                    (customer, call) =>
                    {
                        //if (call.ExternalAccountId)
                        return null;
                    });

                //---------------Assert-----------------------
                Assert.AreEqual(50, customers.Count());

                // todo : check first for calls and last for calls
                Assert.AreEqual(new Customer(), customers.First());
                Assert.AreEqual(new Customer(), customers.Last());
            }
        }


        [Test]
        [Ignore("Finshed and working")]
        public void LearningTests_Goal_WritePhysicalAddressData()
        {
            //---------------Arrange-------------------
            var customerIds = Enumerable.Range(1, 50);
            var postCode = Enumerable.Range(1000, 9999).Select(x => x.ToString()).ToArray();
            var provinces = new[]{"Northern Cape",
                                  "Eastern Cape",
                                  "Free State",
                                  "Western Cape",
                                  "Limpopo",
                                  "North West",
                                  "KwaZulu-Natal",
                                  "Mpumalanga",
                                  "Gauteng" };
            var writeToFile = "D:\\temp-systems\\SimpleDapperExample\\sql\\Insert dbo.PhysicalAddresses.sql";
            File.WriteAllText(writeToFile, string.Empty);
            //---------------Act----------------------
            foreach (var customerId in customerIds)
            {
                var physicalAddress = new Faker<CustomerPhysicalAddress>()
                    .RuleFor(x => x.CustomerId, id => customerId)
                    .RuleFor(x => x.PostCode, p => p.PickRandom(postCode))
                    .RuleFor(x => x.Province, p => p.PickRandom(provinces))
                    .RuleFor(x => x.City, (f, u) => f.Address.City())
                    .RuleFor(x => x.AddressLine2, (f, u) => f.Address.StreetAddress())
                    .RuleFor(x => x.AddressLine1, (f, u) => f.Address.StreetAddress())
                    .Generate(1).First();

                var sql =
                    "insert into PhysicalAddresses(CustomerId, AddressLine1, AddressLine2, City, Province, PostCode, Created, Modified) " +
                    $"values({physicalAddress.CustomerId},'{physicalAddress.AddressLine1}','{physicalAddress.AddressLine2}','{physicalAddress.City}','{physicalAddress.Province}','{physicalAddress.PostCode}','2018-05-05 09:10:11','2018-05-05 12:11:10');";
                File.AppendAllText(writeToFile, sql);
                File.AppendAllText(writeToFile, Environment.NewLine);
            }

            var actual = File.Exists(writeToFile);
            //---------------Assert-----------------------
            //Assert.Fail("Test Not Yet Implemented
            actual.Should().BeTrue();
        }
    }
}
