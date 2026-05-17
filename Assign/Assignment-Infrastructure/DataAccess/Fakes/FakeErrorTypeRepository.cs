using Assignment_Application.Interfaces.Repositories;
using Assignment_Domain.Entities;

namespace Assignment_Infrastructure.DataAccess.Fakes
{
    /// <summary>
    /// In-memory fake repository for ErrorType. Pre-seeded with a set of common
    /// pedagogical error categories so the teacher's evaluation UI has
    /// something to populate its "type of mistake" dropdown with.
    /// </summary>
    public class FakeErrorTypeRepository : IErrorTypeRepository
    {
        private readonly List<ErrorType> _errorTypes;

        public FakeErrorTypeRepository()
        {
            _errorTypes = new List<ErrorType>();
            Seed();
        }

        private void Seed()
        {
            _errorTypes.Add(new ErrorType(
                "Calculation error",
                "Arithmetic or numerical mistake — right approach, wrong number."));

            _errorTypes.Add(new ErrorType(
                "Conceptual error",
                "Misunderstood the underlying concept being tested."));

            _errorTypes.Add(new ErrorType(
                "Procedural error",
                "Followed an incorrect sequence of steps."));

            _errorTypes.Add(new ErrorType(
                "Reading comprehension error",
                "Misread or misinterpreted the question."));

            _errorTypes.Add(new ErrorType(
                "Notation error",
                "Used incorrect symbols, units, or formatting."));

            _errorTypes.Add(new ErrorType(
                "Incomplete answer",
                "Answer started correctly but wasn't carried through to completion."));
        }

        // -----------------------------------------------------------------------
        // IErrorTypeRepository
        // -----------------------------------------------------------------------

        public Task<ErrorType?> GetByIdAsync(Guid id)
        {
            var et = _errorTypes.FirstOrDefault(e => e.Id == id);
            return Task.FromResult(et);
        }

        public Task<IEnumerable<ErrorType>> GetAllAsync()
        {
            IEnumerable<ErrorType> result = _errorTypes.ToList();
            return Task.FromResult(result);
        }

        public Task CreateAsync(ErrorType errorType)
        {
            _errorTypes.Add(errorType);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync() => Task.CompletedTask;
    }
}
