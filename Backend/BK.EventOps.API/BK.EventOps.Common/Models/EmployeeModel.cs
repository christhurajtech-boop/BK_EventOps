using System;
using System.Text.Json.Serialization;

namespace BK.EventOps.Common.Models
{
    public class EmployeeModel
    {
        [JsonPropertyName("employeeId")]
        public string EmployeeId { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("phone")]
        public string Phone { get; set; } = string.Empty;

        [JsonPropertyName("address")]
        public string? Address { get; set; }

        [JsonPropertyName("role")]
        public string Role { get; set; } = string.Empty;

        [JsonPropertyName("dateJoined")]
        public string DateJoined { get; set; } = string.Empty;

        [JsonPropertyName("rateOnSite")]
        public decimal RateOnSite { get; set; }

        [JsonPropertyName("rateOffSite")]
        public decimal RateOffSite { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = "active"; // "active" | "inactive"

        [JsonPropertyName("photoUrl")]
        public string? PhotoUrl { get; set; }

        [JsonPropertyName("notes")]
        public string? Notes { get; set; }

        [JsonPropertyName("salaryType")]
        public string SalaryType { get; set; } = "daily"; // "daily" | "monthly"

        [JsonPropertyName("monthlySalary")]
        public decimal MonthlySalary { get; set; } = 0;
    }
}
