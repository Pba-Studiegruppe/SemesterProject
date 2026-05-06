using Assignment_Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_Application.Interfaces.Services
{
    public interface IAssignmentService
    {
        Task<AssignmentDTO> GetAssignmentAsync(Guid id);
        Task<AssignmentDTO> CreateAssignmentAsync(CreateAssignmentRequest request);
        Task<AssignmentDTO> UpdateAssignmentAsync(UpdateAssignmentRequest request);
    }
}
