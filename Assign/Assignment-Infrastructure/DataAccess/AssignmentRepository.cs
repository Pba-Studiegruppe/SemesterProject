using Assignment_Api;
using Assignment_Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_Infrastructure.DataAccess
{
    public class AssignmentRepository: IAssignmentRepository
    {
        private DbContext dbContext;

        public AssignmentRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<AssignmentSet> AddAsync(AssignmentSet assignmentSet)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AssignmentSet>> GetByCourseIdAsync(Guid courseId)
        {
            throw new NotImplementedException();
        }

        public Task<AssignmentSet?> GetByIdAsync(Guid exerciseId)
        {
            throw new NotImplementedException();
        }

        public Task<AssignmentSet> UpdateAsync(AssignmentSet assignmentSet, byte[] rowVersion)
        {
            throw new NotImplementedException();
        }
    }
}
