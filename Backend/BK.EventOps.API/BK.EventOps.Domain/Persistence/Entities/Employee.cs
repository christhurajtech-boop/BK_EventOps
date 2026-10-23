using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BK.EventOps.Domain.Persistence.Entities
{
    [Table("employees")]
    public class Employee
    {
        [Key]
        [Column("employee_id")]
        [StringLength(50)]
        public string EmployeeId { get; set; } = string.Empty;

        [Column("name")]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [Column("phone")]
        [StringLength(30)]
        public string Phone { get; set; } = string.Empty;

        [Column("address")]
        public string? Address { get; set; }

        [Column("role")]
        [StringLength(80)]
        public string Role { get; set; } = string.Empty;

        [Column("date_joined")]
        [StringLength(30)]
        public string DateJoined { get; set; } = string.Empty;

        [Column("rate_on_site")]
        public decimal RateOnSite { get; set; }

        [Column("rate_off_site")]
        public decimal RateOffSite { get; set; }

        [Column("status")]
        [StringLength(20)]
        public string Status { get; set; } = "active";

        [Column("photo_url")]
        public string? PhotoUrl { get; set; }

        [Column("notes")]
        public string? Notes { get; set; }

        [Column("salary_type")]
        [StringLength(20)]
        public string SalaryType { get; set; } = "daily";

        [Column("monthly_salary")]
        public decimal MonthlySalary { get; set; } = 0;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
