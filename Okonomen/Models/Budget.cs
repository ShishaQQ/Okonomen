using System;
using System.Collections.Generic;

#nullable disable

namespace Okonomen.Models
{
    public partial class Budget
    {
        public Budget()
        {
            BudgetItems = new HashSet<BudgetItem>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }

        public virtual AspNetUser User { get; set; }
        public virtual ICollection<BudgetItem> BudgetItems { get; set; }
    }
}
