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
    public class CustomerDataServiceTests
    {
        [Test]
        public void GetCustomers_WhenDataPresent_ShouldReturnCollectionOfCustomers()
        {
            //---------------Arrange-------------------
            var customerDataService = new CustomerDataService("DapperDemo");
            //---------------Act----------------------
            var actual = customerDataService.GetCustomers();

            var actualFirstAndLast = new List<Customer>
            {
                actual.First(),
                actual.Last()
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
            Assert.AreEqual(50, actual.Count());
            actualFirstAndLast.Should().BeEquivalentTo(expectedFirstAndLast);
        }

        [Test]
        public void GetInvoiceCustomers_WhenPerformingOneToOneMapping_ShouldReturnCustomersAndTheirPhysicalAddress()
        {
            //---------------Arrange-------------------
            var dataService = new CustomerDataService("DapperDemo");
            //---------------Act----------------------
            var actual = dataService.GetInvoiceCustomers();
            var actualFirstAndLast = new List<InvoiceCustomer>
            {
                actual.First(),
                actual.Last()
            };
            //---------------Assert-----------------------
            var expectedFirstAndLast = new List<InvoiceCustomer>
            {
                new InvoiceCustomer
                {
                    Id = 1,
                    Name = "ReshetCall Ltd",
                    Email = "gal@reshetcall.co.il,v@bizvoip.co.za",
                    Login = "gal@reshetcall.co.il",
                    ExternalSystemId = 53580,
                    TimeZoneId = 370,
                    Address = new InvoiceAddress
                    {
                        AddressLine1 = "8218 Valentina Divide",
                        AddressLine2 = "171 Leif Landing",
                        City = "New Missouriport",
                        Province = "Mpumalanga",
                        PostCode = "8484"
                    }
                },
                new InvoiceCustomer
                {
                    Id = 50,
                    Name = "Air Hub Customer1",
                    Email = "litc@live.co.za",
                    Login = null,
                    ExternalSystemId = 55596,
                    TimeZoneId = 113,
                    Address = new InvoiceAddress
                    {
                        AddressLine1 = "6789 Verla Field",
                        AddressLine2 = "83553 Peggie Fords",
                        City = "New Noraport",
                        Province = "Limpopo",
                        PostCode = "9543"
                    }
                }
            };
            Assert.AreEqual(50, actual.Count());
            actualFirstAndLast.Should().BeEquivalentTo(expectedFirstAndLast);
        }

        [Test]
        public void PopulateReportingCustomer_WhenPerformingOneToManyMapping_ShouldReturnCustomersAndTheirCalls()
        {
            //---------------Arrange-------------------
            var dataService = new CustomerDataService("DapperDemo");
            //---------------Act----------------------
            var actual = dataService.GetBillingCustomers();
            var actualFirstAndLast = new List<BillingCustomer>
            {
                actual.First(),
                actual.Last()
            };
            //---------------Assert-----------------------
            var expectedFirstAndLast = new List<BillingCustomer>
            {
                new BillingCustomer
                {
                    Id = 1,
                    Name = "ReshetCall Ltd",
                    Email = "gal@reshetcall.co.il,v@bizvoip.co.za",
                    Login = "gal@reshetcall.co.il",
                    ExternalSystemId = 53580,
                    TimeZoneId = 370,
                    Calls = new List<Call>
                    {
                        new Call
                        {
                            Id = 48,
                            ExternalSystemId = 53580,
                            ExternalCallId = 227027,
                            ConferenceId = "kbtttgebloudabvclnsaabvtffnqsphsht",
                            Cld = "993-366-4987",
                            Cli = "1-871-763-7975 x3820",
                            Network = "gczmrsjpv",
                            RecordType = "Voice Call",
                            BilledSeconds = 4436,
                            UtcBillTime = DateTime.Parse("2018-05-31 09:21:38.000")
                        },
                        new Call
                        {
                            Id = 50,
                            ExternalSystemId = 53580,
                            ExternalCallId = 736812,
                            ConferenceId = "gjawvdbgzwuvioleywblplceauzsdmspua",
                            Cld = "370.642.9181 x69626",
                            Cli = "(373) 921-1047",
                            Network = "whvvozrfjbkvvy",
                            RecordType = "Voice Call",
                            BilledSeconds = 1322,
                            UtcBillTime = DateTime.Parse("2018-05-31 08:34:23.000")
                        },
                        new Call
                        {
                            Id = 72,
                            ExternalSystemId = 53580,
                            ExternalCallId = 822679,
                            ConferenceId = "nzkddomvokvcpfwgpicyuttdhtlvmudpgx",
                            Cld = "1-829-817-3523 x02608",
                            Cli = "668-603-0138",
                            Network = "xewrffhvvld",
                            RecordType = "Voice Call",
                            BilledSeconds = 4337,
                            UtcBillTime = DateTime.Parse("2018-06-02 19:27:57.000")
                        }
                        // 4
                    }
                },
                new BillingCustomer()
                {
                    Id = 50,
                    Name = "Air Hub Customer1",
                    Email = "litc@live.co.za",
                    Login = null,
                    ExternalSystemId = 55596,
                    TimeZoneId = 113,
                    Calls = new List<Call>
                    {

                    }
                }
            };
            Assert.AreEqual(450, actual.Count());
            actualFirstAndLast.Should().BeEquivalentTo(expectedFirstAndLast);
        }

        [Test]
        //[Ignore("Working")]
        public void LearningTest_Goal_CreateCalls()
        {
            //---------------Arrange-------------------
            var customerIds = Enumerable.Range(1, 50);
            var externalSystemIds = GetExternalSystemIds();
            //---------------Act----------------------
            var writeToFile = "D:\\temp-systems\\SimpleDapperExample\\sql\\Insert dbo.Calls.sql";
            File.WriteAllText(writeToFile, string.Empty);
            //---------------Act----------------------
            foreach (var customerId in customerIds)
            {
                var callsToCreate = new Random(100).Next(1, 10);
                var calls = new Faker<Call>()
                    .RuleFor(x => x.ExternalSystemId, id => id.PickRandom(externalSystemIds))
                    .RuleFor(x => x.Cld, p => p.Phone.PhoneNumber())
                    .RuleFor(x => x.Cli, p => p.Phone.PhoneNumber())
                    .RuleFor(x => x.BilledSeconds, p => p.Random.Number(1, 5000))
                    .RuleFor(x => x.ConferenceId, p => p.Random.String2(34))
                    .RuleFor(x => x.ExternalCallId, p => p.Random.Int(10000, 999999))
                    .RuleFor(x => x.Network, p => p.Random.String2(5, 15))
                    .RuleFor(x => x.UtcBillTime, p => p.Date.Soon(5))
                    .RuleFor(x => x.RecordType, p => "Voice Call")
                    .Generate(callsToCreate);

                foreach (var call in calls)
                {
                    var sql =
                        "insert into Calls(ExternalSystemId, ExternalCallId, ConferenceId, Cld, Cli, ServiceType,Network, RecordType, BilledSeconds, UtcBillTime, Created, Modified) " +
                        $"values({call.ExternalSystemId},'{call.ExternalCallId}','{call.ConferenceId}','{call.Cld}','{call.Cli}','client call','{call.Network}','{call.RecordType}','{call.BilledSeconds}','{call.UtcBillTime:yyyy-MM-dd HH:mm:ss}','2018-05-05 09:10:11','2018-05-05 12:11:10');";
                    File.AppendAllText(writeToFile, sql);
                    File.AppendAllText(writeToFile, Environment.NewLine);
                }

                //---------------Assert-----------------------
                var actual = File.Exists(writeToFile);
                //---------------Assert-----------------------
                actual.Should().BeTrue();
            }
        }

        private static int[] GetExternalSystemIds()
        {
            return new[] { 31660,
            31705,
            33616,
            35095,
            37332,
            45246,
            46171,
            46248,
            46650,
            51476,
            51850,
            52203,
            52456,
            53231,
            53340,
            53342,
            53540,
            53580,
            53603,
            54309,
            54336,
            54654,
            54672,
            55113,
            55596,
            55789,
            55912,
            55939,
            55946,
            55963,
            55979,
            56116,
            57324,
            57341,
            58775,
            59580,
            60753,
            60755,
            60894,
            61621,
            61817,
            61825,
            61834,
            61841,
            61846,
            62072,
            67766,
            69213,
            70715,
            70974};
        }


        //[Test]
        //[Ignore("Finshed and working")]
        //public void LearningTests_Goal_WritePhysicalAddressData()
        //{
        //    //---------------Arrange-------------------
        //    var customerIds = Enumerable.Range(1, 50);
        //    var postCode = Enumerable.Range(1000, 9999).Select(x => x.ToString()).ToArray();
        //    var provinces = new[]{"Northern Cape",
        //                          "Eastern Cape",
        //                          "Free State",
        //                          "Western Cape",
        //                          "Limpopo",
        //                          "North West",
        //                          "KwaZulu-Natal",
        //                          "Mpumalanga",
        //                          "Gauteng" };
        //    var writeToFile = "D:\\temp-systems\\SimpleDapperExample\\sql\\Insert dbo.PhysicalAddresses.sql";
        //    File.WriteAllText(writeToFile, string.Empty);
        //    //---------------Act----------------------
        //    foreach (var customerId in customerIds)
        //    {
        //        var physicalAddress = new Faker<CustomerPhysicalAddress>()
        //            .RuleFor(x => x.CustomerId, id => customerId)
        //            .RuleFor(x => x.PostCode, p => p.PickRandom(postCode))
        //            .RuleFor(x => x.Province, p => p.PickRandom(provinces))
        //            .RuleFor(x => x.City, (f, u) => f.Address.City())
        //            .RuleFor(x => x.AddressLine2, (f, u) => f.Address.StreetAddress())
        //            .RuleFor(x => x.AddressLine1, (f, u) => f.Address.StreetAddress())
        //            .Generate(1).First();

        //        var sql =
        //            "insert into PhysicalAddresses(CustomerId, AddressLine1, AddressLine2, City, Province, PostCode, Created, Modified) " +
        //            $"values({physicalAddress.CustomerId},'{physicalAddress.AddressLine1}','{physicalAddress.AddressLine2}','{physicalAddress.City}','{physicalAddress.Province}','{physicalAddress.PostCode}','2018-05-05 09:10:11','2018-05-05 12:11:10');";
        //        File.AppendAllText(writeToFile, sql);
        //        File.AppendAllText(writeToFile, Environment.NewLine);
        //    }

        //    var actual = File.Exists(writeToFile);
        //    //---------------Assert-----------------------
        //    //Assert.Fail("Test Not Yet Implemented
        //    actual.Should().BeTrue();
        //}
    }
}
