



//using ApiCallAdv.Models.Domain;
//using ApiCallAdv.Models.DTO;
//using ApiCallAdv.Repositories.Interface;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;

//namespace ApiCallAdv.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class StudentController : ControllerBase
//    {
//        private readonly IStudentRepository studentRepo;
//        private readonly UserManager<IdentityUser> userManager;
//        private readonly RoleManager<IdentityRole> roleManager;

//        public StudentController(IStudentRepository studentRepo, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
//        {
//            this.studentRepo = studentRepo;
//            this.userManager = userManager;
//            this.roleManager = roleManager;
//        }

//        // Student: view own profile only
//        [HttpGet("me")]
//        [Authorize(Roles = "Student")]
//        public async Task<IActionResult> GetMyProfile()
//        {
//            var userEmail = User.Identity?.Name ?? User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;
//            if (string.IsNullOrEmpty(userEmail)) return Unauthorized();

//            var identityUser = await userManager.FindByEmailAsync(userEmail);
//            if (identityUser == null) return Unauthorized();

//            var student = await studentRepo.GetByIdentityUserIdAsync(identityUser.Id);
//            if (student == null) return NotFound();

//            var dto = new StudentDto
//            {
//                StudentID = student.StudentID,
//                FullName = student.FullName,
//                Age = student.Age,
//                Email = student.Email,
//                Course = student.Course,
//                EnrollmentDate = student.EnrollmentDate,
//                Marks = student.Marks,
//                AttendancePercentage = student.AttendancePercentage,
//                GuardianContact = student.GuardianContact,
//                ClassId = student.ClassId,
//                ClassName = student.Class?.Name,
//                ClassSection = student.Class?.Section
//            };

//            if (student.Class != null && !string.IsNullOrEmpty(student.Class.ClassTeacherUserId))
//            {
//                var teacher = await userManager.FindByIdAsync(student.Class.ClassTeacherUserId);
//                dto.ClassTeacherEmail = teacher?.Email;
//            }

//            return Ok(dto);
//        }

//        // IT & Principal: modify student
//        [HttpPut("{id:guid}")]
//        [Authorize(Roles = "IT,Principal")]
//        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateStudentRequestDto request)
//        {
//            var existing = await studentRepo.GetByIdAsync(id);
//            if (existing == null) return NotFound();

//            existing.FullName = request.FullName;
//            existing.Age = request.Age;
//            existing.Email = request.Email;
//            existing.Course = request.Course;
//            existing.EnrollmentDate = request.EnrollmentDate;
//            existing.Marks = request.Marks;
//            existing.AttendancePercentage = request.AttendancePercentage;
//            existing.GuardianContact = request.GuardianContact;
//            existing.ClassId = request.ClassId;

//            var updated = await studentRepo.UpdateAsync(id, existing);
//            if (updated == null) return NotFound();

//            return Ok(new { Message = "Student updated." });
//        }

//        // Teacher: limited update (email + marks)
//        [HttpPut("{id:guid}/teacher-update")]
//        [Authorize(Roles = "Teacher")]
//        public async Task<IActionResult> TeacherUpdate(Guid id, [FromBody] TeacherUpdateStudentDto request)
//        {
//            var teacherClassIdClaim = User.Claims.FirstOrDefault(c => c.Type == "TeacherClassId")?.Value;
//            if (teacherClassIdClaim == null) return Forbid();

//            var student = await studentRepo.GetByIdAsync(id);
//            if (student == null || student.ClassId?.ToString() != teacherClassIdClaim) return Forbid();

//            student.Email = request.Email;
//            student.Marks = request.Marks;

//            var updated = await studentRepo.UpdateAsync(id, student);
//            if (updated == null) return NotFound();

//            return Ok(new { Message = "Student email and marks updated by class teacher." });
//        }

//        // IT: create student with Identity account
//        [HttpPost("create-by-it")]
//        [Authorize(Roles = "IT")]
//        public async Task<IActionResult> CreateStudentByIT([FromBody] CreateStudentRequestDto request)
//        {
//            var tempPassword = $"Stu@{Guid.NewGuid().ToString("N").Substring(0, 8)}";
//            var identityUser = new IdentityUser
//            {
//                UserName = request.Email.Trim(),
//                Email = request.Email.Trim(),
//                EmailConfirmed = true
//            };

//            var identityResult = await userManager.CreateAsync(identityUser, tempPassword);
//            if (!identityResult.Succeeded)
//                return BadRequest(identityResult.Errors);

//            if (!await roleManager.RoleExistsAsync("Student"))
//                await roleManager.CreateAsync(new IdentityRole("Student"));

//            var roleResult = await userManager.AddToRoleAsync(identityUser, "Student");
//            if (!roleResult.Succeeded)
//                return BadRequest(roleResult.Errors);

//            var student = new Student
//            {
//                StudentID = Guid.NewGuid(),
//                FullName = request.FullName,
//                Age = request.Age,
//                Email = request.Email,
//                Course = request.Course,
//                EnrollmentDate = request.EnrollmentDate == default ? DateTime.Now : request.EnrollmentDate,
//                Marks = request.Marks,
//                AttendancePercentage = request.AttendancePercentage,
//                GuardianContact = request.GuardianContact,
//                ClassId = request.ClassId,
//                IdentityUserId = identityUser.Id
//            };

//            var created = await studentRepo.AddAsync(student);

//            return CreatedAtAction(nameof(GetMyProfile), new { id = created.StudentID }, new
//            {
//                created.StudentID,
//                IdentityUserId = identityUser.Id,
//                TempPassword = tempPassword
//            });
//        }

//        [HttpDelete("{id:guid}")]
//        [Authorize(Roles = "IT")]
//        public async Task<IActionResult> Delete(Guid id)
//        {
//            var deleted = await studentRepo.DeleteAsync(id);
//            if (deleted == null) return NotFound();
//            return Ok(new { Message = "Student deleted", deleted.StudentID });
//        }
//    }
//}

using ApiCallAdv.Models.Domain;
using ApiCallAdv.Models.DTO;
using ApiCallAdv.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApiCallAdv.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository studentRepo;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public StudentController(IStudentRepository studentRepo, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.studentRepo = studentRepo;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        // ✅ IT & Principal: view all students
        [HttpGet]
        [Authorize(Roles = "IT,Principal")]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await studentRepo.GetAllAsync();
            var result = new List<StudentDto>();

            foreach (var s in students)
            {
                var dto = new StudentDto
                {
                    StudentID = s.StudentID,
                    FullName = s.FullName,
                    Age = s.Age,
                    Email = s.Email,
                    Course = s.Course,
                    EnrollmentDate = s.EnrollmentDate,
                    Marks = s.Marks,
                    AttendancePercentage = s.AttendancePercentage,
                    GuardianContact = s.GuardianContact,
                    ClassId = s.ClassId,
                    ClassName = s.Class?.Name,
                    ClassSection = s.Class?.Section
                };

                if (s.Class != null && !string.IsNullOrEmpty(s.Class.ClassTeacherUserId))
                {
                    var teacher = await userManager.FindByIdAsync(s.Class.ClassTeacherUserId);
                    dto.ClassTeacherEmail = teacher?.Email;
                }

                result.Add(dto);
            }

            return Ok(result);
        }

        // Student: view own profile only
        [HttpGet("me")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userEmail = User.Identity?.Name ?? User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(userEmail)) return Unauthorized();

            var identityUser = await userManager.FindByEmailAsync(userEmail);
            if (identityUser == null) return Unauthorized();

            var student = await studentRepo.GetByIdentityUserIdAsync(identityUser.Id);
            if (student == null) return NotFound();

            var dto = new StudentDto
            {
                StudentID = student.StudentID,
                FullName = student.FullName,
                Age = student.Age,
                Email = student.Email,
                Course = student.Course,
                EnrollmentDate = student.EnrollmentDate,
                Marks = student.Marks,
                AttendancePercentage = student.AttendancePercentage,
                GuardianContact = student.GuardianContact,
                ClassId = student.ClassId,
                ClassName = student.Class?.Name,
                ClassSection = student.Class?.Section
            };

            if (student.Class != null && !string.IsNullOrEmpty(student.Class.ClassTeacherUserId))
            {
                var teacher = await userManager.FindByIdAsync(student.Class.ClassTeacherUserId);
                dto.ClassTeacherEmail = teacher?.Email;
            }

            return Ok(dto);
        }

        // IT & Principal: modify student
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "IT,Principal")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateStudentRequestDto request)
        {
            var existing = await studentRepo.GetByIdAsync(id);
            if (existing == null) return NotFound();

            existing.FullName = request.FullName;
            existing.Age = request.Age;
            existing.Email = request.Email;
            existing.Course = request.Course;
            existing.EnrollmentDate = request.EnrollmentDate;
            existing.Marks = request.Marks;
            existing.AttendancePercentage = request.AttendancePercentage;
            existing.GuardianContact = request.GuardianContact;
            existing.ClassId = request.ClassId;

            var updated = await studentRepo.UpdateAsync(id, existing);
            if (updated == null) return NotFound();

            return Ok(new { Message = "Student updated." });
        }

        // Teacher: limited update (email + marks)
        [HttpPut("{id:guid}/teacher-update")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> TeacherUpdate(Guid id, [FromBody] TeacherUpdateStudentDto request)
        {
            var teacherClassIdClaim = User.Claims.FirstOrDefault(c => c.Type == "TeacherClassId")?.Value;
            if (teacherClassIdClaim == null) return Forbid();

            var student = await studentRepo.GetByIdAsync(id);
            if (student == null || student.ClassId?.ToString() != teacherClassIdClaim) return Forbid();

            student.Email = request.Email;
            student.Marks = request.Marks;

            var updated = await studentRepo.UpdateAsync(id, student);
            if (updated == null) return NotFound();

            return Ok(new { Message = "Student email and marks updated by class teacher." });
        }

        // IT: create student with Identity account
        [HttpPost("create-by-it")]
        [Authorize(Roles = "IT")]
        public async Task<IActionResult> CreateStudentByIT([FromBody] CreateStudentRequestDto request)
        {
            var tempPassword = $"Stu@{Guid.NewGuid().ToString("N").Substring(0, 8)}";
            var identityUser = new IdentityUser
            {
                UserName = request.Email.Trim(),
                Email = request.Email.Trim(),
                EmailConfirmed = true
            };

            var identityResult = await userManager.CreateAsync(identityUser, tempPassword);
            if (!identityResult.Succeeded)
                return BadRequest(identityResult.Errors);

            if (!await roleManager.RoleExistsAsync("Student"))
                await roleManager.CreateAsync(new IdentityRole("Student"));

            var roleResult = await userManager.AddToRoleAsync(identityUser, "Student");
            if (!roleResult.Succeeded)
                return BadRequest(roleResult.Errors);

            var student = new Student
            {
                StudentID = Guid.NewGuid(),
                FullName = request.FullName,
                Age = request.Age,
                Email = request.Email,
                Course = request.Course,
                EnrollmentDate = request.EnrollmentDate == default ? DateTime.Now : request.EnrollmentDate,
                Marks = request.Marks,
                AttendancePercentage = request.AttendancePercentage,
                GuardianContact = request.GuardianContact,
                ClassId = request.ClassId,
                IdentityUserId = identityUser.Id
            };

            var created = await studentRepo.AddAsync(student);

            return CreatedAtAction(nameof(GetMyProfile), new { id = created.StudentID }, new
            {
                created.StudentID,
                IdentityUserId = identityUser.Id,
                TempPassword = tempPassword
            });
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "IT")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await studentRepo.DeleteAsync(id);
            if (deleted == null) return NotFound();
            return Ok(new { Message = "Student deleted", deleted.StudentID });
        }
    }
}
