using Amazon.API.Errors;
using Amazon.Repository.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Amazon.API.Controllers
{
    // Draft 
    public class BuggyController : BaseApiController
    {
        private readonly StoreContext _context;

        public BuggyController(StoreContext context)
        {
            _context = context;
        }

        // Get : api/Buggy/notfound
        [HttpGet("notfound")]
        public ActionResult GetNotFoundRequest()
        {
            var product = _context.Products.Find(100);
            if (product == null)
                return NotFound(new ApiResponse(404));
            return Ok(product);
        }

        // Get : api/Buggy/servererror
        [HttpGet("servererror")]
        public ActionResult GetServerError()
        {
            var product = _context.Products.Find(100); // null
            var productToReturn = product.ToString(); // will throw Exeption [Null Refrence  Exeption ]

            return Ok(productToReturn);
        }

        // Get : api/Buggy/badrequest
        [HttpGet("badrequest")]
        public ActionResult GetBadRequst()
        {
            return BadRequest(new ApiResponse(400));
        }

        // Get : api/Buggy/badrequest/Five
        [HttpGet("badrequest/{id}")]
        public ActionResult GetBadRequst(int id)
        {
            return Ok();
        }

    }
}
