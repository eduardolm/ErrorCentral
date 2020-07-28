﻿using System;

namespace ErrorCentral.Domain.DTOs
{
    public class UserDTO 
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}