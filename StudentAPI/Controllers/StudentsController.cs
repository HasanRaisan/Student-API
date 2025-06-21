using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles.Infrastructure;

using StudentAPI.Shared;
using StudentAPIBusiness;

namespace StudentAPI.Controllers
{
    [Route("api/Student")]
    [ApiController]
    public class StudentsController : ControllerBase
    {

        [HttpGet("All", Name = "GetAllStudents")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Models.StudentDTO>> GetStudents()
        {
            List<Models.StudentDTO> StudentsList = StudentAPIBusiness.Student.GetAllStudents();

            if (StudentsList.Count == 0)
            {
                return NotFound("No students found.");
            }
            return Ok(StudentsList);
        }

        [HttpGet("Passed", Name = "GetPassedStudetns")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Models.StudentDTO>> GetPassedStudents()
        {
            List<Models.StudentDTO> PassedstuentList = StudentAPIBusiness.Student.GetPassedStudents();
            if (PassedstuentList.Count == 0)
            {
                return NotFound("No students found.");
            }
            return Ok(PassedstuentList);
        }

        [HttpGet("GradesAverage", Name = "GetGradeAverage")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<double> GetGradeAverage()
        {
            double GradeAverage = StudentAPIBusiness.Student.GetGradeAverage();
            if (GradeAverage == 0)
            {
                return NotFound("No students found.");
            }
            return Ok(GradeAverage); 

        }


        [HttpGet("GetByID/{ID}", Name ="GetStudentsByID")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Models.StudentDTO> GetStudentByID(int ID)
        {
            if (ID <= 0)
                return BadRequest("Invalid Age provided.");

            var student = StudentAPIBusiness.Student.Find(ID);

            if (student == null)
                return NotFound($"Student with Age {ID} not found.");
            

            //we return the DTO not the student object.
            // cuze it's stateless and for securety as well
            return Ok(student.StudentDTO);
        }


        [HttpPost("Add", Name = "AddStudent")]
        //[HttpPost(Name = "AddStudent")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Models.StudentDTO> AddStudent([FromBody] Models.StudentDTO newStudent)
        //public ActionResult<Student> AddStudent(Student newStudent)
        {
            if (newStudent == null || newStudent.Age <= 0 || newStudent.Grade < 0 || string.IsNullOrEmpty(newStudent.Name))
            {
                return BadRequest("Invalid data provided.");
            }

            StudentAPIBusiness.Student Student = new StudentAPIBusiness.Student(new Models.StudentDTO(newStudent.ID, newStudent.Name, newStudent.Age, newStudent.Grade));
            Student.Save();
            newStudent.ID = Student.StudentDTO.ID;
            return CreatedAtRoute("GetStudentsByID", new { ID = newStudent.ID }, newStudent);
        }

        [HttpDelete("Delete/{StudentID}", Name = "DeleteStudent")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteStudent(int StudentID)
        {
            if (StudentID < 1)
                return BadRequest("Invalid ID");

            var student = StudentAPIBusiness.Student.Find(StudentID);
            if (student == null)
                return NotFound($"Student with ID {StudentID} not found!");

            if (!student.Delete())
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while deleting the student." });

            return Ok($"Student with ID {StudentID} has been deleted successfully!");
        }



        [HttpPut("Update/{StudentID}", Name = "UpdateStudent")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Models.StudentDTO> UpdateStudent(int StudentID, Models.StudentDTO UpdatedStudent)
        {
            if (UpdatedStudent == null || StudentID < 1 || UpdatedStudent.Age < 1 || 
                string.IsNullOrEmpty(UpdatedStudent.Name) || UpdatedStudent.Grade < 0)
            {
                return BadRequest("Invalid data provided.");
            }

            var student = StudentAPIBusiness.Student.Find(StudentID);

            if (student == null)
            {
                return NotFound($"Student with Age {StudentID} not found.");
            }

            student.Name = UpdatedStudent.Name;
            student.Age = UpdatedStudent.Age;
            student.Grade = UpdatedStudent.Grade;
            if(!student.Save())
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while updating the student." });
            }

            //we return the DTO not the full student object.
            return Ok(student.StudentDTO);
        }

    }
}
