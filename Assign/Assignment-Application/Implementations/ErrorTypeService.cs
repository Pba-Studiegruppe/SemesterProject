using Assignment_Application.DTO;
using Assignment_Application.Interfaces.Repositories;
using Assignment_Application.Interfaces.Services;
using Assignment_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment_Application.Implementations
{
    public class ErrorTypeService : IErrorTypeService
    {
        private readonly IErrorTypeRepository _repository;

        public ErrorTypeService(IErrorTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ErrorTypeDTO>> GetAllErrorTypesAsync()
        {
            var errorTypes = await _repository.GetAllAsync();
            return errorTypes.Select(ToDto).ToList();
        }

        public async Task<ErrorTypeDTO> GetErrorTypeAsync(Guid id)
        {
            var et = await _repository.GetByIdAsync(id);
            if (et is null)
                throw new KeyNotFoundException($"ErrorType with id '{id}' not found.");
            return ToDto(et);
        }

        public async Task<ErrorTypeDTO> CreateErrorTypeAsync(CreateErrorTypeRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            // Domain constructor enforces non-empty name.
            var et = new ErrorType(request.Name, request.Description);

            await _repository.CreateAsync(et);
            await _repository.SaveChangesAsync();

            return ToDto(et);
        }

        public async Task<ErrorTypeDTO> UpdateErrorTypeAsync(Guid id, UpdateErrorTypeRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            var et = await _repository.GetByIdAsync(id);
            if (et is null)
                throw new KeyNotFoundException($"ErrorType with id '{id}' not found.");

            // Domain validates name; description can be null.
            et.Rename(request.Name);
            et.UpdateDescription(request.Description);

            await _repository.SaveChangesAsync();

            return ToDto(et);
        }

        private static ErrorTypeDTO ToDto(ErrorType et) => new()
        {
            Id = et.Id,
            Name = et.Name,
            Description = et.Description
        };
    }
}
