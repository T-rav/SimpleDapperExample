using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApplication.Models
{
    public class SimpleCustomer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public int TimeZoneId { get; set; }
        public int ExternalSystemId { get; set; }

    }
}
