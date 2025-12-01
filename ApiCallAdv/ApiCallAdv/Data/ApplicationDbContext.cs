using Microsoft.EntityFrameworkCore;
using ApiCallAdv.Models.Domain;

namespace ApiCallAdv.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<StudentLeaveRequest> StudentLeaveRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Class>(entity =>
            {
                entity.HasKey(c => c.ClassId);
                entity.Property(c => c.Name).IsRequired().HasMaxLength(50);
                entity.Property(c => c.Section).IsRequired().HasMaxLength(20);
                entity.Property(c => c.ClassTeacherUserId).IsRequired().HasMaxLength(450);
                entity.HasMany(c => c.Students)
                      .WithOne(s => s.Class)
                      .HasForeignKey(s => s.ClassId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(s => s.StudentID);
                entity.Property(s => s.FullName).IsRequired().HasMaxLength(200);
                entity.Property(s => s.Email).IsRequired().HasMaxLength(200);
                entity.Property(s => s.Course).IsRequired().HasMaxLength(150);
                entity.Property(s => s.EnrollmentDate).HasDefaultValueSql("GETDATE()");
                entity.Property(s => s.IdentityUserId).IsRequired().HasMaxLength(450);
                entity.Property(s => s.Marks).HasDefaultValue(0);
                entity.Property(s => s.AttendancePercentage).HasDefaultValue(0);
                entity.Property(s => s.GuardianContact).HasMaxLength(50);
            });

            modelBuilder.Entity<StudentLeaveRequest>(entity =>
            {
                entity.HasKey(l => l.LeaveId);
                entity.Property(l => l.Reason).IsRequired().HasMaxLength(500);
                entity.Property(l => l.Status).IsRequired().HasMaxLength(20);
                entity.HasOne(l => l.Student)
                      .WithMany()
                      .HasForeignKey(l => l.StudentId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
