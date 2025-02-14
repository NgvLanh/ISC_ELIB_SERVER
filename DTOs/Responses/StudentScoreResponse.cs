using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class StudentScoreResponse
    {
        public long Id { get; set; }
        public double? Score { get; set; }
        public long ScoreTypeId { get; set; }
        public long SubjectId { get; set; }
        public long UserId { get; set; }
        public long SemesterId { get; set; }
    }
}
