using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MyBot.Dtos
{
    public class UserDto
    {
        [JsonProperty("userId")]
        public int Id { get; set; }

        public DateTime? Birthday { get; set; }

        public int BranchId { get; set; }

        public string BranchName { get; set; }

        public DateTime? ContractSigned { get; set; }

        /// <summary>
        /// There will not be notification about missed daily report for this person.
        /// </summary>
        public bool DailyReportNotRequired { get; set; }

        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        [JsonProperty("fristName")]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Photo { get; set; }

        public string PrimaryEmail { get; set; }

        /// <summary>
        /// User status.
        /// </summary>
        public string Status { get; set; }

        public int TimeZone { get; set; }

        public string TimeZoneCode { get; set; }

        /// <summary>
        /// User type (client, employee, etc).
        /// </summary>
        public string TypeId { get; set; }

        public bool Utilization { get; set; }

        public int WorkTypeId { get; set; }

        public string Name => FirstName + " " + LastName;
    }
}
