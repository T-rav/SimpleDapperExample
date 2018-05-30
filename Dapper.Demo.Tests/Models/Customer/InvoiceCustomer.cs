namespace Dapper.Demo.Tests.Models.Customer
{
    public class InvoiceCustomer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public int TimeZoneId { get; set; }
        public int ExternalSystemId { get; set; }

        public InvoiceAddress Address { get; set; }
    }
}
