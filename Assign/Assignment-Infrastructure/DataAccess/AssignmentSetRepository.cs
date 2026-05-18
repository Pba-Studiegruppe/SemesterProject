using Assignment_Application.Interfaces.Repositories;
using Assignment_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_Infrastructure.DataAccess
{
    public class AssignmentSetRepository : IAssignmentSetRepository
    {
        public Task CreateAsync(AssignmentSet assignmentSet)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AssignmentSet>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AssignmentSet>> GetByCourseIdAsync(Guid courseId)
        {
            throw new NotImplementedException();
        }

        public Task<AssignmentSet?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
