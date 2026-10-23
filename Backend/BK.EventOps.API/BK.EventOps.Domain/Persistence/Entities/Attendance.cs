using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BK.EventOps.Domain.Persistence.Entities
{
    [Table("attendances")]
    public class Attendance
    {
        [Key]
        [Column("attendance_id")]
        [StringLength(100)]
        public string AttendanceId { get; set; } = string.Empty;

        [Column("employee_id")]
        [StringLength(50)]
        public string EmployeeId { get; set; } = string.Empty;

        [Column("event_id")]
        [StringLength(50)]
        public string EventId { get; set; } = "general";

        [Column("date")]
        [StringLength(30)]
        public string Date { get; set; } = string.Empty;

        [Column("work_location")]
        [StringLength(20)]
        public string WorkLocation { get; set; } = "onsite";

        [Column("rate_applied")]
        public decimal RateApplied { get; set; }

        [Column("status")]
        [StringLength(20)]
        public string Status { get; set; } = "present";

        [Column("payment_status")]
        [StringLength(20)]
        public string PaymentStatus { get; set; } = "unpaid";

        [Column("marked_by")]
        [StringLength(100)]
        public string MarkedBy { get; set; } = string.Empty;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee? Employee { get; set; }
    }
}
