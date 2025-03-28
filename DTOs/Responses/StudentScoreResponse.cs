using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class StudentScoreResponse
    {
        public int Id { get; set; }
        public double? Score { get; set; }
        public int ScoreTypeId { get; set; }
        public int SubjectId { get; set; }
        public int UserId { get; set; }
        public int SemesterId { get; set; }
    }
}
