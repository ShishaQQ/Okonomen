using System;
using System.Collections.Generic;

#nullable disable

namespace Okonomen.Models
{
    public partial class Budget
    {
        public Budget()
        {
            BudgetItems = new List<BudgetItem>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }

        public virtual AspNetUser User { get; set; }
        public virtual ICollection<BudgetItem> BudgetItems { get; set; }

        public void AddBudgetTtems(string name, decimal number)
        {
            BudgetItem item = new BudgetItem
            {
                Id = Guid.NewGuid(),
                Name = name,
                Number = number,
                BudgetId = this.Id
            };

            this.BudgetItems.Add(item);
            
        }
    }
   
}
