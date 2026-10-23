using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BK.EventOps.Common.Models
{
    public class EventModel
    {
        [JsonPropertyName("eventId")]
        public string EventId { get; set; } = string.Empty;

        [JsonPropertyName("eventName")]
        public string EventName { get; set; } = string.Empty;

        [JsonPropertyName("eventType")]
        public string EventType { get; set; } = string.Empty;

        [JsonPropertyName("eventDate")]
        public string EventDate { get; set; } = string.Empty;

        [JsonPropertyName("endDate")]
        public string? EndDate { get; set; }

        [JsonPropertyName("venue")]
        public string Venue { get; set; } = string.Empty;

        [JsonPropertyName("customerName")]
        public string CustomerName { get; set; } = string.Empty;

        [JsonPropertyName("customerPhone")]
        public string CustomerPhone { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public string Status { get; set; } = "enquiry"; // "enquiry" | "confirmed" | "completed" | "cancelled"

        [JsonPropertyName("quotedAmount")]
        public decimal QuotedAmount { get; set; }

        [JsonPropertyName("totalAmountCollected")]
        public decimal TotalAmountCollected { get; set; }

        [JsonPropertyName("paymentStatus")]
        public string PaymentStatus { get; set; } = "unpaid"; // "unpaid" | "pending" | "paid"

        [JsonPropertyName("assignedEmployeeIds")]
        public List<string> AssignedEmployeeIds { get; set; } = new();

        [JsonPropertyName("createdBy")]
        public string CreatedBy { get; set; } = string.Empty;

        [JsonPropertyName("createdAt")]
        public string CreatedAt { get; set; } = DateTime.UtcNow.ToString("o");

        [JsonPropertyName("updatedAt")]
        public string UpdatedAt { get; set; } = DateTime.UtcNow.ToString("o");
    }

    public class UpdateEventStatusRequest
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
    }
}
