using System;

namespace ErrorCentral.Domain.DTOs

{
    public class LogDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserDto User { get; set; }
        public EnvironmentDto Environment { get; set; }
        public LayerDto Layer { get; set; }
        public LevelDto Level { get; set; }
        public StatusDto Status { get; set; }
    }
}