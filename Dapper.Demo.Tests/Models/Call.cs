using System;

namespace Dapper.Demo.Tests.Models
{
    public class Call
    {
        public int Id { get; set; }
        public int ExternalSystemId { get; set; }
        public int ExternalCallId { get; set; }
        public string ConferenceId { get; set; }
        public string Cld { get; set; }
        public string Cli { get; set; }

        public string Network { get; set; }
        public string RecordType { get; set; }
        public int BilledSeconds { get; set; }

        public DateTime UtcBillTime { get; set; }

    }
}
