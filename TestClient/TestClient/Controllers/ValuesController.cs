using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TestClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IHttpContextAccessor _context;

        public ValuesController(IHttpContextAccessor context)
        {
            _context = context;
        }

        // GET api/values
        [HttpGet]
        [Authorize]
        public ActionResult<string> Get()
        {
            //如果要验证通过  必须在头部新增 Headers.Add("Authorization", "Bearer "+json["token"]);
            //注意前面加 Bearer+空格
            var id = _context.HttpContext.User.FindFirst("id");
            var name = _context.HttpContext.User.FindFirst("name");
            var userid = id != null ? Convert.ToInt32(id.Value) : 0;
            var userName = name != null ? name.Value : "";

            return  $"userid={userid},userName={userName}.";
        }

        
    }
}
