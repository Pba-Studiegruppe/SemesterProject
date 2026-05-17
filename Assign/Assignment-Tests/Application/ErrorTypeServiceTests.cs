using Assignment_Application.DTO;
using Assignment_Application.Implementations;
using Assignment_Application.Interfaces.Repositories;
using Assignment_Domain.Entities;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Assignment_Tests.Application
{
    // ─────────────────────────────────────────────────────────────────────────
    // GetAllErrorTypesAsync
    // ─────────────────────────────────────────────────────────────────────────

    public class ErrorTypeServiceTests_GetAll_Tests
    {
        [Fact]
        public async Task Should_Return_All_ErrorTypes_As_DTOs()
        {
            // Arrange
            var et1 = new ErrorType("Calculation error", "Arithmetic mistake");
            var et2 = new ErrorType("Logical error");

            var repo = new Mock<IErrorTypeRepository>();
            repo.Setup(r => r.GetAllAsync()).ReturnsAsync(new[] { et1, et2 });
            var service = new ErrorTypeService(repo.Object);

            // Act
            var dtos = (await service.GetAllErrorTypesAsync()).ToList();

            // Assert
            dtos.Should().HaveCount(2);
            dtos.Should().Contain(d => d.Name == "Calculation error" && d.Description == "Arithmetic mistake");
            dtos.Should().Contain(d => d.Name == "Logical error" && d.Description == null);
        }

        [Fact]
        public async Task Should_Return_Empty_When_None()
        {
            // Arrange
            var repo = new Mock<IErrorTypeRepository>();
            repo.Setup(r => r.GetAllAsync()).ReturnsAsync(Array.Empty<ErrorType>());
            var service = new ErrorTypeService(repo.Object);

            // Act
            var dtos = await service.GetAllErrorTypesAsync();

            // Assert
            dtos.Should().BeEmpty();
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // GetErrorTypeAsync
    // ─────────────────────────────────────────────────────────────────────────

    public class ErrorTypeServiceTests_Get_Tests
    {
        [Fact]
        public async Task Should_Return_DTO_When_Found()
        {
            // Arrange
            var et = new ErrorType("Conceptual error", "Misunderstood the concept");
            var repo = new Mock<IErrorTypeRepository>();
            repo.Setup(r => r.GetByIdAsync(et.Id)).ReturnsAsync(et);
            var service = new ErrorTypeService(repo.Object);

            // Act
            var dto = await service.GetErrorTypeAsync(et.Id);

            // Assert
            dto.Id.Should().Be(et.Id);
            dto.Name.Should().Be("Conceptual error");
            dto.Description.Should().Be("Misunderstood the concept");
        }

        [Fact]
        public async Task Should_Throw_When_Not_Found()
        {
            // Arrange
            var repo = new Mock<IErrorTypeRepository>();
            repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((ErrorType?)null);
            var service = new ErrorTypeService(repo.Object);

            // Act
            Func<Task> act = () => service.GetErrorTypeAsync(Guid.NewGuid());

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>();
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // CreateErrorTypeAsync
    // ─────────────────────────────────────────────────────────────────────────

    public class ErrorTypeServiceTests_Create_Tests
    {
        [Fact]
        public async Task Should_Create_And_Return_DTO()
        {
            // Arrange
            var repo = new Mock<IErrorTypeRepository>();
            repo.Setup(r => r.CreateAsync(It.IsAny<ErrorType>())).Returns(Task.CompletedTask);
            repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
            var service = new ErrorTypeService(repo.Object);

            var request = new CreateErrorTypeRequest
            {
                Name = "Procedural error",
                Description = "Followed wrong steps"
            };

            // Act
            var dto = await service.CreateErrorTypeAsync(request);

            // Assert
            dto.Id.Should().NotBe(Guid.Empty);
            dto.Name.Should().Be("Procedural error");
            dto.Description.Should().Be("Followed wrong steps");

            repo.Verify(r => r.CreateAsync(It.Is<ErrorType>(e =>
                e.Name == "Procedural error" &&
                e.Description == "Followed wrong steps")), Times.Once);
            repo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Should_Throw_When_Request_Null()
        {
            var service = new ErrorTypeService(new Mock<IErrorTypeRepository>().Object);

            Func<Task> act = () => service.CreateErrorTypeAsync(null!);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task Should_Propagate_Domain_Exception_When_Name_Empty(string? name)
        {
            // Arrange
            var repo = new Mock<IErrorTypeRepository>();
            var service = new ErrorTypeService(repo.Object);
            var request = new CreateErrorTypeRequest { Name = name! };

            // Act
            Func<Task> act = () => service.CreateErrorTypeAsync(request);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
            repo.Verify(r => r.CreateAsync(It.IsAny<ErrorType>()), Times.Never);
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // UpdateErrorTypeAsync
    // ─────────────────────────────────────────────────────────────────────────

    public class ErrorTypeServiceTests_Update_Tests
    {
        [Fact]
        public async Task Should_Update_Name_And_Description()
        {
            // Arrange
            var et = new ErrorType("Old name", "Old description");
            var repo = new Mock<IErrorTypeRepository>();
            repo.Setup(r => r.GetByIdAsync(et.Id)).ReturnsAsync(et);
            repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
            var service = new ErrorTypeService(repo.Object);

            var request = new UpdateErrorTypeRequest
            {
                Name = "New name",
                Description = "New description"
            };

            // Act
            var dto = await service.UpdateErrorTypeAsync(et.Id, request);

            // Assert
            dto.Name.Should().Be("New name");
            dto.Description.Should().Be("New description");
            et.Name.Should().Be("New name");
            et.Description.Should().Be("New description");
            repo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Should_Allow_Clearing_Description()
        {
            // Arrange
            var et = new ErrorType("Name", "Description that will be cleared");
            var repo = new Mock<IErrorTypeRepository>();
            repo.Setup(r => r.GetByIdAsync(et.Id)).ReturnsAsync(et);
            var service = new ErrorTypeService(repo.Object);

            var request = new UpdateErrorTypeRequest { Name = "Name", Description = null };

            // Act
            var dto = await service.UpdateErrorTypeAsync(et.Id, request);

            // Assert
            dto.Description.Should().BeNull();
            et.Description.Should().BeNull();
        }

        [Fact]
        public async Task Should_Throw_When_Request_Null()
        {
            var service = new ErrorTypeService(new Mock<IErrorTypeRepository>().Object);

            Func<Task> act = () => service.UpdateErrorTypeAsync(Guid.NewGuid(), null!);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Should_Throw_When_Not_Found()
        {
            var repo = new Mock<IErrorTypeRepository>();
            repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((ErrorType?)null);
            var service = new ErrorTypeService(repo.Object);

            Func<Task> act = () => service.UpdateErrorTypeAsync(
                Guid.NewGuid(),
                new UpdateErrorTypeRequest { Name = "x" });

            await act.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task Should_Propagate_Domain_Exception_When_Name_Empty(string? name)
        {
            // Arrange
            var et = new ErrorType("Valid name");
            var repo = new Mock<IErrorTypeRepository>();
            repo.Setup(r => r.GetByIdAsync(et.Id)).ReturnsAsync(et);
            var service = new ErrorTypeService(repo.Object);

            // Act
            Func<Task> act = () => service.UpdateErrorTypeAsync(et.Id,
                new UpdateErrorTypeRequest { Name = name! });

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
            repo.Verify(r => r.SaveChangesAsync(), Times.Never);
        }
    }
}
