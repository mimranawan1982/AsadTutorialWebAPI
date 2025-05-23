﻿using System.ComponentModel.DataAnnotations;

namespace AsadTutorialWebAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public byte[] Password { get; set; }

        public byte[] PasswordKey { get; set; }
    }
}