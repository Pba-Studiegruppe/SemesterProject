using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Domain.shared
{
    public abstract class Entity(Guid id)
    {
        public Guid Id { get; init; } = id;

        [Timestamp] public byte[] RowVersion { get; private set; } = [];
    }
}
