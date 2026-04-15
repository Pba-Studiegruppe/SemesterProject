using Assignment_Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_Domain.Entities
{
    public class Assignment : Entity
    {


        public Assignment() : base(Guid.NewGuid())
        {
        }
    }
}
