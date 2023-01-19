using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Core.Entities
{
    public class Employee :BaseEntity
    {
        public string Name { get; set; }
        public decimal Salary { get; set; }

        public Department deprtment { get; set; }
    }
}
