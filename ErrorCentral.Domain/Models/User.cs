using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ErrorCentral.Domain.Interfaces;

namespace ErrorCentral.Domain.Models
{
    public class User : IBaseEntity
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }
        
        [Column("full_name")]
        [MaxLength(100)]
        [Required]
        public string FullName { get; set; }
        
        [Column("email")]
        [MaxLength(100)]
        [Required]
        public string Email { get; set; }
        
        [Column("password")]
        [MaxLength(100)]
        [Required]
        public string Password { get; set; }
        
        [Column("created_at")]
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}