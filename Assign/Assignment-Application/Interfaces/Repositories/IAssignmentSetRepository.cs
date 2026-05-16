using Assignment_Api;
using Assignment_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_Application.Interfaces.Repositories
{
    public interface IAssignmentSetRepository
    {
        Task<AssignmentSet?> GetByIdAsync(Guid id);
        Task<IEnumerable<AssignmentSet>> GetByCourseIdAsync(Guid courseId);
        Task CreateAsync(AssignmentSet assignmentSet);
        Task SaveChangesAsync();

    }
}
