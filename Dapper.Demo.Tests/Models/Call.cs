using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApplication.Models
{
    public class Call
    {
        public int Id { get; set; }
        public int ExternalAccountId { get; set; }
        public int ExternalCallId { get; set; }
        public int ConferenceId { get; set; }
        public string Cld { get; set; }
        public string Cli { get; set; }

        public string Network { get; set; }
        public string RecordType { get; set; }
        public int BilledSeconds { get; set; }

        public DateTime UtcBillTime { get; set; }

    }
}
