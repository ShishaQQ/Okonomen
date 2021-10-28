using System;
using System.Collections.Generic;

#nullable disable

namespace Okonomen.Models
{
    public partial class BudgetItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal? Number { get; set; }
        public Guid BudgetId { get; set; }

        public virtual Budget Budget { get; set; }
    }
}
