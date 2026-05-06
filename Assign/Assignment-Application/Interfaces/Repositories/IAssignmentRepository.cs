using Assignment_Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_Application.Interfaces.Repositories
{
    public interface IAssignmentRepository
    {
        Task<AssignmentSet> AddAsync(AssignmentSet assignmentSet);
        Task<AssignmentSet?> GetByIdAsync(Guid exerciseId);
        Task<AssignmentSet> UpdateAsync(AssignmentSet assignmentSet, byte[] rowVersion);
        Task<IEnumerable<AssignmentSet>> GetByCourseIdAsync(Guid courseId);
    }
}
