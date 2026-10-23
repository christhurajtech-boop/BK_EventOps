using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BK.EventOps.Application.Interface;
using BK.EventOps.Common.Models;
using BK.EventOps.Infrastructure.Interface;

namespace BK.EventOps.Application.Services
{
    public class EventOpsService : IEventOpsService
    {
        private readonly IEventOpsRepository _repository;

        public EventOpsService(IEventOpsRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // --- Employees ---
        public Task<IEnumerable<EmployeeModel>> GetEmployeesAsync(CancellationToken cancellationToken = default)
        {
            return _repository.GetAllEmployeesAsync(cancellationToken);
        }

        public Task<EmployeeModel?> GetEmployeeByIdAsync(string employeeId, CancellationToken cancellationToken = default)
        {
            return _repository.GetEmployeeByIdAsync(employeeId, cancellationToken);
        }

        public Task<EmployeeModel> SaveEmployeeAsync(EmployeeModel employee, CancellationToken cancellationToken = default)
        {
            return _repository.UpsertEmployeeAsync(employee, cancellationToken);
        }

        public Task<EmployeeModel?> ToggleEmployeeStatusAsync(string employeeId, CancellationToken cancellationToken = default)
        {
            return _repository.ToggleEmployeeStatusAsync(employeeId, cancellationToken);
        }

        public Task<EmployeeModel?> UpdateEmployeeStatusAsync(string employeeId, string status, CancellationToken cancellationToken = default)
        {
            return _repository.UpdateEmployeeStatusAsync(employeeId, status, cancellationToken);
        }

        public Task<bool> HasAttendanceForEmployeeAsync(string employeeId, CancellationToken cancellationToken = default)
        {
            return _repository.HasAttendanceForEmployeeAsync(employeeId, cancellationToken);
        }

        public Task<bool> DeleteEmployeeAsync(string employeeId, CancellationToken cancellationToken = default)
        {
            return _repository.DeleteEmployeeAsync(employeeId, cancellationToken);
        }

        // --- Events ---
        public Task<IEnumerable<EventModel>> GetEventsAsync(CancellationToken cancellationToken = default)
        {
            return _repository.GetAllEventsAsync(cancellationToken);
        }

        public Task<EventModel?> GetEventByIdAsync(string eventId, CancellationToken cancellationToken = default)
        {
            return _repository.GetEventByIdAsync(eventId, cancellationToken);
        }

        public Task<EventModel> SaveEventAsync(EventModel eventModel, CancellationToken cancellationToken = default)
        {
            return _repository.UpsertEventAsync(eventModel, cancellationToken);
        }

        public Task<EventModel?> UpdateEventStatusAsync(string eventId, string status, CancellationToken cancellationToken = default)
        {
            return _repository.UpdateEventStatusAsync(eventId, status, cancellationToken);
        }

        public Task<bool> DeleteEventAsync(string eventId, CancellationToken cancellationToken = default)
        {
            return _repository.DeleteEventAsync(eventId, cancellationToken);
        }

        public Task<bool> HasTransactionsForEventAsync(string eventId, CancellationToken cancellationToken = default)
        {
            return _repository.HasTransactionsForEventAsync(eventId, cancellationToken);
        }

        // --- Attendance ---
        public Task<IEnumerable<AttendanceModel>> GetAttendanceAsync(CancellationToken cancellationToken = default)
        {
            return _repository.GetAllAttendanceAsync(cancellationToken);
        }

        public Task<IEnumerable<AttendanceModel>> GetAttendanceByEventAsync(string eventId, CancellationToken cancellationToken = default)
        {
            return _repository.GetAttendanceByEventAsync(eventId, cancellationToken);
        }

        public Task<AttendanceModel> SaveAttendanceAsync(AttendanceModel attendance, CancellationToken cancellationToken = default)
        {
            return _repository.UpsertAttendanceAsync(attendance, cancellationToken);
        }

        public Task<IEnumerable<AttendanceModel>> SaveBulkAttendanceAsync(List<AttendanceModel> attendances, CancellationToken cancellationToken = default)
        {
            return _repository.UpsertBulkAttendanceAsync(attendances, cancellationToken);
        }

        // --- Transactions ---
        public Task<IEnumerable<EventTransactionModel>> GetTransactionsAsync(CancellationToken cancellationToken = default)
        {
            return _repository.GetAllTransactionsAsync(cancellationToken);
        }

        public Task<IEnumerable<EventTransactionModel>> GetTransactionsByEventAsync(string eventId, CancellationToken cancellationToken = default)
        {
            return _repository.GetTransactionsByEventAsync(eventId, cancellationToken);
        }

        public Task<EventTransactionModel> SaveTransactionAsync(EventTransactionModel transaction, CancellationToken cancellationToken = default)
        {
            return _repository.UpsertTransactionAsync(transaction, cancellationToken);
        }

        public Task<bool> DeleteTransactionAsync(string transactionId, CancellationToken cancellationToken = default)
        {
            return _repository.DeleteTransactionAsync(transactionId, cancellationToken);
        }

        // --- Salary Payments ---
        public Task<IEnumerable<SalaryPaymentModel>> GetSalaryPaymentsAsync(CancellationToken cancellationToken = default)
        {
            return _repository.GetAllSalaryPaymentsAsync(cancellationToken);
        }

        public Task<IEnumerable<SalaryPaymentModel>> GetSalaryPaymentsByEmployeeAsync(string employeeId, CancellationToken cancellationToken = default)
        {
            return _repository.GetSalaryPaymentsByEmployeeAsync(employeeId, cancellationToken);
        }

        public Task<SalaryPaymentModel> ProcessSalaryPayoutAsync(CreateSalaryPayoutRequest request, CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            var payment = new SalaryPaymentModel
            {
                PaymentId = $"pay_{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}",
                EmployeeId = request.EmployeeId,
                AttendanceIds = request.AttendanceIds ?? new List<string>(),
                AmountPaid = request.AmountPaid,
                Month = string.IsNullOrWhiteSpace(request.Month) ? now.ToString("yyyy-MM") : request.Month,
                PaidOn = now.ToString("o"),
                Mode = request.Mode,
                PaidBy = request.PaidBy
            };

            return _repository.AddSalaryPaymentAsync(payment, cancellationToken);
        }
    }
}
