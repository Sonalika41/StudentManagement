//using ApiCallAdv.Data;
//using ApiCallAdv.Models.Domain;
//using ApiCallAdv.Repositories.Interface;
//using Microsoft.EntityFrameworkCore;

//namespace ApiCallAdv.Repositories.Implementation
//{
//    public class StudentRepository : IStudentRepository
//    {
//        private readonly ApplicationDbContext dbContext;
//        public StudentRepository(ApplicationDbContext dbContext) => this.dbContext = dbContext;

//        public async Task<Student> AddAsync(Student student)
//        {
//            if (student.StudentID == default) student.StudentID = Guid.NewGuid();
//            if (student.EnrollmentDate == default) student.EnrollmentDate = DateTime.Now;
//            await dbContext.Students.AddAsync(student);
//            await dbContext.SaveChangesAsync();
//            return student;
//        }

//        public async Task<Student?> DeleteAsync(Guid id)
//        {
//            var existing = await dbContext.Students.FirstOrDefaultAsync(s => s.StudentID == id);
//            if (existing == null) return null;
//            dbContext.Students.Remove(existing);
//            await dbContext.SaveChangesAsync();
//            return existing;
//        }

//        public async Task<Student?> GetByIdAsync(Guid id)
//        {
//            return await dbContext.Students.Include(s => s.Class)
//                                           .FirstOrDefaultAsync(s => s.StudentID == id);
//        }

//        public async Task<Student?> GetByIdentityUserIdAsync(string identityUserId)
//        {
//            return await dbContext.Students.Include(s => s.Class)
//                                           .FirstOrDefaultAsync(s => s.IdentityUserId == identityUserId);
//        }

//        public async Task<IEnumerable<Student>> GetByClassAsync(Guid classId)
//        {
//            return await dbContext.Students.Where(s => s.ClassId == classId).ToListAsync();
//        }

//        public async Task<Student?> UpdateAsync(Guid id, Student student)
//        {
//            var existing = await dbContext.Students.FirstOrDefaultAsync(s => s.StudentID == id);
//            if (existing == null) return null;

//            existing.FullName = student.FullName;
//            existing.Age = student.Age;
//            existing.Email = student.Email;
//            existing.Course = student.Course;
//            existing.EnrollmentDate = student.EnrollmentDate;
//            existing.Marks = student.Marks;
//            existing.AttendancePercentage = student.AttendancePercentage;
//            existing.GuardianContact = student.GuardianContact;
//            existing.ClassId = student.ClassId;

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
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext dbContext;
        public StudentRepository(ApplicationDbContext dbContext) => this.dbContext = dbContext;

        // ✅ NEW: Fetch all students with class info
        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await dbContext.Students
                .Include(s => s.Class)
                .ToListAsync();
        }

        public async Task<Student> AddAsync(Student student)
        {
            if (student.StudentID == default) student.StudentID = Guid.NewGuid();
            if (student.EnrollmentDate == default) student.EnrollmentDate = DateTime.Now;
            await dbContext.Students.AddAsync(student);
            await dbContext.SaveChangesAsync();
            return student;
        }

        public async Task<Student?> DeleteAsync(Guid id)
        {
            var existing = await dbContext.Students.FirstOrDefaultAsync(s => s.StudentID == id);
            if (existing == null) return null;
            dbContext.Students.Remove(existing);
            await dbContext.SaveChangesAsync();
            return existing;
        }

        public async Task<Student?> GetByIdAsync(Guid id)
        {
            return await dbContext.Students
                .Include(s => s.Class)
                .FirstOrDefaultAsync(s => s.StudentID == id);
        }

        public async Task<Student?> GetByIdentityUserIdAsync(string identityUserId)
        {
            return await dbContext.Students
                .Include(s => s.Class)
                .FirstOrDefaultAsync(s => s.IdentityUserId == identityUserId);
        }

        public async Task<IEnumerable<Student>> GetByClassAsync(Guid classId)
        {
            return await dbContext.Students
                .Where(s => s.ClassId == classId)
                .Include(s => s.Class)
                .ToListAsync();
        }

        public async Task<Student?> UpdateAsync(Guid id, Student student)
        {
            var existing = await dbContext.Students.FirstOrDefaultAsync(s => s.StudentID == id);
            if (existing == null) return null;

            existing.FullName = student.FullName;
            existing.Age = student.Age;
            existing.Email = student.Email;
            existing.Course = student.Course;
            existing.EnrollmentDate = student.EnrollmentDate;
            existing.Marks = student.Marks;
            existing.AttendancePercentage = student.AttendancePercentage;
            existing.GuardianContact = student.GuardianContact;
            existing.ClassId = student.ClassId;

            await dbContext.SaveChangesAsync();
            return existing;
        }
    }
}
