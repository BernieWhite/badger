// Copyright (c) Bernie White.
// Licensed under the MIT License.

using Badger.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Badger.Controllers
{
    [Route("healthz")]
    [ApiController]
    public sealed class HealthController : ControllerBase
    {
        private readonly HealthServiceOptions _Options;

        public HealthController(IOptions<HealthServiceOptions> options)
        {
            _Options = options.Value;
        }

        // GET api/<HealthController>/5
        [HttpGet("{id}")]
        public ActionResult Get(string id)
        {
            if (!_Options.Keys.Contains(id))
                return Unauthorized();

            return Ok();
        }

        [HttpHead("{id}")]
        public ActionResult Head(string id)
        {
            if (!_Options.Keys.Contains(id))
                return Unauthorized();

            return Ok();
        }
    }
}
