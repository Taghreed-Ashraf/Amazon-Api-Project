using Amazon.Core.Entities;
using Amazon.Core.Repositories;
using Amazon.Core.Specification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Amazon.API.Controllers
{
    
    public class EmployeeController : BaseApiController
    {

        private readonly IGenericRepository<Employee> _employeesRepo;

        public EmployeeController(IGenericRepository<Employee> employeesRepo)
        {
            _employeesRepo = employeesRepo;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetProducts()
        {
            
            var spec = new EmployeewithDepartmenSpecfiction();
            var employees = await _employeesRepo.GetAllwithSpecAsync(spec);

            return Ok(employees);
        }
    }
}
