using System;

namespace Assignment_Application.DTO
{
    public class ErrorTypeDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class CreateErrorTypeRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class UpdateErrorTypeRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
