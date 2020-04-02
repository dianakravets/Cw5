

using System;
using System.Data.SqlClient;
using Cw5.DTOs.Requests;
using Cw5.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Cw5.Services
{
    public class SqlServerStudentDbService : IStudentDbService
    

    {
        public SqlServerStudentDbService(/*.. */ )
        {

        }
        private const string ConString =
            "Data Source=db-mssql16.pjwstk.edu.pl;Initial Catalog=s19054;User ID=apbds19054;Password=admin";

        [HttpPost]
        public void EnrollStudent(EnrollStudentRequest request)
        {

            using (var con = new SqlConnection(ConString))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();
                var tran = con.BeginTransaction();
                try
                {
                    com.CommandText = "select IdStudy from Studies where Name=@Name";
                    com.Parameters.AddWithValue("Name", request.Studies);
                    var dr = com.ExecuteReader();
                    if (!dr.Read())
                    {
                       // return BadRequest("Studia nie istnieja");
                    }

                    int idstudies = (int) dr["IdStudies"];
                    com.CommandText = "select IdEnrollment from Enrollment where Semester=1 and IdStudy=@idstudies";
                    com.Parameters.AddWithValue("idstudies", idstudies);
                    int semester = 1;

                    if (!dr.Read())
                    {
                        com.CommandText =
                            "Insert into Enrollment(IdEnrollment,Semester,IdStudy,StartDate) VALUES (@index,@semester,@idStudy,@date)";
                        com.Parameters.AddWithValue("index", 4);
                        com.Parameters.AddWithValue("semester", semester);
                        com.Parameters.AddWithValue("idStudy", idstudies);
                        com.Parameters.AddWithValue("date", DateTime.Today);
                    }

                    int idenroll = (int) dr["IdEnrollment"];
                    com.CommandText = "select IndexNumber from Student where IndexNumber=@id";
                    com.Parameters.AddWithValue("id", request.IndexNumber);
                    if (dr.Read())
                    {
                    }

                    com.CommandText =
                        "Insert into Student (IndexNumber,FirstName,LastName,BirthDate,IdEnrollment) values(@idstud,@fname,@lname,@birth,@idenrollment) ";
                    com.Parameters.AddWithValue("idstud", request.IndexNumber);
                    com.Parameters.AddWithValue("fname", request.FirstName);
                    com.Parameters.AddWithValue("lname", request.LastName);
                    com.Parameters.AddWithValue("birth", request.BirthDate);
                    com.Parameters.AddWithValue("idenrollment", idenroll);
                    com.ExecuteNonQuery();


                    var response = new EnrollStudentResponse();
                    response.Semester = semester;
                    response.LastName = request.LastName;
                    response.StartDate = DateTime.Today;
                    tran.Commit();
                   // return Created("", response);
                }
                catch (SqlException ex)
                {
                    tran.Rollback();
                }
            }


         //  return Ok();
        }

        [HttpPost]
        public void PromoteStudents(int semester, string studies)
        {

            using (var con = new SqlConnection(ConString))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "cw5";
               // com.Parameters.AddWithValue("Studies", "IT");
               // com.Parameters.AddWithValue("Semester", 1);
                con.Open();
                var dr = com.ExecuteReader();
                dr.Read();
                var newenrollment = new EnrollStudentResponse();

                newenrollment.Semester = int.Parse(dr["Semester"].ToString());
                    newenrollment.LastName = dr["LastName"].ToString();
                    newenrollment.StartDate = DateTime.Parse("StartDate");
                

               // return Created("", newenrollment);
            }

            
        }
    }
}