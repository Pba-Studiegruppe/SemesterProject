using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Application.DTO
{
    public class KeywordDTO
    {
        public Guid Id { get; set; }
        public string KeywordName { get; set; }
        public string KeywordType { get; set; }


    }

    public class CreateKeywordRequest
    {
        public string KeywordName { get; set; }
        public string KeywordType { get; set; }

    }
}
