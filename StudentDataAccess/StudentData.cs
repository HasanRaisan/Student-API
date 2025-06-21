using StudentAPI.Shared;
using Microsoft.Data.SqlClient;
using System.Data;

namespace StudentAPIDataAccess
{
    public static class StudentData
    {
        private static string _connectionString = "Server=localhost;Database=StudentsDB;User Id=sa;Password=sa123456;Encrypt=False;TrustServerCertificate=True;Connection Timeout=30;";

        public static List<Models.StudentDTO> GetAllStudents()
        {
            List<Models.StudentDTO> StudentsList = new List<Models.StudentDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = new SqlCommand("GetAllStudents", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            StudentsList.Add(new Models.StudentDTO(
                                reader.GetInt32(reader.GetOrdinal("Id")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetInt32(reader.GetOrdinal("Age")),
                                reader.GetInt32(reader.GetOrdinal("Grade"))
                                ));
                        }
                    }
                }
            }
            catch
            {
                // handle exceptions (e.g., log them)
            }
            return StudentsList;

        }

        public static List<Models.StudentDTO> GetPassedStudents()
        {
            List<Models.StudentDTO> passedStudentsList = new List<Models.StudentDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = new SqlCommand("GetPassedStudents", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            passedStudentsList.Add(new Models.StudentDTO(
                                reader.GetInt32(reader.GetOrdinal("Id")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetInt32(reader.GetOrdinal("Age")),
                                reader.GetInt32(reader.GetOrdinal("Grade"))
                                ));
                        }
                    }

                }
            }
            catch
            {
                // handle exceptions (e.g., log them)
            }
            return passedStudentsList;

        }

        public static double GetGradeAverage()
        {
            double average = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = new SqlCommand("GetAverageGrade", connection))
                { 
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        average = Convert.ToDouble(result);
                    }
                }
            }
            catch
            {
                // handle exceptions (e.g., log them)
            }
            return average;
        }

        public static Models.StudentDTO? GetStudentByID(int StudentId)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = new SqlCommand("GetStudentById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@StudentId", @StudentId);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Models.StudentDTO(
                                reader.GetInt32(reader.GetOrdinal("Id")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetInt32(reader.GetOrdinal("Age")),
                                reader.GetInt32(reader.GetOrdinal("Grade"))
                            );
                        }
                        else
                            return null;
                    }
                }
            }
            catch
            {
                // handle exceptions (e.g., log them)
            }

            return null;
        }

        public static int AddStudent(Models.StudentDTO studentDTO)
        {
            int newStudentId = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = new SqlCommand("AddStudent", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Name", studentDTO.Name);
                    command.Parameters.AddWithValue("@Age", studentDTO.Age);
                    command.Parameters.AddWithValue("@Grade", studentDTO.Grade);
                    SqlParameter outPutStudentID = new SqlParameter("@NewStudentId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    command.Parameters.Add(outPutStudentID);
                    connection.Open();
                    command.ExecuteNonQuery();

                    if (outPutStudentID.Value != DBNull.Value && outPutStudentID.Value != null)
                    {
                        newStudentId = Convert.ToInt32(outPutStudentID.Value);
                    }
                }
            }
            catch
            {
                // handle exceptions (e.g., log them)
            }
            return newStudentId;

        }

        public static bool UpdateStudent(Models.StudentDTO studentDTO)
        {
            int rowAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = new SqlCommand("UpdateStudent", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@StudentId", studentDTO.ID);
                    command.Parameters.AddWithValue("@Name", studentDTO.Name);
                    command.Parameters.AddWithValue("@Age", studentDTO.Age);
                    command.Parameters.AddWithValue("@Grade", studentDTO.Grade);

                    connection.Open();
                    rowAffected = command.ExecuteNonQuery();

                }
            }
            catch
            {
                // handle exceptions (e.g., log them)
            }
            return rowAffected > 0;

        }

        public static bool DeleteStudent(int StudentId)
        {
            int rowAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                using (SqlCommand command = new SqlCommand("DeleteStudent", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@StudentId", StudentId);

                    connection.Open();
                    rowAffected = command.ExecuteNonQuery();
                }
            }
            catch
            {
                // handle exceptions (e.g., log them)
            }
            return rowAffected > 0;
        }
    }
}
