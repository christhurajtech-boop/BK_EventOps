using System;
using System.Text.Json.Serialization;

namespace BK.EventOps.Common.Models
{
    public class AttendanceModel
    {
        [JsonPropertyName("attendanceId")]
        public string AttendanceId { get; set; } = string.Empty;

        [JsonPropertyName("employeeId")]
        public string EmployeeId { get; set; } = string.Empty;

        [JsonPropertyName("eventId")]
        public string EventId { get; set; } = "general";

        [JsonPropertyName("date")]
        public string Date { get; set; } = string.Empty;

        [JsonPropertyName("workLocation")]
        public string WorkLocation { get; set; } = "onsite"; // "onsite" | "offsite"

        [JsonPropertyName("rateApplied")]
        public decimal RateApplied { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = "present"; // "present" | "absent" | "half-day"

        [JsonPropertyName("paymentStatus")]
        public string PaymentStatus { get; set; } = "unpaid"; // "unpaid" | "paid"

        [JsonPropertyName("markedBy")]
        public string MarkedBy { get; set; } = string.Empty;

        [JsonPropertyName("createdAt")]
        public string CreatedAt { get; set; } = DateTime.UtcNow.ToString("o");
    }
}
