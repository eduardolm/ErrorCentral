using System;

namespace ErrorCentral.Domain.DTOs

{
    public class LogDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserDTO User { get; set; }
        public EnvironmentDTO Environment { get; set; }
        public LayerDTO Layer { get; set; }
        public LevelDTO Level { get; set; }
        public StatusDTO Status { get; set; }
    }
}