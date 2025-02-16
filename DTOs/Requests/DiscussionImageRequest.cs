﻿using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class DiscussionImageRequest
    {
        [Required]
        public int DiscussionId { get; set; }

        [Required]
        [Url]
        public string ImageUrl { get; set; } = string.Empty;
    }
}
