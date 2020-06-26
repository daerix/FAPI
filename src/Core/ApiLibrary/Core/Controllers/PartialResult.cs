using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApiLibrary.Core.Controllers
{
    public class PartialResult : ActionResult, IActionResult
    {
        private readonly object _result;
        public PartialResult(object result)
        {
            _result = result;
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            ObjectResult or = new ObjectResult(_result);
            or.StatusCode = 206;
            return or.ExecuteResultAsync(context);
        }
    }
}
