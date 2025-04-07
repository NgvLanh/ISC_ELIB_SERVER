using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

    public class StudentScoreDashboardResponse
    {
        public StudentScoreByTestResponse Test { get; set; }
        public ICollection<StudentResponse> Students { get; set; }
    }

    public class StudentScoreByTestResponse
    {
        public int Id { get; set; }
        public ClassScoreResponse Class { get; set; }
        public SubjectScoreResponse Subject { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    public class SubjectScoreResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ClassScoreResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }

    public class StudentResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ICollection<SemesterScoreResponse> Semesters { get; set; }
        public double AverageScore { get; set; }
        public bool? Passed { get; set; }
        public DateTime LastUpdate { get; set; }
    }

    public class SemesterScoreResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ScoreResponse> Scores { get; set; }
        public double AverageScore { get; set; }
    }

    public class ScoreResponse
    {
        public int Id { get; set; }
        // public int UserId { get; set; }
        public double Score { get; set; }
        public TypeScoreResponse ScoreType { get; set; }

    }

    public class TypeScoreResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }
    }
}
