using Exercise_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Application.Interfaces.Repositories
{
    public interface IKeywordRepository
    {
        void AddKeyword(Keyword keyword);
        void GetAllKeywords();
        void GetKeywordById(Guid keywordId);
    }
}
