﻿namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class AnswersQaResponse
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreateAt { get; set; }
        public int UserId { get; set; }
        public int QuestionId { get; set; }
    }
}
