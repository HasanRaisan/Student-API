using StudentAPI.Shared;
using StudentAPIDataAccess;
using System.ComponentModel;

namespace StudentAPIBusiness
{
    public class Student
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;

        public string Name { get; set; } = "";
        public int Age { get; set; } = 0;
        public int Grade { get; set; } = 0;
        public int ID { get; set; } = 0;

        public Models.StudentDTO StudentDTO
        {
            get
            {
                return new Models.StudentDTO(ID, Name, Age, Grade);
            }
        }


        public Student(Models.StudentDTO studentDTO, enMode mode = enMode.AddNew)
        {
            Mode = mode;
            ID = studentDTO.ID;
            Name = studentDTO.Name;
            Age = studentDTO.Age;
            Grade = studentDTO.Grade;
        }


        static public List<Models.StudentDTO> GetAllStudents() => StudentData.GetAllStudents();

        static public List<Models.StudentDTO> GetPassedStudents() =>  StudentData.GetPassedStudents();
        

        static public double GetGradeAverage() => StudentData.GetGradeAverage();
        

        static public Student? Find(int StudnetID)
        {
            Models.StudentDTO? studentDTO = StudentData.GetStudentByID(StudnetID);
            if (studentDTO != null)
            {
                return new Student(studentDTO, enMode.Update);
            }
            else
            {
                return null;
            }
        }
        private bool _AddNewStudent()
        {
            this.ID = StudentData.AddStudent(this.StudentDTO);
            return (this.ID != 0);
        }

        private bool _UpdateStudent()
        {
            return StudentData.UpdateStudent(this.StudentDTO);
        }

        public bool Delete() =>  StudentData.DeleteStudent(this.ID);
        

        public bool Delete(int StudentId)
        {
            if (StudentId <= 0)
                throw new ArgumentException("Invalid student ID provided.");

            return StudentData.DeleteStudent(StudentId);
        }

        public bool Save()
        {
            if (Mode == enMode.AddNew)
            {
                if (string.IsNullOrEmpty(Name) || Age <= 0 || Grade < 0)
                    throw new ArgumentException("Invalid student data provided.");

                if (_AddNewStudent())
                {
                    this.Mode = enMode.Update;
                    return true;
                }
                else
                    return false;

            }
            else
            {
                return _UpdateStudent();
            }


        }
    }
}
