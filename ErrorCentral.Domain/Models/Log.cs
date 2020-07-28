using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ErrorCentral.Domain.Interfaces;

namespace ErrorCentral.Domain.Models
{
    public class Log : IBaseEntity
    {
        [Column("id")]
        [Key]
        [Required]
        public int Id { get; set; }
        
        [Column("name")]
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
        
        [Column("description")]
        [MaxLength(400)]
        [Required]
        public string Description { get; set; }
        
        [Column("created_at")]
        [Required]
        public DateTime CreatedAt { get; set; }
        
        [Column("user_id")]
        public int UserId { get; set; }
        
        [Column("environment_id")]
        public int EnvironmentId { get; set; }
        
        [Column("layer_id")]
        public int LayerId { get; set; }
        
        [Column("level_id")]
        public int LevelId { get; set; }
        
        [Column("status_id")]
        public int StatusId { get; set; }
        
        
        // Foreign keys
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        
        [ForeignKey("EnvironmentId")]
        public virtual Environment Environment { get; set; }
        
        [ForeignKey("LayerId")]
        public virtual Layer Layer { get; set; }
        
        [ForeignKey("LevelId")]
        public virtual Level Level { get; set; }
        
        [ForeignKey("StatusId")]
        public virtual Status Status { get; set; }
    }
}