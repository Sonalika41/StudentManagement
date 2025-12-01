using ApiCallAdv.Models.Domain;

namespace ApiCallAdv.Repositories.Interface
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetAllAsync(); // ✅ NEW: fetch all students

        Task<Student?> GetByIdAsync(Guid id);
        Task<Student?> GetByIdentityUserIdAsync(string identityUserId);
        Task<IEnumerable<Student>> GetByClassAsync(Guid classId);

        Task<Student> AddAsync(Student student);
        Task<Student?> UpdateAsync(Guid id, Student student);
        Task<Student?> DeleteAsync(Guid id);
    }
}
