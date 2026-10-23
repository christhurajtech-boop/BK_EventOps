using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BK.EventOps.Domain.Persistence.Entities
{
    [Table("salary_payments")]
    public class SalaryPayment
    {
        [Key]
        [Column("payment_id")]
        [StringLength(50)]
        public string PaymentId { get; set; } = string.Empty;

        [Column("employee_id")]
        [StringLength(50)]
        public string EmployeeId { get; set; } = string.Empty;

        [Column("attendance_ids", TypeName = "jsonb")]
        public string AttendanceIdsJson { get; set; } = "[]";

        [Column("amount_paid")]
        public decimal AmountPaid { get; set; }

        [Column("month")]
        [StringLength(20)]
        public string Month { get; set; } = string.Empty;

        [Column("paid_on")]
        public DateTime PaidOn { get; set; } = DateTime.UtcNow;

        [Column("mode")]
        [StringLength(30)]
        public string Mode { get; set; } = "UPI";

        [Column("paid_by")]
        [StringLength(100)]
        public string PaidBy { get; set; } = string.Empty;

        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee? Employee { get; set; }
    }
}
