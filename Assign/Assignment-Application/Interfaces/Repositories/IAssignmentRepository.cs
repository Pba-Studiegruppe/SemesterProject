using Assignment_Api;
using Assignment_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_Application.Interfaces.Repositories
{
    public interface IAssignmentRepository
    {
        Task<Assignment?> GetByIdAsync(Guid id);
        Task CreateAsync(Assignment assignment);
        Task SaveChangesAsync();
    }
}
