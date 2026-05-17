using Assignment_Application.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Assignment_Application.Interfaces.Services
{
    public interface IErrorTypeService
    {
        Task<IEnumerable<ErrorTypeDTO>> GetAllErrorTypesAsync();
        Task<ErrorTypeDTO> GetErrorTypeAsync(Guid id);
        Task<ErrorTypeDTO> CreateErrorTypeAsync(CreateErrorTypeRequest request);
        Task<ErrorTypeDTO> UpdateErrorTypeAsync(Guid id, UpdateErrorTypeRequest request);
    }
}
