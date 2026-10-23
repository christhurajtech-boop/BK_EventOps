using System;
using System.Text.Json.Serialization;

namespace BK.EventOps.Common.Models
{
    public class EventTransactionModel
    {
        [JsonPropertyName("transactionId")]
        public string TransactionId { get; set; } = string.Empty;

        [JsonPropertyName("eventId")]
        public string EventId { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public string Type { get; set; } = "expense"; // "income" | "expense"

        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("date")]
        public string Date { get; set; } = string.Empty;

        [JsonPropertyName("note")]
        public string? Note { get; set; }

        [JsonPropertyName("receiptUrl")]
        public string? ReceiptUrl { get; set; }

        [JsonPropertyName("recordedBy")]
        public string RecordedBy { get; set; } = string.Empty;
    }
}
