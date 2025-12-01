using ApiCallAdv.Models.Domain;

namespace ApiCallAdv.Repositories.Interface
{
    public interface IStudentLeaveRepository
    {
        Task<StudentLeaveRequest> CreateAsync(StudentLeaveRequest leave);
        Task<StudentLeaveRequest?> GetByIdAsync(Guid leaveId);
        Task<IEnumerable<StudentLeaveRequest>> GetByStudentAsync(Guid studentId);
        Task<StudentLeaveRequest?> UpdateStatusAsync(Guid leaveId, string status);
        Task<IEnumerable<StudentLeaveRequest>> GetByClassIdAsync(Guid classId);

    }
}
