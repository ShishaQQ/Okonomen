using Okonomen.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Okonomen.Code
{
    public class ExcelHelper
    {
        public static void WriteBudgetCsv(Budget budget) {
            string filePath = @"C:\Users\Liz\Desktop\budget\" + budget.Id + ".csv";
            StringBuilder sb = new StringBuilder(); 

            foreach (var item in budget.BudgetItems)
            {
                sb.AppendLine(item.Name);
                sb.AppendLine(item.Number.ToString());
            }
           
            File.WriteAllText(filePath, sb.ToString());

        }
        // TODO lad brugeren selv vælge hvilken mappe der skal downloades til og fremvis nede i browseren at den er blevet downloadet
    }
}

