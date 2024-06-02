﻿namespace ReddgitAPI.Application.Questions.Models
{
    public class QuestionDto
    {
        public string Id { get; set; }
        public string Title { get; set; }

        public string Content { get; set; }
        public string UserName { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}
