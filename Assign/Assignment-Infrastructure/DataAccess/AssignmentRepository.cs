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

        public Task CreateAsync(Assignment assignment)
        {
            throw new NotImplementedException();
        }

        public Task<Assignment?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
