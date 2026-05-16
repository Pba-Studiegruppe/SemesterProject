using System;
using System.Threading.Tasks;

namespace Assignment_Application.Interfaces.Services
{
    public interface IAssignmentPdfService
    {
        Task<byte[]> GenerateAsync(Guid assignmentId);
    }
}