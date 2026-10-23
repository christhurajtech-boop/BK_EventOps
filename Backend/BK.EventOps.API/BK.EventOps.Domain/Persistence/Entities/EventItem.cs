using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BK.EventOps.Domain.Persistence.Entities
{
    [Table("events")]
    public class EventItem
    {
        [Key]
        [Column("event_id")]
        [StringLength(50)]
        public string EventId { get; set; } = string.Empty;

        [Column("event_name")]
        [StringLength(200)]
        public string EventName { get; set; } = string.Empty;

        [Column("event_type")]
        [StringLength(80)]
        public string EventType { get; set; } = string.Empty;

        [Column("event_date")]
        [StringLength(30)]
        public string EventDate { get; set; } = string.Empty;

        [Column("end_date")]
        [StringLength(30)]
        public string? EndDate { get; set; }

        [Column("venue")]
        [StringLength(250)]
        public string Venue { get; set; } = string.Empty;

        [Column("customer_name")]
        [StringLength(150)]
        public string CustomerName { get; set; } = string.Empty;

        [Column("customer_phone")]
        [StringLength(30)]
        public string CustomerPhone { get; set; } = string.Empty;

        [Column("status")]
        [StringLength(20)]
        public string Status { get; set; } = "enquiry";

        [Column("quoted_amount")]
        public decimal QuotedAmount { get; set; }

        [Column("total_amount_collected")]
        public decimal TotalAmountCollected { get; set; }

        [Column("payment_status")]
        [StringLength(30)]
        public string PaymentStatus { get; set; } = "unpaid";

        [Column("assigned_employee_ids", TypeName = "jsonb")]
        public string AssignedEmployeeIdsJson { get; set; } = "[]";

        [Column("created_by")]
        [StringLength(100)]
        public string CreatedBy { get; set; } = "Admin";

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
