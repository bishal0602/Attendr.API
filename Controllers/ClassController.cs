using Attendr.API.Helpers;
using Attendr.API.Models;
using Attendr.API.Models.Class;
using Attendr.API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Attendr.API.Controllers
{
    [Route("api/classes")]
    [ApiController]
    [Authorize]
    public class ClassController : ControllerBase
    {
        private readonly IClassRepository _classRepository;
        private readonly IClassStudentHelper _classStudentHelper;
        private readonly IMapper _mapper;

        public ClassController(IClassRepository classRepository, IClassStudentHelper classStudentHelper, IMapper mapper)
        {
            _classRepository = classRepository ?? throw new ArgumentNullException(nameof(classRepository));
            _classStudentHelper = classStudentHelper ?? throw new ArgumentNullException(nameof(classStudentHelper));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassDto>>> GetClasses()
        {
            var classesFromDb = await _classRepository.GetClassesAsync();
            var classToReturn = _mapper.Map<List<ClassDto>>(classesFromDb);
            return Ok(classToReturn);
        }

        [HttpGet("{classId}", Name = "GetClassById")]
        public async Task<IActionResult> GetClassById([FromRoute] Guid classId, [FromQuery] bool includeStudents = false)
        {
            var classFromDb = await _classRepository.GetClassByIdAsync(classId);

            if (classFromDb is null)
                return NotFound(new ErrorDetails(StatusCodes.Status404NotFound, $"Class with id {classId} does not exist!"));

            if (includeStudents)
            {
                return Ok(_mapper.Map<ClassWithStudentsDto>(classFromDb));
            }
            return Ok(_mapper.Map<ClassDto>(classFromDb));
        }


        //[Authorize("admin")]
        [HttpPost]
        public async Task<IActionResult> AddClass([FromBody] ClassCreationDto Class)
        {
            var classAlreadyExists = await _classRepository.ExistsClassAsync(Class.Year, Class.Department, Class.Group);
            if (classAlreadyExists)
            {
                return BadRequest(new ErrorDetails(StatusCodes.Status400BadRequest, "Class Already Exists!"));
            }
            var classToAdd = _mapper.Map<Entities.Class>(Class);

            await _classRepository.AddClassWithStudentsAsync(classToAdd);
            await _classRepository.SaveAsync();

            return CreatedAtRoute("GetClassById", new
            {
                classId = classToAdd.Id,
            }, new { result = "Class successfully created!", details = "Check Loaction header to navigate!" }); // TODO : Standardize creation response
        }

        [HttpGet("myclass")]
        public async Task<IActionResult> GetLoggedInUsersClass([FromQuery] bool includeStudents = true)
        {
            var userClaims = ((ClaimsIdentity)User.Identity!).Claims;
            var userEmail = userClaims.FirstOrDefault(c => c.Type == "email")?.Value;
            if (userEmail == null)
            {
                throw new Exception("No email provided in claims");
            }
            (string studentYear, string studentDepartment, string studentGroup) = _classStudentHelper.GetStudentsClassDetailsFromEmail(userEmail);

            var classFromDb = await _classRepository.GetClassByYearDepartGroupAsync(studentYear, studentDepartment, studentGroup);

            if (classFromDb is null)
                return NotFound(new ErrorDetails(StatusCodes.Status404NotFound, "User is not assigned to class"));

            if (includeStudents)
            {
                return Ok(_mapper.Map<ClassWithStudentsDto>(classFromDb));
            }
            return Ok(_mapper.Map<ClassDto>(classFromDb));
        }

    }
}
