using System;

namespace Cw5.DTOs.Responses
{
    public class EnrollStudentResponse
    {
        public string LastName { get; set; }
        public DateTime StartDate { get; set; }
        public int Semester { get; set; }
    }
}