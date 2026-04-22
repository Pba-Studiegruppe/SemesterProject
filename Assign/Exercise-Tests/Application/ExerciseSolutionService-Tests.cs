using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Tests.Application
{
    public class SetExerciseSolution_Tests
    {
        [Fact]
        public async Task SetExerciseSolution_Should_Set_Solution_For_Question()
        {
        }

        [Fact]
        public async Task SetExerciseSolution_Should_Overwrite_Existing_Question_Solution()
        {
        }

        [Fact]
        public async Task SetExerciseSolution_Should_Throw_When_Setting_Solution_For_Nonexistent_Question()
        {
        }

        [Fact]
        public async Task SetExerciseSolution_Should_Throw_When_Setting_Solution_For_Question_With_Null_Content()
        {

        }
    }

    public class UpdateExerciseSolution_Tests
    {
        [Fact]
        public async Task UpdateExerciseSolution_Should_Update_Solution_For_Exercise()
        {
        }
        [Fact]
        public async Task UpdateExerciseSolution_Should_Throw_When_Updating_Solution_For_Nonexistent_Exercise()
        {
        }
        [Fact]
        public async Task UpdateExerciseSolution_Should_Throw_When_Updating_Solution_With_Null_Content()
        {
        }
    }

    public class RemoveExerciseSolution_Tests
    {
        [Fact]
        public async Task RemoveExerciseSolution_Should_Remove_Solution_From_Exercise()
        {
        }
        [Fact]
        public async Task RemoveExerciseSolution_Should_Throw_When_Removing_Solution_For_Nonexistent_Exercise()
        {
        }
    }
}
