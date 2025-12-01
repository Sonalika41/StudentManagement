////using ApiCallAdv.Models.DTO;
////using ApiCallAdv.Repositories.Interface;
////using ApiCallAdv.Data;
////using ApiCallAdv.Models.Domain;
////using Microsoft.AspNetCore.Authorization;
////using Microsoft.AspNetCore.Identity;
////using Microsoft.AspNetCore.Mvc;
////using Microsoft.EntityFrameworkCore;

////namespace ApiCallAdv.Controllers
////{
////    [Route("api/[controller]")]
////    [ApiController]
////    public class AuthController : ControllerBase
////    {
////        private readonly UserManager<IdentityUser> userManager;
////        private readonly RoleManager<IdentityRole> roleManager;
////        private readonly ITokenRepository tokenRepository;
////        private readonly ApplicationDbContext appDb;

////        public AuthController(UserManager<IdentityUser> userManager,
////                              RoleManager<IdentityRole> roleManager,
////                              ITokenRepository tokenRepository,
////                              ApplicationDbContext appDb)
////        {
////            this.userManager = userManager;
////            this.roleManager = roleManager;
////            this.tokenRepository = tokenRepository;
////            this.appDb = appDb;
////        }

////        // Register Student | Teacher | Principal (IT is seeded only)
////        [HttpPost("register")]
////        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
////        {
////            var allowedRoles = new[] { "Student", "Teacher", "Principal" };
////            if (!allowedRoles.Contains(request.Role, StringComparer.OrdinalIgnoreCase))
////            {
////                ModelState.AddModelError("", "Only Student, Teacher, or Principal roles can be registered.");
////                return ValidationProblem(ModelState);
////            }

////            var user = new IdentityUser
////            {
////                UserName = request.Email.Trim(),
////                Email = request.Email.Trim(),
////                EmailConfirmed = true
////            };

////            var identityResult = await userManager.CreateAsync(user, request.Password);
////            if (!identityResult.Succeeded)
////            {
////                foreach (var err in identityResult.Errors)
////                    ModelState.AddModelError("", err.Description);
////                return ValidationProblem(ModelState);
////            }

////            if (!await roleManager.RoleExistsAsync(request.Role))
////                await roleManager.CreateAsync(new IdentityRole(request.Role));

////            await userManager.AddToRoleAsync(user, request.Role);

////            Guid? studentId = null;

////            if (request.Role.Equals("Student", StringComparison.OrdinalIgnoreCase))
////            {
////                var student = new Student
////                {
////                    StudentID = Guid.NewGuid(),
////                    FullName = request.FullName ?? request.Email,
////                    Age = 0,
////                    Email = request.Email,
////                    Course = "General",
////                    EnrollmentDate = DateTime.Now,
////                    Marks = 0,
////                    AttendancePercentage = 0,
////                    IdentityUserId = user.Id,
////                    ClassId = null
////                };
////                await appDb.Students.AddAsync(student);
////                await appDb.SaveChangesAsync();
////                studentId = student.StudentID;
////            }

////            return Ok(new
////            {
////                Message = "Registered successfully.",
////                Role = request.Role,
////                IdentityUserId = user.Id,
////                StudentID = studentId
////            });
////        }

////        // Login — builds claims with StudentID or TeacherClassId where applicable
////        [HttpPost("login")]
////        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
////        {
////            var identityUser = await userManager.FindByEmailAsync(request.Email);
////            if (identityUser is null)
////            {
////                ModelState.AddModelError("", "Email or Password Incorrect");
////                return ValidationProblem(ModelState);
////            }

////            var checkPasswordResult = await userManager.CheckPasswordAsync(identityUser, request.Password);
////            if (!checkPasswordResult)
////            {
////                ModelState.AddModelError("", "Email or Password Incorrect");
////                return ValidationProblem(ModelState);
////            }

////            var roles = (await userManager.GetRolesAsync(identityUser)).ToList();

////            Guid? studentIdClaim = null;
////            Guid? teacherClassIdClaim = null;

////            if (roles.Contains("Student"))
////            {
////                var student = await appDb.Students.FirstOrDefaultAsync(s => s.IdentityUserId == identityUser.Id);
////                if (student != null)
////                    studentIdClaim = student.StudentID;
////            }

////            if (roles.Contains("Teacher"))
////            {
////                var teacherClass = await appDb.Classes.FirstOrDefaultAsync(c => c.ClassTeacherUserId == identityUser.Id);
////                if (teacherClass != null)
////                    teacherClassIdClaim = teacherClass.ClassId;
////            }

////            var jwtToken = tokenRepository.CreateJwtToken(identityUser, roles, studentIdClaim, teacherClassIdClaim);

////            return Ok(new LoginResponseDto
////            {
////                Email = request.Email,
////                Roles = roles,
////                Token = jwtToken,
////                StudentID = studentIdClaim,
////                TeacherClassId = teacherClassIdClaim
////            });
////        }

////        [HttpPost("assign-role")]
////        [Authorize(Roles = "IT")]
////        public async Task<IActionResult> AssignRole([FromBody] RoleDto request)
////        {
////            var user = await userManager.FindByIdAsync(request.UserId);
////            if (user == null) return NotFound(new { Message = "User not found." });

////            var allowedRoles = new[] { "Student", "Teacher", "Principal" };
////            if (!allowedRoles.Contains(request.Role, StringComparer.OrdinalIgnoreCase))
////                return BadRequest(new { Message = "Only Student, Teacher, or Principal roles can be assigned." });

////            var currentRoles = await userManager.GetRolesAsync(user);

////            if (currentRoles.Contains("Student") && (request.Role == "Teacher" || request.Role == "Principal"))
////                return BadRequest(new { Message = "Students cannot be reassigned to Teacher or Principal." });

////            if (currentRoles.Contains("Teacher") && request.Role == "Principal") { }
////            else if (currentRoles.Contains("Principal") && request.Role == "Teacher") { }
////            else if (currentRoles.Contains("Student") && request.Role == "Student") { }
////            else if (currentRoles.Contains(request.Role))
////                return BadRequest(new { Message = $"User already has role {request.Role}." });
////            else
////                return BadRequest(new { Message = "Invalid role transition." });

////            if (!await roleManager.RoleExistsAsync(request.Role))
////                await roleManager.CreateAsync(new IdentityRole(request.Role));

////            var result = await userManager.AddToRoleAsync(user, request.Role);
////            if (result.Succeeded) return Ok(new { Message = $"Role {request.Role} assigned to {user.Email}" });

////            return BadRequest(result.Errors);
////        }

////        [HttpPost("remove-role")]
////        [Authorize(Roles = "IT")]
////        public async Task<IActionResult> RemoveRole([FromBody] RoleDto request)
////        {
////            var user = await userManager.FindByIdAsync(request.UserId);
////            if (user == null) return NotFound(new { Message = "User not found." });

////            var currentRoles = await userManager.GetRolesAsync(user);

////            if (request.Role == "Student")
////                return BadRequest(new { Message = "Cannot remove Student role." });

////            if (request.Role != "Teacher" && request.Role != "Principal")
////                return BadRequest(new { Message = "Only Teacher or Principal roles can be removed." });

////            if (!currentRoles.Contains(request.Role))
////                return BadRequest(new { Message = $"User does not have role {request.Role}." });

////            var result = await userManager.RemoveFromRoleAsync(user, request.Role);
////            if (result.Succeeded) return Ok(new { Message = $"Role {request.Role} removed from {user.Email}" });

////            return BadRequest(result.Errors);
////        }

////        [HttpGet("users")]
////        [Authorize(Roles = "IT")]
////        public async Task<IActionResult> GetAllUsers()
////        {
////            var users = userManager.Users.ToList();
////            var result = new List<object>();
////            foreach (var user in users)
////            {
////                var roles = await userManager.GetRolesAsync(user);
////                result.Add(new { id = user.Id, email = user.Email, roles });
////            }
////            return Ok(result);
////        }

////        // ✅ New endpoint: Teachers only (IT + Principal)
////        [HttpGet("teachers")]
////        [Authorize(Roles = "IT,Principal")]
////        public async Task<IActionResult> GetTeachers()
////        {
////            var users = userManager.Users.ToList();
////            var result = new List<object>();
////            foreach (var user in users)
////            {
////                var roles = await userManager.GetRolesAsync(user);
////                if (roles.Contains("Teacher"))
////                {
////                    result.Add(new { id = user.Id, email = user.Email });
////                }
////            }
////            return Ok(result);
////        }

////        [HttpDelete("user/{userId}")]
////        [Authorize(Roles = "IT")]
////        public async Task<IActionResult> DeleteUser(string userId)
////        {
////            var user = await userManager.FindByIdAsync(userId);
////            if (user == null) return NotFound(new { Message = "User not found." });

////            var result = await userManager.DeleteAsync(user);
////            if (result.Succeeded)
////                return Ok(new { Message = "User deleted successfully." });

////            return BadRequest(result.Errors);
////        }
////    }
////}


//using ApiCallAdv.Models.DTO;
//using ApiCallAdv.Repositories.Interface;
//using ApiCallAdv.Data;
//using ApiCallAdv.Models.Domain;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace ApiCallAdv.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AuthController : ControllerBase
//    {
//        private readonly UserManager<IdentityUser> userManager;
//        private readonly RoleManager<IdentityRole> roleManager;
//        private readonly ITokenRepository tokenRepository;
//        private readonly ApplicationDbContext appDb;

//        private const string SuperAdminEmail = "admin@school.local"; // ✅ central constant

//        public AuthController(UserManager<IdentityUser> userManager,
//                              RoleManager<IdentityRole> roleManager,
//                              ITokenRepository tokenRepository,
//                              ApplicationDbContext appDb)
//        {
//            this.userManager = userManager;
//            this.roleManager = roleManager;
//            this.tokenRepository = tokenRepository;
//            this.appDb = appDb;
//        }

//        // Register Student | Teacher | Principal (IT is seeded only)
//        [HttpPost("register")]
//        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
//        {
//            var allowedRoles = new[] { "Student", "Teacher", "Principal" };
//            if (!allowedRoles.Contains(request.Role, StringComparer.OrdinalIgnoreCase))
//            {
//                ModelState.AddModelError("", "Only Student, Teacher, or Principal roles can be registered.");
//                return ValidationProblem(ModelState);
//            }

//            var user = new IdentityUser
//            {
//                UserName = request.Email.Trim(),
//                Email = request.Email.Trim(),
//                EmailConfirmed = true
//            };

//            var identityResult = await userManager.CreateAsync(user, request.Password);
//            if (!identityResult.Succeeded)
//            {
//                foreach (var err in identityResult.Errors)
//                    ModelState.AddModelError("", err.Description);
//                return ValidationProblem(ModelState);
//            }

//            if (!await roleManager.RoleExistsAsync(request.Role))
//                await roleManager.CreateAsync(new IdentityRole(request.Role));

//            await userManager.AddToRoleAsync(user, request.Role);

//            Guid? studentId = null;

//            if (request.Role.Equals("Student", StringComparison.OrdinalIgnoreCase))
//            {
//                var student = new Student
//                {
//                    StudentID = Guid.NewGuid(),
//                    FullName = request.FullName ?? request.Email,
//                    Age = 0,
//                    Email = request.Email,
//                    Course = "General",
//                    EnrollmentDate = DateTime.Now,
//                    Marks = 0,
//                    AttendancePercentage = 0,
//                    IdentityUserId = user.Id,
//                    ClassId = null
//                };
//                await appDb.Students.AddAsync(student);
//                await appDb.SaveChangesAsync();
//                studentId = student.StudentID;
//            }

//            return Ok(new
//            {
//                Message = "Registered successfully.",
//                Role = request.Role,
//                IdentityUserId = user.Id,
//                StudentID = studentId
//            });
//        }

//        // Login — builds claims with StudentID or TeacherClassId where applicable
//        [HttpPost("login")]
//        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
//        {
//            var identityUser = await userManager.FindByEmailAsync(request.Email);
//            if (identityUser is null)
//            {
//                ModelState.AddModelError("", "Email or Password Incorrect");
//                return ValidationProblem(ModelState);
//            }

//            var checkPasswordResult = await userManager.CheckPasswordAsync(identityUser, request.Password);
//            if (!checkPasswordResult)
//            {
//                ModelState.AddModelError("", "Email or Password Incorrect");
//                return ValidationProblem(ModelState);
//            }

//            var roles = (await userManager.GetRolesAsync(identityUser)).ToList();

//            Guid? studentIdClaim = null;
//            Guid? teacherClassIdClaim = null;

//            if (roles.Contains("Student"))
//            {
//                var student = await appDb.Students.FirstOrDefaultAsync(s => s.IdentityUserId == identityUser.Id);
//                if (student != null)
//                    studentIdClaim = student.StudentID;
//            }

//            if (roles.Contains("Teacher"))
//            {
//                var teacherClass = await appDb.Classes.FirstOrDefaultAsync(c => c.ClassTeacherUserId == identityUser.Id);
//                if (teacherClass != null)
//                    teacherClassIdClaim = teacherClass.ClassId;
//            }

//            var jwtToken = tokenRepository.CreateJwtToken(identityUser, roles, studentIdClaim, teacherClassIdClaim);

//            return Ok(new LoginResponseDto
//            {
//                Email = request.Email,
//                Roles = roles,
//                Token = jwtToken,
//                StudentID = studentIdClaim,
//                TeacherClassId = teacherClassIdClaim
//            });
//        }

//        [HttpPost("assign-role")]
//        [Authorize(Roles = "IT")]
//        public async Task<IActionResult> AssignRole([FromBody] RoleDto request)
//        {
//            var user = await userManager.FindByIdAsync(request.UserId);
//            if (user == null) return NotFound(new { Message = "User not found." });

//            if (user.Email == SuperAdminEmail)
//                return BadRequest(new { Message = "Superadmin cannot be modified." });

//            var allowedRoles = new[] { "Student", "Teacher", "Principal" };
//            if (!allowedRoles.Contains(request.Role, StringComparer.OrdinalIgnoreCase))
//                return BadRequest(new { Message = "Only Student, Teacher, or Principal roles can be assigned." });

//            var currentRoles = await userManager.GetRolesAsync(user);

//            if (currentRoles.Contains("Student") && (request.Role == "Teacher" || request.Role == "Principal"))
//                return BadRequest(new { Message = "Students cannot be reassigned to Teacher or Principal." });

//            if (currentRoles.Contains("Teacher") && request.Role == "Principal") { }
//            else if (currentRoles.Contains("Principal") && request.Role == "Teacher") { }
//            else if (currentRoles.Contains("Student") && request.Role == "Student") { }
//            else if (currentRoles.Contains(request.Role))
//                return BadRequest(new { Message = $"User already has role {request.Role}." });
//            else
//                return BadRequest(new { Message = "Invalid role transition." });

//            if (!await roleManager.RoleExistsAsync(request.Role))
//                await roleManager.CreateAsync(new IdentityRole(request.Role));

//            var result = await userManager.AddToRoleAsync(user, request.Role);
//            if (result.Succeeded) return Ok(new { Message = $"Role {request.Role} assigned to {user.Email}" });

//            return BadRequest(result.Errors);
//        }

//        [HttpPost("remove-role")]
//        [Authorize(Roles = "IT")]
//        public async Task<IActionResult> RemoveRole([FromBody] RoleDto request)
//        {
//            var user = await userManager.FindByIdAsync(request.UserId);
//            if (user == null) return NotFound(new { Message = "User not found." });

//            if (user.Email == SuperAdminEmail)
//                return BadRequest(new { Message = "Superadmin cannot be modified." });

//            var currentRoles = await userManager.GetRolesAsync(user);

//            if (request.Role == "Student")
//                return BadRequest(new { Message = "Cannot remove Student role." });

//            if (request.Role != "Teacher" && request.Role != "Principal")
//                return BadRequest(new { Message = "Only Teacher or Principal roles can be removed." });

//            if (!currentRoles.Contains(request.Role))
//                return BadRequest(new { Message = $"User does not have role {request.Role}." });

//            var result = await userManager.RemoveFromRoleAsync(user, request.Role);
//            if (result.Succeeded) return Ok(new { Message = $"Role {request.Role} removed from {user.Email}" });

//            return BadRequest(result.Errors);
//        }

//        [HttpGet("users")]
//        [Authorize(Roles = "IT")]
//        public async Task<IActionResult> GetAllUsers()
//        {
//            var users = userManager.Users
//                .Where(u => u.Email != SuperAdminEmail) // ✅ exclude superadmin
//                .ToList();

//            var result = new List<object>();
//            foreach (var user in users)
//            {
//                var roles = await userManager.GetRolesAsync(user);
//                result.Add(new { id = user.Id, email = user.Email, roles });
//            }
//            return Ok(result);
//        }

//        [HttpGet("teachers")]
//        [Authorize(Roles = "IT,Principal")]
//        public async Task<IActionResult> GetTeachers()
//        {
//            var users = userManager.Users.ToList();
//            var result = new List<object>();
//            foreach (var user in users)
//            {
//                var roles = await userManager.GetRolesAsync(user);
//                if (roles.Contains("Teacher"))
//                {
//                    result.Add(new { id = user.Id, email = user.Email });
//                }
//            }
//            return Ok(result);
//        }

//        [HttpDelete("user/{userId}")]
//        [Authorize(Roles = "IT")]
//        public async Task<IActionResult> DeleteUser(string userId)
//        {
//            var user = await userManager.FindByIdAsync(userId);
//            if (user == null) return NotFound(new { Message = "User not found." });

//            if (user.Email == SuperAdminEmail)
//                return BadRequest(new { Message = "Superadmin cannot be deleted." });

//            var result = await userManager.DeleteAsync(user);
//            if (result.Succeeded)
//                return Ok(new { Message = "User deleted successfully." });

//            return BadRequest(result.Errors);
//        }
//    }
//}


using ApiCallAdv.Models.DTO;
using ApiCallAdv.Repositories.Interface;
using ApiCallAdv.Data;
using ApiCallAdv.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCallAdv.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ITokenRepository tokenRepository;
        private readonly ApplicationDbContext appDb;

        private const string SuperAdminEmail = "admin@school.local"; // central constant

        public AuthController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ITokenRepository tokenRepository,
            ApplicationDbContext appDb)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.tokenRepository = tokenRepository;
            this.appDb = appDb;
        }

        // Register Student | Teacher | Principal (IT is seeded only)
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var allowedRoles = new[] { "Student", "Teacher", "Principal" };
            if (!allowedRoles.Contains(request.Role, StringComparer.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("", "Only Student, Teacher, or Principal roles can be registered.");
                return ValidationProblem(ModelState);
            }

            var user = new IdentityUser
            {
                UserName = request.Email.Trim(),
                Email = request.Email.Trim(),
                EmailConfirmed = true
            };

            var identityResult = await userManager.CreateAsync(user, request.Password);
            if (!identityResult.Succeeded)
            {
                foreach (var err in identityResult.Errors)
                    ModelState.AddModelError("", err.Description);
                return ValidationProblem(ModelState);
            }

            if (!await roleManager.RoleExistsAsync(request.Role))
                await roleManager.CreateAsync(new IdentityRole(request.Role));

            await userManager.AddToRoleAsync(user, request.Role);

            Guid? studentId = null;

            if (request.Role.Equals("Student", StringComparison.OrdinalIgnoreCase))
            {
                var student = new Student
                {
                    StudentID = Guid.NewGuid(),
                    FullName = request.FullName ?? request.Email,
                    Age = 0,
                    Email = request.Email,
                    Course = "General",
                    EnrollmentDate = DateTime.Now,
                    Marks = 0,
                    AttendancePercentage = 0,
                    IdentityUserId = user.Id,
                    ClassId = null
                };

                await appDb.Students.AddAsync(student);
                await appDb.SaveChangesAsync();
                studentId = student.StudentID;
            }

            return Ok(new
            {
                Message = "Registered successfully.",
                Role = request.Role,
                IdentityUserId = user.Id,
                StudentID = studentId
            });
        }

        // Login — builds claims with StudentID or TeacherClassId where applicable
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var identityUser = await userManager.FindByEmailAsync(request.Email);
            if (identityUser is null)
            {
                ModelState.AddModelError("", "Email or Password Incorrect");
                return ValidationProblem(ModelState);
            }

            var checkPasswordResult = await userManager.CheckPasswordAsync(identityUser, request.Password);
            if (!checkPasswordResult)
            {
                ModelState.AddModelError("", "Email or Password Incorrect");
                return ValidationProblem(ModelState);
            }

            var roles = (await userManager.GetRolesAsync(identityUser)).ToList();

            Guid? studentIdClaim = null;
            Guid? teacherClassIdClaim = null;

            if (roles.Contains("Student"))
            {
                var student = await appDb.Students.FirstOrDefaultAsync(s => s.IdentityUserId == identityUser.Id);
                if (student != null)
                    studentIdClaim = student.StudentID;
            }

            if (roles.Contains("Teacher"))
            {
                var teacherClass = await appDb.Classes.FirstOrDefaultAsync(c => c.ClassTeacherUserId == identityUser.Id);
                if (teacherClass != null)
                    teacherClassIdClaim = teacherClass.ClassId;
            }

            var jwtToken = tokenRepository.CreateJwtToken(identityUser, roles, studentIdClaim, teacherClassIdClaim);

            return Ok(new LoginResponseDto
            {
                Email = request.Email,
                Roles = roles,
                Token = jwtToken,
                StudentID = studentIdClaim,
                TeacherClassId = teacherClassIdClaim
            });
        }

        // Assign role (IT only)
        [HttpPost("assign-role")]
        [Authorize(Roles = "IT")]
        public async Task<IActionResult> AssignRole([FromBody] RoleDto request)
        {
            var user = await userManager.FindByIdAsync(request.UserId);
            if (user == null) return NotFound(new { Message = "User not found." });

            if (user.Email == SuperAdminEmail)
                return BadRequest(new { Message = "Superadmin cannot be modified." });

            var allowedRoles = new[] { "Student", "Teacher", "Principal" };
            if (!allowedRoles.Contains(request.Role, StringComparer.OrdinalIgnoreCase))
                return BadRequest(new { Message = "Only Student, Teacher, or Principal roles can be assigned." });

            var currentRoles = await userManager.GetRolesAsync(user);

            if (currentRoles.Contains("Student") && (request.Role == "Teacher" || request.Role == "Principal"))
                return BadRequest(new { Message = "Students cannot be reassigned to Teacher or Principal." });

            if (currentRoles.Contains("Teacher") && request.Role == "Principal") { }
            else if (currentRoles.Contains("Principal") && request.Role == "Teacher") { }
            else if (currentRoles.Contains("Student") && request.Role == "Student") { }
            else if (currentRoles.Contains(request.Role))
                return BadRequest(new { Message = $"User already has role {request.Role}." });
            else
                return BadRequest(new { Message = "Invalid role transition." });

            if (!await roleManager.RoleExistsAsync(request.Role))
                await roleManager.CreateAsync(new IdentityRole(request.Role));

            var result = await userManager.AddToRoleAsync(user, request.Role);
            if (result.Succeeded) return Ok(new { Message = $"Role {request.Role} assigned to {user.Email}" });

            return BadRequest(result.Errors);
        }

        // Remove role (IT only)
        [HttpPost("remove-role")]
        [Authorize(Roles = "IT")]
        public async Task<IActionResult> RemoveRole([FromBody] RoleDto request)
        {
            var user = await userManager.FindByIdAsync(request.UserId);
            if (user == null) return NotFound(new { Message = "User not found." });

            if (user.Email == SuperAdminEmail)
                return BadRequest(new { Message = "Superadmin cannot be modified." });

            var currentRoles = await userManager.GetRolesAsync(user);

            if (request.Role == "Student")
                return BadRequest(new { Message = "Cannot remove Student role." });

            if (request.Role != "Teacher" && request.Role != "Principal")
                return BadRequest(new { Message = "Only Teacher or Principal roles can be removed." });

            if (!currentRoles.Contains(request.Role))
                return BadRequest(new { Message = $"User does not have role {request.Role}." });

            var result = await userManager.RemoveFromRoleAsync(user, request.Role);
            if (result.Succeeded) return Ok(new { Message = $"Role {request.Role} removed from {user.Email}" });

            return BadRequest(result.Errors);
        }

        // Get all users (excluding superadmin)
        [HttpGet("users")]
        [Authorize(Roles = "IT")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = userManager.Users
                .Where(u => u.Email != SuperAdminEmail)
                .ToList();

            var result = new List<object>();
            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                result.Add(new { id = user.Id, email = user.Email, roles });
            }
            return Ok(result);
        }

        // Get teachers (IT + Principal)
        [HttpGet("teachers")]
        [Authorize(Roles = "IT,Principal")]
        public async Task<IActionResult> GetTeachers()
        {
            var users = userManager.Users.ToList();
            var result = new List<object>();
            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                if (roles.Contains("Teacher"))
                {
                    result.Add(new { id = user.Id, email = user.Email });
                }
            }
            return Ok(result);
        }

        // Delete user (IT only) + manual cleanup for associated Student
        [HttpDelete("user/{userId}")]
        [Authorize(Roles = "IT")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return NotFound(new { Message = "User not found." });

            if (user.Email == SuperAdminEmail)
                return BadRequest(new { Message = "Superadmin cannot be deleted." });

            // Manual cleanup: remove associated Student record if present
            var student = await appDb.Students.FirstOrDefaultAsync(s => s.IdentityUserId == user.Id);
            if (student != null)
            {
                appDb.Students.Remove(student);
                await appDb.SaveChangesAsync();
            }

            var result = await userManager.DeleteAsync(user);
            if (result.Succeeded)
                return Ok(new { Message = "User (and associated student, if any) deleted successfully." });

            return BadRequest(result.Errors);
        }
    }
}
