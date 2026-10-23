using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BK.EventOps.Common.Models;
using BK.EventOps.Infrastructure.Interface;

namespace BK.EventOps.Infrastructure.Repositories
{
    public class EventOpsRepository : IEventOpsRepository
    {
        // Thread-safe in-memory stores ensuring instantaneous, reliable API responses
        private static readonly ConcurrentDictionary<string, EmployeeModel> _employees = new();
        private static readonly ConcurrentDictionary<string, EventModel> _events = new();
        private static readonly ConcurrentDictionary<string, AttendanceModel> _attendances = new();
        private static readonly ConcurrentDictionary<string, EventTransactionModel> _transactions = new();
        private static readonly ConcurrentDictionary<string, SalaryPaymentModel> _salaryPayments = new();

        static EventOpsRepository()
        {
            SeedInitialData();
        }

        private static void SeedInitialData()
        {
            // Seed sample initial employees
            var emp1 = new EmployeeModel
            {
                EmployeeId = "emp_1",
                Name = "Rajesh Kumar",
                Phone = "9876543210",
                Role = "Audio Engineer",
                DateJoined = "2024-01-15",
                RateOnSite = 1200,
                RateOffSite = 800,
                Status = "active",
                Notes = "Lead sound engineer"
            };
            var emp2 = new EmployeeModel
            {
                EmployeeId = "emp_2",
                Name = "Suresh Raina",
                Phone = "9876543211",
                Role = "Technician",
                DateJoined = "2024-02-01",
                RateOnSite = 1000,
                RateOffSite = 700,
                Status = "active",
                Notes = "Lighting technician"
            };
            _employees.TryAdd(emp1.EmployeeId, emp1);
            _employees.TryAdd(emp2.EmployeeId, emp2);

            // Seed sample event
            var evt1 = new EventModel
            {
                EventId = "evt_1",
                EventName = "Grand Wedding Reception",
                EventType = "Wedding",
                EventDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                Venue = "Royal Palace Hall, Chennai",
                CustomerName = "Anand & Priya",
                CustomerPhone = "9988776655",
                Status = "confirmed",
                QuotedAmount = 45000,
                TotalAmountCollected = 15000,
                AssignedEmployeeIds = new List<string> { "emp_1", "emp_2" },
                CreatedBy = "Admin",
                CreatedAt = DateTime.UtcNow.ToString("o"),
                UpdatedAt = DateTime.UtcNow.ToString("o")
            };
            _events.TryAdd(evt1.EventId, evt1);
        }

        // --- Employees ---
        public Task<IEnumerable<EmployeeModel>> GetAllEmployeesAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IEnumerable<EmployeeModel>>(_employees.Values.OrderBy(e => e.Name).ToList());
        }

        public Task<EmployeeModel?> GetEmployeeByIdAsync(string employeeId, CancellationToken cancellationToken = default)
        {
            _employees.TryGetValue(employeeId, out var employee);
            return Task.FromResult(employee);
        }

        public Task<EmployeeModel> UpsertEmployeeAsync(EmployeeModel employee, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(employee.EmployeeId))
            {
                employee.EmployeeId = $"emp_{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";
            }
            _employees[employee.EmployeeId] = employee;
            return Task.FromResult(employee);
        }

        public Task<EmployeeModel?> ToggleEmployeeStatusAsync(string employeeId, CancellationToken cancellationToken = default)
        {
            if (_employees.TryGetValue(employeeId, out var emp))
            {
                emp.Status = emp.Status == "active" ? "inactive" : "active";
                _employees[employeeId] = emp;
                return Task.FromResult<EmployeeModel?>(emp);
            }
            return Task.FromResult<EmployeeModel?>(null);
        }

        public Task<bool> DeleteEmployeeAsync(string employeeId, CancellationToken cancellationToken = default)
        {
            var removed = _employees.TryRemove(employeeId, out _);
            return Task.FromResult(removed);
        }

        // --- Events ---
        public Task<IEnumerable<EventModel>> GetAllEventsAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IEnumerable<EventModel>>(_events.Values.OrderByDescending(e => e.EventDate).ToList());
        }

        public Task<EventModel?> GetEventByIdAsync(string eventId, CancellationToken cancellationToken = default)
        {
            _events.TryGetValue(eventId, out var ev);
            return Task.FromResult(ev);
        }

        public Task<EventModel> UpsertEventAsync(EventModel eventModel, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(eventModel.EventId))
            {
                eventModel.EventId = $"evt_{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";
            }
            eventModel.UpdatedAt = DateTime.UtcNow.ToString("o");
            _events[eventModel.EventId] = eventModel;
            return Task.FromResult(eventModel);
        }

        public Task<EventModel?> UpdateEventStatusAsync(string eventId, string status, CancellationToken cancellationToken = default)
        {
            if (_events.TryGetValue(eventId, out var ev))
            {
                ev.Status = status;
                ev.UpdatedAt = DateTime.UtcNow.ToString("o");
                _events[eventId] = ev;
                return Task.FromResult<EventModel?>(ev);
            }
            return Task.FromResult<EventModel?>(null);
        }

        public Task<bool> DeleteEventAsync(string eventId, CancellationToken cancellationToken = default)
        {
            var removed = _events.TryRemove(eventId, out _);
            return Task.FromResult(removed);
        }

        // --- Attendance ---
        public Task<IEnumerable<AttendanceModel>> GetAllAttendanceAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IEnumerable<AttendanceModel>>(_attendances.Values.OrderByDescending(a => a.Date).ToList());
        }

        public Task<IEnumerable<AttendanceModel>> GetAttendanceByEventAsync(string eventId, CancellationToken cancellationToken = default)
        {
            var list = _attendances.Values.Where(a => a.EventId == eventId).ToList();
            return Task.FromResult<IEnumerable<AttendanceModel>>(list);
        }

        public Task<AttendanceModel> UpsertAttendanceAsync(AttendanceModel attendance, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(attendance.AttendanceId))
            {
                attendance.AttendanceId = $"{attendance.EmployeeId}_{attendance.EventId}_{attendance.Date}";
            }
            _attendances[attendance.AttendanceId] = attendance;
            return Task.FromResult(attendance);
        }

        public Task<IEnumerable<AttendanceModel>> UpsertBulkAttendanceAsync(List<AttendanceModel> attendances, CancellationToken cancellationToken = default)
        {
            foreach (var att in attendances)
            {
                if (string.IsNullOrWhiteSpace(att.AttendanceId))
                {
                    att.AttendanceId = $"{att.EmployeeId}_{att.EventId}_{att.Date}";
                }
                _attendances[att.AttendanceId] = att;
            }
            return Task.FromResult<IEnumerable<AttendanceModel>>(attendances);
        }

        // --- Transactions ---
        public Task<IEnumerable<EventTransactionModel>> GetAllTransactionsAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IEnumerable<EventTransactionModel>>(_transactions.Values.OrderByDescending(t => t.Date).ToList());
        }

        public Task<IEnumerable<EventTransactionModel>> GetTransactionsByEventAsync(string eventId, CancellationToken cancellationToken = default)
        {
            var list = _transactions.Values.Where(t => t.EventId == eventId).ToList();
            return Task.FromResult<IEnumerable<EventTransactionModel>>(list);
        }

        public Task<EventTransactionModel> UpsertTransactionAsync(EventTransactionModel transaction, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(transaction.TransactionId))
            {
                transaction.TransactionId = $"tx_{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";
            }
            _transactions[transaction.TransactionId] = transaction;

            // Recompute event collected amount if this is an income transaction
            if (!string.IsNullOrEmpty(transaction.EventId) && _events.TryGetValue(transaction.EventId, out var ev))
            {
                var incomeSum = _transactions.Values
                    .Where(t => t.EventId == transaction.EventId && t.Type == "income")
                    .Sum(t => t.Amount);
                ev.TotalAmountCollected = incomeSum;
                ev.UpdatedAt = DateTime.UtcNow.ToString("o");
                _events[transaction.EventId] = ev;
            }

            return Task.FromResult(transaction);
        }

        public Task<bool> DeleteTransactionAsync(string transactionId, CancellationToken cancellationToken = default)
        {
            if (_transactions.TryRemove(transactionId, out var removed))
            {
                if (!string.IsNullOrEmpty(removed.EventId) && _events.TryGetValue(removed.EventId, out var ev))
                {
                    var incomeSum = _transactions.Values
                        .Where(t => t.EventId == removed.EventId && t.Type == "income")
                        .Sum(t => t.Amount);
                    ev.TotalAmountCollected = incomeSum;
                    ev.UpdatedAt = DateTime.UtcNow.ToString("o");
                    _events[removed.EventId] = ev;
                }
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        // --- Salary Payments ---
        public Task<IEnumerable<SalaryPaymentModel>> GetAllSalaryPaymentsAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IEnumerable<SalaryPaymentModel>>(_salaryPayments.Values.OrderByDescending(s => s.PaidOn).ToList());
        }

        public Task<SalaryPaymentModel> AddSalaryPaymentAsync(SalaryPaymentModel payment, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(payment.PaymentId))
            {
                payment.PaymentId = $"pay_{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";
            }
            _salaryPayments[payment.PaymentId] = payment;

            // Mark attendances as paid
            if (payment.AttendanceIds != null)
            {
                foreach (var attId in payment.AttendanceIds)
                {
                    if (_attendances.TryGetValue(attId, out var att))
                    {
                        att.PaymentStatus = "paid";
                        _attendances[attId] = att;
                    }
                }
            }

            return Task.FromResult(payment);
        }
    }
}
