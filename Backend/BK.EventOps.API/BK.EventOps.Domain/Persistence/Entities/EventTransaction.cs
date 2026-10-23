using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BK.EventOps.Domain.Persistence.Entities
{
    [Table("transactions")]
    public class EventTransaction
    {
        [Key]
        [Column("transaction_id")]
        [StringLength(50)]
        public string TransactionId { get; set; } = string.Empty;

        [Column("event_id")]
        [StringLength(50)]
        public string EventId { get; set; } = string.Empty;

        [Column("type")]
        [StringLength(20)]
        public string Type { get; set; } = "expense";

        [Column("category")]
        [StringLength(100)]
        public string Category { get; set; } = string.Empty;

        [Column("amount")]
        public decimal Amount { get; set; }

        [Column("date")]
        [StringLength(30)]
        public string Date { get; set; } = string.Empty;

        [Column("note")]
        public string? Note { get; set; }

        [Column("receipt_url")]
        public string? ReceiptUrl { get; set; }

        [Column("recorded_by")]
        [StringLength(100)]
        public string RecordedBy { get; set; } = string.Empty;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(EventId))]
        public virtual EventItem? Event { get; set; }
    }
}
