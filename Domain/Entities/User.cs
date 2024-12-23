﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApplicationProduct.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, MaxLength(50)]
        public string Username { get; set; }

        [Required, MinLength(8)]
        public string Password { get; set; } // Hash a senha antes de salvar

        public bool Enable { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}
