using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ConfigurationSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ChannelFactory<IRoleService> _service;

        public ValuesController(ChannelFactory<IRoleService> service)
        {
            _service = service;
            var channel = service.CreateChannel();
            var role = channel.GetRoles("user1");
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { _service.GetType().FullName };
        }
    }
}
