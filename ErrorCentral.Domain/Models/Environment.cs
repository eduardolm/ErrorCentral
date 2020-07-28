using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ErrorCentral.Domain.Interfaces;

namespace ErrorCentral.Domain.Models
{
    public class Environment : IBaseEntity
    {
        [Column("id")]
        [Key]
        [Required]
        public int Id { get; set; }
        
        [Column("name")]
        [MaxLength(20)]
        [Required]
        public string Name { get; set; }
    }
}