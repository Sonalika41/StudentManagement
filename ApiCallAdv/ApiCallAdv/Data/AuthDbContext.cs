using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ApiCallAdv.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var studentRoleId = "11111111-1111-1111-1111-111111111111";
            var teacherRoleId = "22222222-2222-2222-2222-222222222222";
            var principalRoleId = "33333333-3333-3333-3333-333333333333";
            var itRoleId = "44444444-4444-4444-4444-444444444444";

            var roles = new List<IdentityRole>
            {
                new() { Id = studentRoleId, Name = "Student", NormalizedName = "STUDENT", ConcurrencyStamp = studentRoleId },
                new() { Id = teacherRoleId, Name = "Teacher", NormalizedName = "TEACHER", ConcurrencyStamp = teacherRoleId },
                new() { Id = principalRoleId, Name = "Principal", NormalizedName = "PRINCIPAL", ConcurrencyStamp = principalRoleId },
                new() { Id = itRoleId, Name = "IT", NormalizedName = "IT", ConcurrencyStamp = itRoleId }
            };
            builder.Entity<IdentityRole>().HasData(roles);

            // Seed IT superadmin
            var itAdminUserId = "55555555-5555-5555-5555-555555555555";
            var itAdmin = new IdentityUser
            {
                Id = itAdminUserId,
                UserName = "admin@school.local",
                Email = "admin@school.local",
                NormalizedEmail = "ADMIN@SCHOOL.LOCAL",
                NormalizedUserName = "ADMIN@SCHOOL.LOCAL",
                EmailConfirmed = true
            };
            itAdmin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(itAdmin, "Admin@12345");
            builder.Entity<IdentityUser>().HasData(itAdmin);
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                UserId = itAdminUserId,
                RoleId = itRoleId
            });
        }
    }
}
