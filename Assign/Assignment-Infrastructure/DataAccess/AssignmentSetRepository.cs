using Assignment_Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_Infrastructure.DataAccess
{
    public class AssignmentSetRepository: IAssignmentSetRepository
    {
        private DbContext dbContext;

        public AssignmentSetRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
