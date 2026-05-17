using Assignment_Domain.Entities;
using FluentAssertions;
using System;
using Xunit;

namespace Assignment_Tests.Domain
{
    public class ErrorTypeTests
    {
        [Fact]
        public void Constructor_Should_Set_Properties()
        {
            // Arrange / Act
            var et = new ErrorType("Calculation error", "Mistake in arithmetic");

            // Assert
            et.Id.Should().NotBe(Guid.Empty);
            et.Name.Should().Be("Calculation error");
            et.Description.Should().Be("Mistake in arithmetic");
        }

        [Fact]
        public void Constructor_Should_Allow_Null_Description()
        {
            // Arrange / Act
            var et = new ErrorType("Logical error");

            // Assert
            et.Description.Should().BeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_Should_Throw_When_Name_Empty(string? name)
        {
            // Act
            Action act = () => new ErrorType(name!, "desc");

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Rename_Should_Change_Name()
        {
            // Arrange
            var et = new ErrorType("Old name");

            // Act
            et.Rename("New name");

            // Assert
            et.Name.Should().Be("New name");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Rename_Should_Throw_When_Name_Empty(string? name)
        {
            // Arrange
            var et = new ErrorType("Old name");

            // Act
            Action act = () => et.Rename(name!);

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void UpdateDescription_Should_Change_Description()
        {
            // Arrange
            var et = new ErrorType("Some error", "old");

            // Act
            et.UpdateDescription("new");

            // Assert
            et.Description.Should().Be("new");
        }
    }
}
