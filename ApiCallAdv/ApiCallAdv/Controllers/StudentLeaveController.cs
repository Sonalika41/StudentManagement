////using ApiCallAdv.Models.Domain;
////using ApiCallAdv.Models.DTO;
////using ApiCallAdv.Repositories.Interface;
////using Microsoft.AspNetCore.Authorization;
////using Microsoft.AspNetCore.Mvc;

////namespace ApiCallAdv.Controllers
////{
////    [ApiController]
////    [Route("api/[controller]")]
////    public class StudentLeaveController : ControllerBase
////    {
////        private readonly IStudentLeaveRepository leaveRepo;

////        public StudentLeaveController(IStudentLeaveRepository leaveRepo) => this.leaveRepo = leaveRepo;

////        // Student applies for leave: only for self (uses StudentID claim from JWT)
////        [HttpPost("apply")]
////        [Authorize(Roles = "Student")]
////        public async Task<IActionResult> CreateLeave([FromBody] CreateLeaveRequestDto request)
////        {
////            var studentIdStr = User.Claims.FirstOrDefault(c => c.Type == "StudentID")?.Value;
////            if (string.IsNullOrEmpty(studentIdStr)) return Unauthorized();

////            var leave = new StudentLeaveRequest
////            {
////                StudentId = Guid.Parse(studentIdStr),
////                FromDate = request.FromDate,
////                ToDate = request.ToDate,
////                Reason = request.Reason,
////                Status = "Pending"
////            };

////            var created = await leaveRepo.CreateAsync(leave);
////            return Ok(new StudentLeaveRequestDto
////            {
////                LeaveId = created.LeaveId,
////                StudentId = created.StudentId,
////                FromDate = created.FromDate,
////                ToDate = created.ToDate,
////                Reason = created.Reason,
////                Status = created.Status
////            });
////        }

////        // Student views own leave requests
////        [HttpGet("mine")]
////        [Authorize(Roles = "Student")]
////        public async Task<IActionResult> GetMyLeaves()
////        {
////            var studentIdStr = User.Claims.FirstOrDefault(c => c.Type == "StudentID")?.Value;
////            if (string.IsNullOrEmpty(studentIdStr)) return Unauthorized();
////            var items = await leaveRepo.GetByStudentAsync(Guid.Parse(studentIdStr));
////            var dtos = items.Select(l => new StudentLeaveRequestDto
////            {
////                LeaveId = l.LeaveId,
////                StudentId = l.StudentId,
////                FromDate = l.FromDate,
////                ToDate = l.ToDate,
////                Reason = l.Reason,
////                Status = l.Status
////            });
////            return Ok(dtos);
////        }

////        // Teacher/Principal/IT can approve or reject
////        [HttpPut("status")]
////        [Authorize(Roles = "Teacher,Principal,IT")]
////        public async Task<IActionResult> UpdateStatus([FromBody] UpdateLeaveStatusDto request)
////        {
////            var updated = await leaveRepo.UpdateStatusAsync(request.LeaveId, request.Status);
////            if (updated is null) return NotFound();
////            return Ok(new StudentLeaveRequestDto
////            {
////                LeaveId = updated.LeaveId,
////                StudentId = updated.StudentId,
////                FromDate = updated.FromDate,
////                ToDate = updated.ToDate,
////                Reason = updated.Reason,
////                Status = updated.Status
////            });
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
//    public class StudentLeaveController : ControllerBase
//    {
//        private readonly IStudentLeaveRepository leaveRepo;

//        public StudentLeaveController(IStudentLeaveRepository leaveRepo) => this.leaveRepo = leaveRepo;

//        // Student applies for leave: only for self (uses StudentID claim from JWT)
//        [HttpPost("apply")]
//        [Authorize(Roles = "Student")]
//        public async Task<IActionResult> CreateLeave([FromBody] CreateLeaveRequestDto request)
//        {
//            var studentIdStr = User.Claims.FirstOrDefault(c => c.Type == "StudentID")?.Value;
//            if (string.IsNullOrEmpty(studentIdStr)) return Unauthorized();

//            var leave = new StudentLeaveRequest
//            {
//                StudentId = Guid.Parse(studentIdStr),
//                FromDate = request.FromDate,
//                ToDate = request.ToDate,
//                Reason = request.Reason,
//                Status = "Pending"
//            };

//            var created = await leaveRepo.CreateAsync(leave);
//            return Ok(new StudentLeaveRequestDto
//            {
//                LeaveId = created.LeaveId,
//                StudentId = created.StudentId,
//                FromDate = created.FromDate,
//                ToDate = created.ToDate,
//                Reason = created.Reason,
//                Status = created.Status
//            });
//        }

//        // Student views own leave requests
//        [HttpGet("mine")]
//        [Authorize(Roles = "Student")]
//        public async Task<IActionResult> GetMyLeaves()
//        {
//            var studentIdStr = User.Claims.FirstOrDefault(c => c.Type == "StudentID")?.Value;
//            if (string.IsNullOrEmpty(studentIdStr)) return Unauthorized();

//            var items = await leaveRepo.GetByStudentAsync(Guid.Parse(studentIdStr));
//            var dtos = items.Select(l => new StudentLeaveRequestDto
//            {
//                LeaveId = l.LeaveId,
//                StudentId = l.StudentId,
//                FromDate = l.FromDate,
//                ToDate = l.ToDate,
//                Reason = l.Reason,
//                Status = l.Status
//            });
//            return Ok(dtos);
//        }

//        // Teacher/Principal/IT can approve or reject
//        [HttpPut("status")]
//        [Authorize(Roles = "Teacher,Principal,IT")]
//        public async Task<IActionResult> UpdateStatus([FromBody] UpdateLeaveStatusDto request)
//        {
//            var leave = await leaveRepo.GetByIdAsync(request.LeaveId);
//            if (leave is null) return NotFound();

//            // Restrict Teacher to their own class students only
//            if (User.IsInRole("Teacher"))
//            {
//                var teacherClassIdClaim = User.Claims.FirstOrDefault(c => c.Type == "TeacherClassId")?.Value;
//                if (string.IsNullOrEmpty(teacherClassIdClaim)) return Forbid();

//                if (leave.Student?.ClassId?.ToString() != teacherClassIdClaim)
//                    return Forbid();
//            }

//            var updated = await leaveRepo.UpdateStatusAsync(request.LeaveId, request.Status);
//            if (updated is null) return NotFound();

//            return Ok(new StudentLeaveRequestDto
//            {
//                LeaveId = updated.LeaveId,
//                StudentId = updated.StudentId,
//                FromDate = updated.FromDate,
//                ToDate = updated.ToDate,
//                Reason = updated.Reason,
//                Status = updated.Status
//            });
//        }
//    }
//}



using ApiCallAdv.Models.Domain;
using ApiCallAdv.Models.DTO;
using ApiCallAdv.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiCallAdv.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentLeaveController : ControllerBase
    {
        private readonly IStudentLeaveRepository leaveRepo;

        public StudentLeaveController(IStudentLeaveRepository leaveRepo) => this.leaveRepo = leaveRepo;

        // Student applies for leave: only for self (uses StudentID claim from JWT)
        [HttpPost("apply")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> CreateLeave([FromBody] CreateLeaveRequestDto request)
        {
            var studentIdStr = User.Claims.FirstOrDefault(c => c.Type == "StudentID")?.Value;
            if (string.IsNullOrEmpty(studentIdStr)) return Unauthorized();

            var leave = new StudentLeaveRequest
            {
                StudentId = Guid.Parse(studentIdStr),
                FromDate = request.FromDate,
                ToDate = request.ToDate,
                Reason = request.Reason,
                Status = "Pending"
            };

            var created = await leaveRepo.CreateAsync(leave);
            return Ok(new StudentLeaveRequestDto
            {
                LeaveId = created.LeaveId,
                StudentId = created.StudentId,
                FromDate = created.FromDate,
                ToDate = created.ToDate,
                Reason = created.Reason,
                Status = created.Status
            });
        }

        // Student views own leave requests
        [HttpGet("mine")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetMyLeaves()
        {
            var studentIdStr = User.Claims.FirstOrDefault(c => c.Type == "StudentID")?.Value;
            if (string.IsNullOrEmpty(studentIdStr)) return Unauthorized();

            var items = await leaveRepo.GetByStudentAsync(Guid.Parse(studentIdStr));
            var dtos = items.Select(l => new StudentLeaveRequestDto
            {
                LeaveId = l.LeaveId,
                StudentId = l.StudentId,
                FromDate = l.FromDate,
                ToDate = l.ToDate,
                Reason = l.Reason,
                Status = l.Status
            });
            return Ok(dtos);
        }

        // Teacher/Principal/IT can approve or reject
        [HttpPut("status")]
        [Authorize(Roles = "Teacher,Principal,IT")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateLeaveStatusDto request)
        {
            var leave = await leaveRepo.GetByIdAsync(request.LeaveId);
            if (leave is null) return NotFound();

            // Restrict Teacher to their own class students only
            if (User.IsInRole("Teacher"))
            {
                var teacherClassIdClaim = User.Claims.FirstOrDefault(c => c.Type == "TeacherClassId")?.Value;
                if (string.IsNullOrEmpty(teacherClassIdClaim)) return Forbid();

                if (leave.Student?.ClassId?.ToString() != teacherClassIdClaim)
                    return Forbid();
            }

            var updated = await leaveRepo.UpdateStatusAsync(request.LeaveId, request.Status);
            if (updated is null) return NotFound();

            return Ok(new StudentLeaveRequestDto
            {
                LeaveId = updated.LeaveId,
                StudentId = updated.StudentId,
                FromDate = updated.FromDate,
                ToDate = updated.ToDate,
                Reason = updated.Reason,
                Status = updated.Status
            });
        }

        // Teacher view: all leave requests for their class students
        [HttpGet("my-class")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> GetLeavesForMyClass()
        {
            var teacherClassIdClaim = User.Claims.FirstOrDefault(c => c.Type == "TeacherClassId")?.Value;
            if (string.IsNullOrEmpty(teacherClassIdClaim)) return Forbid();

            var items = await leaveRepo.GetByClassIdAsync(Guid.Parse(teacherClassIdClaim));
            var dtos = items.Select(l => new StudentLeaveRequestDto
            {
                LeaveId = l.LeaveId,
                StudentId = l.StudentId,
                StudentName = l.Student?.FullName,
                FromDate = l.FromDate,
                ToDate = l.ToDate,
                Reason = l.Reason,
                Status = l.Status
            });
            return Ok(dtos);
        }
    }
}
