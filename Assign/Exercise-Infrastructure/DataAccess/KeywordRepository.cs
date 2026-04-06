using Exercise_Application.Interfaces.Repositories;
using Exercise_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Infrastructure.DataAccess
{
    public class KeywordRepository : IKeywordRepository
    {
        public void AddKeyword(Keyword keyword)
        {
            throw new NotImplementedException();
        }

        public void GetAllKeywords()
        {
            throw new NotImplementedException();
        }

        public void GetKeywordById(Guid keywordId)
        {
            throw new NotImplementedException();
        }
    }
}
