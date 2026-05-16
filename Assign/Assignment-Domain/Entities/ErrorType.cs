using System;
using System.Collections.Generic;

namespace Assignment_Domain.Entities;

public partial class ErrorType
{
    public Guid Id { get; set; }

    public string? ErrorName { get; set; }

    public string? ErrorDescription { get; set; }

    public byte[]? RowVersion { get; set; }

    public virtual ICollection<ExerciseFeedback> ExerciseFeedbacks { get; set; } = new List<ExerciseFeedback>();
}
