using Amazon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Core.Specification
{
    public class EmployeewithDepartmenSpecfiction : BaseSpecification<Employee>
    {
        public EmployeewithDepartmenSpecfiction()
        {
            AddIncludes(E => E.deprtment);
        }

        public EmployeewithDepartmenSpecfiction(int id) : base(E => E.Id == id)
        {
            AddIncludes(E => E.deprtment);
        }
    }
}
