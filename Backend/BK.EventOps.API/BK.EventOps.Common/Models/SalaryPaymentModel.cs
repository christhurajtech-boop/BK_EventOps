using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BK.EventOps.Common.Models
{
    public class SalaryPaymentModel
    {
        [JsonPropertyName("paymentId")]
        public string PaymentId { get; set; } = string.Empty;

        [JsonPropertyName("employeeId")]
        public string EmployeeId { get; set; } = string.Empty;

        [JsonPropertyName("attendanceIds")]
        public List<string> AttendanceIds { get; set; } = new();

        [JsonPropertyName("amountPaid")]
        public decimal AmountPaid { get; set; }

        [JsonPropertyName("month")]
        public string Month { get; set; } = string.Empty; // YYYY-MM

        [JsonPropertyName("paidOn")]
        public string PaidOn { get; set; } = DateTime.UtcNow.ToString("o");

        [JsonPropertyName("mode")]
        public string Mode { get; set; } = "Cash"; // "Cash" | "UPI" | "Bank Transfer"

        [JsonPropertyName("paidBy")]
        public string PaidBy { get; set; } = string.Empty;

        [JsonPropertyName("salaryType")]
        public string SalaryType { get; set; } = "daily"; // "daily" | "monthly"
    }

    public class CreateSalaryPayoutRequest
    {
        [JsonPropertyName("employeeId")]
        public string EmployeeId { get; set; } = string.Empty;

        [JsonPropertyName("attendanceIds")]
        public List<string> AttendanceIds { get; set; } = new();

        [JsonPropertyName("amountPaid")]
        public decimal AmountPaid { get; set; }

        [JsonPropertyName("month")]
        public string? Month { get; set; }

        [JsonPropertyName("mode")]
        public string Mode { get; set; } = "UPI";

        [JsonPropertyName("paidBy")]
        public string PaidBy { get; set; } = string.Empty;

        [JsonPropertyName("salaryType")]
        public string SalaryType { get; set; } = "daily"; // "daily" | "monthly"
    }
}
