using ApiCallAdv.Models.Domain;

namespace ApiCallAdv.Repositories.Interface
{
    public interface IClassRepository
    {
        Task<IEnumerable<Class>> GetAllAsync();
        Task<Class?> GetByIdAsync(Guid id);
        Task<Class> AddAsync(Class cls);
        Task<Class?> UpdateAsync(Guid id, Class cls);
        Task<Class?> DeleteAsync(Guid id);
        Task<bool> AssignClassTeacherAsync(Guid classId, string teacherUserId);
    }
}
