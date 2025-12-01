////using ApiCallAdv.Models.Domain;
////using ApiCallAdv.Models.DTO;
////using ApiCallAdv.Repositories.Interface;
////using Microsoft.AspNetCore.Authorization;
////using Microsoft.AspNetCore.Identity;
////using Microsoft.AspNetCore.Mvc;

////namespace ApiCallAdv.Controllers
////{
////    [ApiController]
////    [Route("api/[controller]")]
////    public class ClassController : ControllerBase
////    {
////        private readonly IClassRepository classRepo;
////        private readonly UserManager<IdentityUser> userManager;

////        public ClassController(IClassRepository classRepo, UserManager<IdentityUser> userManager)
////        {
////            this.classRepo = classRepo;
////            this.userManager = userManager;
////        }

////        // View classes — Teacher, Principal, IT
////        [HttpGet]
////        [Authorize(Roles = "Teacher,Principal,IT")]
////        public async Task<IActionResult> GetAll()
////        {
////            var classes = await classRepo.GetAllAsync();
////            var dtos = new List<ClassDto>();
////            foreach (var c in classes)
////            {
////                var teacher = string.IsNullOrEmpty(c.ClassTeacherUserId) ? null : await userManager.FindByIdAsync(c.ClassTeacherUserId);
////                dtos.Add(new ClassDto
////                {
////                    ClassId = c.ClassId,
////                    Name = c.Name,
////                    Section = c.Section,
////                    ClassTeacherUserId = c.ClassTeacherUserId,
////                    ClassTeacherEmail = teacher?.Email,
////                    StudentCount = c.Students.Count
////                });
////            }
////            return Ok(dtos);
////        }

////        // IT: create class
////        [HttpPost]
////        [Authorize(Roles = "IT")]
////        public async Task<IActionResult> Create([FromBody] CreateClassRequestDto request)
////        {
////            var cls = new Class
////            {
////                ClassId = Guid.NewGuid(),
////                Name = request.Name,
////                Section = request.Section,
////                ClassTeacherUserId = request.ClassTeacherUserId
////            };
////            var created = await classRepo.AddAsync(cls);
////            return CreatedAtAction(nameof(GetAll), new { id = created.ClassId }, new { created.ClassId });
////        }

////        // Principal & IT: update class (including mapping)
////        [HttpPut("{id:guid}")]
////        [Authorize(Roles = "Principal,IT")]
////        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateClassRequestDto request)
////        {
////            var cls = new Class
////            {
////                Name = request.Name,
////                Section = request.Section,
////                ClassTeacherUserId = request.ClassTeacherUserId
////            };
////            var updated = await classRepo.UpdateAsync(id, cls);
////            if (updated == null) return NotFound();
////            return Ok(new { Message = "Class updated." });
////        }

////        // Principal & IT: assign class teacher explicitly
////        [HttpPost("assign-teacher")]
////        [Authorize(Roles = "Principal,IT")]
////        public async Task<IActionResult> AssignClassTeacher([FromBody] AssignClassTeacherDto request)
////        {
////            var teacher = await userManager.FindByIdAsync(request.TeacherUserId);
////            if (teacher == null) return NotFound(new { Message = "Teacher user not found." });

////            var roles = await userManager.GetRolesAsync(teacher);
////            if (!roles.Contains("Teacher")) return BadRequest(new { Message = "User is not a Teacher." });

////            var ok = await classRepo.AssignClassTeacherAsync(request.ClassId, request.TeacherUserId);
////            if (!ok) return NotFound(new { Message = "Class not found." });

////            return Ok(new { Message = "Class teacher assigned." });
////        }

////        // IT: delete class
////        [HttpDelete("{id:guid}")]
////        [Authorize(Roles = "IT")]
////        public async Task<IActionResult> Delete(Guid id)
////        {
////            var deleted = await classRepo.DeleteAsync(id);
////            if (deleted == null) return NotFound();
////            return Ok(new { Message = "Class deleted", deleted.ClassId });
////        }
////    }
////}


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
//    public class ClassController : ControllerBase
//    {
//        private readonly IClassRepository classRepo;
//        private readonly UserManager<IdentityUser> userManager;

//        public ClassController(IClassRepository classRepo, UserManager<IdentityUser> userManager)
//        {
//            this.classRepo = classRepo;
//            this.userManager = userManager;
//        }

//        // View classes — Teacher, Principal, IT
//        [HttpGet]
//        [Authorize(Roles = "Teacher,Principal,IT")]
//        public async Task<IActionResult> GetAll()
//        {
//            var classes = await classRepo.GetAllAsync();

//            // Teachers should only see their own class
//            if (User.IsInRole("Teacher"))
//            {
//                var teacherClassIdClaim = User.Claims.FirstOrDefault(c => c.Type == "TeacherClassId")?.Value;
//                if (!string.IsNullOrEmpty(teacherClassIdClaim))
//                {
//                    classes = classes.Where(c => c.ClassId.ToString() == teacherClassIdClaim);
//                }
//            }

//            var dtos = new List<ClassDto>();
//            foreach (var c in classes)
//            {
//                var teacher = string.IsNullOrEmpty(c.ClassTeacherUserId) ? null : await userManager.FindByIdAsync(c.ClassTeacherUserId);
//                dtos.Add(new ClassDto
//                {
//                    ClassId = c.ClassId,
//                    Name = c.Name,
//                    Section = c.Section,
//                    ClassTeacherUserId = c.ClassTeacherUserId,
//                    ClassTeacherEmail = teacher?.Email,
//                    StudentCount = c.Students.Count
//                });
//            }
//            return Ok(dtos);
//        }

//        // Teacher: view detailed student list for own class
//        [HttpGet("my-class/students")]
//        [Authorize(Roles = "Teacher")]
//        public async Task<IActionResult> GetMyClassStudents()
//        {
//            var teacherClassIdClaim = User.Claims.FirstOrDefault(c => c.Type == "TeacherClassId")?.Value;
//            if (string.IsNullOrEmpty(teacherClassIdClaim)) return Forbid();

//            var cls = await classRepo.GetByIdAsync(Guid.Parse(teacherClassIdClaim));
//            if (cls == null) return NotFound();

//            var students = cls.Students.Select(s => new
//            {
//                s.StudentID,
//                s.FullName,
//                s.Email,
//                s.Marks,
//                s.AttendancePercentage
//            }).ToList();

//            return Ok(new
//            {
//                ClassId = cls.ClassId,
//                cls.Name,
//                cls.Section,
//                StudentCount = students.Count,
//                Students = students
//            });
//        }

//        // IT: create class
//        [HttpPost]
//        [Authorize(Roles = "IT")]
//        public async Task<IActionResult> Create([FromBody] CreateClassRequestDto request)
//        {
//            var cls = new Class
//            {
//                ClassId = Guid.NewGuid(),
//                Name = request.Name,
//                Section = request.Section,
//                ClassTeacherUserId = request.ClassTeacherUserId
//            };
//            var created = await classRepo.AddAsync(cls);
//            return CreatedAtAction(nameof(GetAll), new { id = created.ClassId }, new { created.ClassId });
//        }

//        // Principal & IT: update class (including mapping)
//        [HttpPut("{id:guid}")]
//        [Authorize(Roles = "Principal,IT")]
//        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateClassRequestDto request)
//        {
//            var cls = new Class
//            {
//                Name = request.Name,
//                Section = request.Section,
//                ClassTeacherUserId = request.ClassTeacherUserId
//            };
//            var updated = await classRepo.UpdateAsync(id, cls);
//            if (updated == null) return NotFound();
//            return Ok(new { Message = "Class updated." });
//        }

//        // Principal & IT: assign class teacher explicitly
//        [HttpPost("assign-teacher")]
//        [Authorize(Roles = "Principal,IT")]
//        public async Task<IActionResult> AssignClassTeacher([FromBody] AssignClassTeacherDto request)
//        {
//            var teacher = await userManager.FindByIdAsync(request.TeacherUserId);
//            if (teacher == null) return NotFound(new { Message = "Teacher user not found." });

//            var roles = await userManager.GetRolesAsync(teacher);
//            if (!roles.Contains("Teacher")) return BadRequest(new { Message = "User is not a Teacher." });

//            var ok = await classRepo.AssignClassTeacherAsync(request.ClassId, request.TeacherUserId);
//            if (!ok) return NotFound(new { Message = "Class not found." });

//            //return Ok(new { Message = "Class teacher assigned." });
//            return Ok(new
//            {
//                Message = "Class teacher assigned.",
//                ClassId = request.ClassId,
//                TeacherUserId = request.TeacherUserId,
//                TeacherEmail = teacher.Email
//            });
//        }

//        // IT: delete class
//        [HttpDelete("{id:guid}")]
//        [Authorize(Roles = "IT")]
//        public async Task<IActionResult> Delete(Guid id)
//        {
//            var deleted = await classRepo.DeleteAsync(id);
//            if (deleted == null) return NotFound();
//            return Ok(new { Message = "Class deleted", deleted.ClassId });
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
    public class ClassController : ControllerBase
    {
        private readonly IClassRepository classRepo;
        private readonly UserManager<IdentityUser> userManager;

        public ClassController(IClassRepository classRepo, UserManager<IdentityUser> userManager)
        {
            this.classRepo = classRepo;
            this.userManager = userManager;
        }

        // View classes — Teacher, Principal, IT
        [HttpGet]
        [Authorize(Roles = "Teacher,Principal,IT")]
        public async Task<IActionResult> GetAll()
        {
            var classes = await classRepo.GetAllAsync();

            // Teachers should only see their own class
            if (User.IsInRole("Teacher"))
            {
                var teacherClassIdClaim = User.Claims.FirstOrDefault(c => c.Type == "TeacherClassId")?.Value;
                if (!string.IsNullOrEmpty(teacherClassIdClaim))
                {
                    classes = classes.Where(c => c.ClassId.ToString() == teacherClassIdClaim);
                }
            }

            var dtos = new List<ClassDto>();
            foreach (var c in classes)
            {
                var teacher = string.IsNullOrEmpty(c.ClassTeacherUserId) ? null : await userManager.FindByIdAsync(c.ClassTeacherUserId);
                dtos.Add(new ClassDto
                {
                    ClassId = c.ClassId,
                    Name = c.Name,
                    Section = c.Section,
                    ClassTeacherUserId = c.ClassTeacherUserId,
                    ClassTeacherEmail = teacher?.Email,
                    StudentCount = c.Students.Count
                });
            }
            return Ok(dtos);
        }

        // Optional: Get class by id (for edit prefill)
        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Teacher,Principal,IT")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var cls = await classRepo.GetByIdAsync(id);
            if (cls == null) return NotFound();

            var teacher = string.IsNullOrEmpty(cls.ClassTeacherUserId) ? null : await userManager.FindByIdAsync(cls.ClassTeacherUserId);

            return Ok(new ClassDto
            {
                ClassId = cls.ClassId,
                Name = cls.Name,
                Section = cls.Section,
                ClassTeacherUserId = cls.ClassTeacherUserId,
                ClassTeacherEmail = teacher?.Email,
                StudentCount = cls.Students.Count
            });
        }

        // Teacher: view detailed student list for own class
        [HttpGet("my-class/students")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> GetMyClassStudents()
        {
            var teacherClassIdClaim = User.Claims.FirstOrDefault(c => c.Type == "TeacherClassId")?.Value;
            if (string.IsNullOrEmpty(teacherClassIdClaim)) return Forbid();

            var cls = await classRepo.GetByIdAsync(Guid.Parse(teacherClassIdClaim));
            if (cls == null) return NotFound();

            var students = cls.Students.Select(s => new
            {
                s.StudentID,
                s.FullName,
                s.Email,
                s.Marks,
                s.AttendancePercentage
            }).ToList();

            return Ok(new
            {
                ClassId = cls.ClassId,
                cls.Name,
                cls.Section,
                StudentCount = students.Count,
                Students = students
            });
        }

        // Principal & IT: get students by class id
        //[HttpGet("{id:guid}/students")]
        //[Authorize(Roles = "Principal,IT")]
        //public async Task<IActionResult> GetStudentsByClassId(Guid id)
        //{
        //    var cls = await classRepo.GetByIdAsync(id);
        //    if (cls == null) return NotFound(new { Message = "Class not found." });

        //    var students = cls.Students.Select(s => new
        //    {
        //        s.StudentID,
        //        s.FullName,
        //        s.Email,
        //        s.Marks,
        //        s.AttendancePercentage
        //    }).ToList();

        //    return Ok(new
        //    {
        //        ClassId = cls.ClassId,
        //        cls.Name,
        //        cls.Section,
        //        StudentCount = students.Count,
        //        Students = students
        //    });
        //}

        [HttpGet("{id:guid}/students")]
        [Authorize(Roles = "Principal,IT")]
        public async Task<IActionResult> GetStudentsByClassId(Guid id)
        {
            var cls = await classRepo.GetByIdAsync(id);
            if (cls == null) return NotFound(new { Message = "Class not found." });

            var students = cls.Students.Select(s => new
            {
                s.StudentID,
                s.FullName,
                s.Age,
                s.Email,
                s.Course,
                s.EnrollmentDate,
                s.Marks,
                s.AttendancePercentage,
                s.GuardianContact,
                s.ClassId
            }).ToList();

            return Ok(new
            {
                ClassId = cls.ClassId,
                cls.Name,
                cls.Section,
                StudentCount = students.Count,
                Students = students
            });
        }


        // IT: create class
        [HttpPost]
        [Authorize(Roles = "IT")]
        public async Task<IActionResult> Create([FromBody] CreateClassRequestDto request)
        {
            var cls = new Class
            {
                ClassId = Guid.NewGuid(),
                Name = request.Name,
                Section = request.Section,
                ClassTeacherUserId = request.ClassTeacherUserId
            };
            var created = await classRepo.AddAsync(cls);
            return CreatedAtAction(nameof(GetAll), new { id = created.ClassId }, new { created.ClassId });
        }

        // Principal & IT: update class (including mapping)
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Principal,IT")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateClassRequestDto request)
        {
            var cls = new Class
            {
                Name = request.Name,
                Section = request.Section,
                ClassTeacherUserId = request.ClassTeacherUserId
            };
            var updated = await classRepo.UpdateAsync(id, cls);
            if (updated == null) return NotFound();
            return Ok(new { Message = "Class updated." });
        }

        // Principal & IT: assign class teacher explicitly
        [HttpPost("assign-teacher")]
        [Authorize(Roles = "Principal,IT")]
        public async Task<IActionResult> AssignClassTeacher([FromBody] AssignClassTeacherDto request)
        {
            var teacher = await userManager.FindByIdAsync(request.TeacherUserId);
            if (teacher == null) return NotFound(new { Message = "Teacher user not found." });

            var roles = await userManager.GetRolesAsync(teacher);
            if (!roles.Contains("Teacher")) return BadRequest(new { Message = "User is not a Teacher." });

            var ok = await classRepo.AssignClassTeacherAsync(request.ClassId, request.TeacherUserId);
            if (!ok) return NotFound(new { Message = "Class not found." });

            return Ok(new
            {
                Message = "Class teacher assigned.",
                ClassId = request.ClassId,
                TeacherUserId = request.TeacherUserId,
                TeacherEmail = teacher.Email
            });
        }

        // IT: delete class
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "IT")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await classRepo.DeleteAsync(id);
            if (deleted == null) return NotFound();
            return Ok(new { Message = "Class deleted", deleted.ClassId });
        }
    }
}
