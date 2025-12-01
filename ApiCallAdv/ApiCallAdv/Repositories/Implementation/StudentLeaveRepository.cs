//using ApiCallAdv.Data;
//using ApiCallAdv.Models.Domain;
//using ApiCallAdv.Repositories.Interface;
//using Microsoft.EntityFrameworkCore;

//namespace ApiCallAdv.Repositories.Implementation
//{
//    public class StudentLeaveRepository : IStudentLeaveRepository
//    {
//        private readonly ApplicationDbContext dbContext;
//        public StudentLeaveRepository(ApplicationDbContext dbContext) => this.dbContext = dbContext;

//        public async Task<StudentLeaveRequest> CreateAsync(StudentLeaveRequest leave)
//        {
//            leave.LeaveId = Guid.NewGuid();
//            leave.Status = string.IsNullOrWhiteSpace(leave.Status) ? "Pending" : leave.Status;
//            await dbContext.StudentLeaveRequests.AddAsync(leave);
//            await dbContext.SaveChangesAsync();
//            return leave;
//        }

//        public async Task<StudentLeaveRequest?> GetByIdAsync(Guid leaveId)
//        {
//            return await dbContext.StudentLeaveRequests.Include(l => l.Student)
//                                                       .FirstOrDefaultAsync(l => l.LeaveId == leaveId);
//        }

//        public async Task<IEnumerable<StudentLeaveRequest>> GetByStudentAsync(Guid studentId)
//        {
//            return await dbContext.StudentLeaveRequests.Where(l => l.StudentId == studentId).ToListAsync();
//        }

//        public async Task<StudentLeaveRequest?> UpdateStatusAsync(Guid leaveId, string status)
//        {
//            var existing = await dbContext.StudentLeaveRequests.FirstOrDefaultAsync(l => l.LeaveId == leaveId);
//            if (existing is null) return null;
//            existing.Status = status;
//            await dbContext.SaveChangesAsync();
//            return existing;
//        }
//    }
//}



//using ApiCallAdv.Data;
//using ApiCallAdv.Models.Domain;
//using ApiCallAdv.Repositories.Interface;
//using Microsoft.EntityFrameworkCore;

//namespace ApiCallAdv.Repositories.Implementation
//{
//    public class StudentLeaveRepository : IStudentLeaveRepository
//    {
//        private readonly ApplicationDbContext dbContext;
//        public StudentLeaveRepository(ApplicationDbContext dbContext) => this.dbContext = dbContext;

//        public async Task<StudentLeaveRequest> CreateAsync(StudentLeaveRequest leave)
//        {
//            leave.LeaveId = Guid.NewGuid();
//            leave.Status = string.IsNullOrWhiteSpace(leave.Status) ? "Pending" : leave.Status;
//            await dbContext.StudentLeaveRequests.AddAsync(leave);
//            await dbContext.SaveChangesAsync();
//            return leave;
//        }

//        public async Task<StudentLeaveRequest?> GetByIdAsync(Guid leaveId)
//        {
//            return await dbContext.StudentLeaveRequests
//                .Include(l => l.Student)
//                .ThenInclude(s => s.Class) // ensure we load Class for teacher validation
//                .FirstOrDefaultAsync(l => l.LeaveId == leaveId);
//        }

//        public async Task<IEnumerable<StudentLeaveRequest>> GetByStudentAsync(Guid studentId)
//        {
//            return await dbContext.StudentLeaveRequests
//                .Where(l => l.StudentId == studentId)
//                .ToListAsync();
//        }

//        public async Task<StudentLeaveRequest?> UpdateStatusAsync(Guid leaveId, string status)
//        {
//            var existing = await dbContext.StudentLeaveRequests.FirstOrDefaultAsync(l => l.LeaveId == leaveId);
//            if (existing is null) return null;

//            existing.Status = status;
//            await dbContext.SaveChangesAsync();
//            return existing;
//        }
//    }
//}


using ApiCallAdv.Data;
using ApiCallAdv.Models.Domain;
using ApiCallAdv.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ApiCallAdv.Repositories.Implementation
{
    public class StudentLeaveRepository : IStudentLeaveRepository
    {
        private readonly ApplicationDbContext dbContext;
        public StudentLeaveRepository(ApplicationDbContext dbContext) => this.dbContext = dbContext;

        public async Task<StudentLeaveRequest> CreateAsync(StudentLeaveRequest leave)
        {
            leave.LeaveId = Guid.NewGuid();
            leave.Status = string.IsNullOrWhiteSpace(leave.Status) ? "Pending" : leave.Status;
            await dbContext.StudentLeaveRequests.AddAsync(leave);
            await dbContext.SaveChangesAsync();
            return leave;
        }

        public async Task<StudentLeaveRequest?> GetByIdAsync(Guid leaveId)
        {
            return await dbContext.StudentLeaveRequests
                .Include(l => l.Student)
                .ThenInclude(s => s.Class) // needed for teacher validation
                .FirstOrDefaultAsync(l => l.LeaveId == leaveId);
        }

        public async Task<IEnumerable<StudentLeaveRequest>> GetByStudentAsync(Guid studentId)
        {
            return await dbContext.StudentLeaveRequests
                .Where(l => l.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<StudentLeaveRequest?> UpdateStatusAsync(Guid leaveId, string status)
        {
            var existing = await dbContext.StudentLeaveRequests.FirstOrDefaultAsync(l => l.LeaveId == leaveId);
            if (existing is null) return null;

            existing.Status = status;
            await dbContext.SaveChangesAsync();
            return existing;
        }

        // ✅ NEW: Get leave requests for all students in a class
        public async Task<IEnumerable<StudentLeaveRequest>> GetByClassIdAsync(Guid classId)
        {
            return await dbContext.StudentLeaveRequests
                .Include(l => l.Student)
                .Where(l => l.Student != null && l.Student.ClassId == classId)
                .ToListAsync();
        }
    }
}
