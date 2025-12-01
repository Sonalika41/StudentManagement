using ApiCallAdv.Data;
using ApiCallAdv.Models.Domain;
using ApiCallAdv.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ApiCallAdv.Repositories.Implementation
{
    public class ClassRepository : IClassRepository
    {
        private readonly ApplicationDbContext dbContext;
        public ClassRepository(ApplicationDbContext dbContext) => this.dbContext = dbContext;

        public async Task<Class> AddAsync(Class cls)
        {
            cls.ClassId = Guid.NewGuid();
            await dbContext.Classes.AddAsync(cls);
            await dbContext.SaveChangesAsync();
            return cls;
        }

        public async Task<Class?> DeleteAsync(Guid id)
        {
            var existing = await dbContext.Classes.FirstOrDefaultAsync(c => c.ClassId == id);
            if (existing == null) return null;
            dbContext.Classes.Remove(existing);
            await dbContext.SaveChangesAsync();
            return existing;
        }

        public async Task<IEnumerable<Class>> GetAllAsync()
        {
            return await dbContext.Classes.Include(c => c.Students).ToListAsync();
        }

        public async Task<Class?> GetByIdAsync(Guid id)
        {
            return await dbContext.Classes.Include(c => c.Students)
                                          .FirstOrDefaultAsync(c => c.ClassId == id);
        }

        public async Task<Class?> UpdateAsync(Guid id, Class cls)
        {
            var existing = await dbContext.Classes.FirstOrDefaultAsync(c => c.ClassId == id);
            if (existing == null) return null;

            existing.Name = cls.Name;
            existing.Section = cls.Section;
            existing.ClassTeacherUserId = cls.ClassTeacherUserId;

            await dbContext.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> AssignClassTeacherAsync(Guid classId, string teacherUserId)
        {
            var cls = await dbContext.Classes.FirstOrDefaultAsync(c => c.ClassId == classId);
            if (cls == null) return false;
            cls.ClassTeacherUserId = teacherUserId;
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
