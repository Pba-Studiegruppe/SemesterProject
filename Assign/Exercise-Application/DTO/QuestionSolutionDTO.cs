using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Application.DTO
{
    public class QuestionSolutionDTO
    {
        public int Id { get; set; }
        public string SolutionText { get; set; }
        public int QuestionId { get; set; }
    }
}
