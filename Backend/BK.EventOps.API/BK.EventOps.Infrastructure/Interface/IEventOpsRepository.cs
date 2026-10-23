using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BK.EventOps.Common.Models;

namespace BK.EventOps.Infrastructure.Interface
{
    public interface IEventOpsRepository
    {
        // Employees
        Task<IEnumerable<EmployeeModel>> GetAllEmployeesAsync(CancellationToken cancellationToken = default);
        Task<EmployeeModel?> GetEmployeeByIdAsync(string employeeId, CancellationToken cancellationToken = default);
        Task<EmployeeModel> UpsertEmployeeAsync(EmployeeModel employee, CancellationToken cancellationToken = default);
        Task<EmployeeModel?> ToggleEmployeeStatusAsync(string employeeId, CancellationToken cancellationToken = default);
        Task<bool> DeleteEmployeeAsync(string employeeId, CancellationToken cancellationToken = default);

        // Events
        Task<IEnumerable<EventModel>> GetAllEventsAsync(CancellationToken cancellationToken = default);
        Task<EventModel?> GetEventByIdAsync(string eventId, CancellationToken cancellationToken = default);
        Task<EventModel> UpsertEventAsync(EventModel eventModel, CancellationToken cancellationToken = default);
        Task<EventModel?> UpdateEventStatusAsync(string eventId, string status, CancellationToken cancellationToken = default);
        Task<bool> DeleteEventAsync(string eventId, CancellationToken cancellationToken = default);

        // Attendance
        Task<IEnumerable<AttendanceModel>> GetAllAttendanceAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<AttendanceModel>> GetAttendanceByEventAsync(string eventId, CancellationToken cancellationToken = default);
        Task<AttendanceModel> UpsertAttendanceAsync(AttendanceModel attendance, CancellationToken cancellationToken = default);
        Task<IEnumerable<AttendanceModel>> UpsertBulkAttendanceAsync(List<AttendanceModel> attendances, CancellationToken cancellationToken = default);

        // Transactions
        Task<IEnumerable<EventTransactionModel>> GetAllTransactionsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<EventTransactionModel>> GetTransactionsByEventAsync(string eventId, CancellationToken cancellationToken = default);
        Task<EventTransactionModel> UpsertTransactionAsync(EventTransactionModel transaction, CancellationToken cancellationToken = default);
        Task<bool> DeleteTransactionAsync(string transactionId, CancellationToken cancellationToken = default);

        // Salary Payments
        Task<IEnumerable<SalaryPaymentModel>> GetAllSalaryPaymentsAsync(CancellationToken cancellationToken = default);
        Task<SalaryPaymentModel> AddSalaryPaymentAsync(SalaryPaymentModel payment, CancellationToken cancellationToken = default);
    }
}
