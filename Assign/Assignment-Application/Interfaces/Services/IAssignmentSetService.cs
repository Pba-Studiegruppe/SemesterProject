using Assignment_Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_Application.Interfaces.Services
{
    public interface IAssignmentSetService
    {
        Task<AssignmentSetDTO> GetAssignmentSetsAsync(Guid id);
        Task<IEnumerable<AssignmentSetDTO>> GetAssignmentSetsByCourseIdAsync(Guid courseId);
        Task<AssignmentSetDTO> CreateAssignmentSetAsync(CreateAssignmentSetRequest request);
         Task<AssignmentSetDTO> UpdateAssignmentSetAsync(Guid assignmentSetId, UpdateAssignmentSetRequest request);

        Task<AssignmentSetDTO> PublishAssignmentSetAsync(Guid id);
        Task<IEnumerable<AssignmentSetDTO>> GetAllAssignmentSetsAsync();
    }
}
