using Microsoft.AspNetCore.Mvc;
using ServiceModel.Configuration;
using System.Collections.Generic;

namespace ConfigurationSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IRoleService _service;

        public ValuesController(IChannelFactoryProvider channelProvider)
        {
            _service = channelProvider.CreateChannelFactory<IRoleService>("roles").CreateChannel();
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new ActionResult<IEnumerable<string>>(_service.GetRoles("user1"));
        }
    }
}
