using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BK.EventOps.Common.Models;

namespace BK.EventOps.Application.Interface
{
    public interface IEventOpsService
    {
        // Employees
        Task<IEnumerable<EmployeeModel>> GetEmployeesAsync(CancellationToken cancellationToken = default);
        Task<EmployeeModel?> GetEmployeeByIdAsync(string employeeId, CancellationToken cancellationToken = default);
        Task<EmployeeModel> SaveEmployeeAsync(EmployeeModel employee, CancellationToken cancellationToken = default);
        Task<EmployeeModel?> ToggleEmployeeStatusAsync(string employeeId, CancellationToken cancellationToken = default);
        Task<EmployeeModel?> UpdateEmployeeStatusAsync(string employeeId, string status, CancellationToken cancellationToken = default);
        Task<bool> HasAttendanceForEmployeeAsync(string employeeId, CancellationToken cancellationToken = default);
        Task<bool> DeleteEmployeeAsync(string employeeId, CancellationToken cancellationToken = default);

        // Events
        Task<IEnumerable<EventModel>> GetEventsAsync(CancellationToken cancellationToken = default);
        Task<EventModel?> GetEventByIdAsync(string eventId, CancellationToken cancellationToken = default);
        Task<EventModel> SaveEventAsync(EventModel eventModel, CancellationToken cancellationToken = default);
        Task<EventModel?> UpdateEventStatusAsync(string eventId, string status, CancellationToken cancellationToken = default);
        Task<bool> HasTransactionsForEventAsync(string eventId, CancellationToken cancellationToken = default);
        Task<bool> DeleteEventAsync(string eventId, CancellationToken cancellationToken = default);

        // Attendance
        Task<IEnumerable<AttendanceModel>> GetAttendanceAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<AttendanceModel>> GetAttendanceByEventAsync(string eventId, CancellationToken cancellationToken = default);
        Task<AttendanceModel> SaveAttendanceAsync(AttendanceModel attendance, CancellationToken cancellationToken = default);
        Task<IEnumerable<AttendanceModel>> SaveBulkAttendanceAsync(List<AttendanceModel> attendances, CancellationToken cancellationToken = default);

        // Transactions
        Task<IEnumerable<EventTransactionModel>> GetTransactionsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<EventTransactionModel>> GetTransactionsByEventAsync(string eventId, CancellationToken cancellationToken = default);
        Task<EventTransactionModel> SaveTransactionAsync(EventTransactionModel transaction, CancellationToken cancellationToken = default);
        Task<bool> DeleteTransactionAsync(string transactionId, CancellationToken cancellationToken = default);

        // Salary Payments
        Task<IEnumerable<SalaryPaymentModel>> GetSalaryPaymentsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<SalaryPaymentModel>> GetSalaryPaymentsByEmployeeAsync(string employeeId, CancellationToken cancellationToken = default);
        Task<SalaryPaymentModel> ProcessSalaryPayoutAsync(CreateSalaryPayoutRequest request, CancellationToken cancellationToken = default);
    }
}
