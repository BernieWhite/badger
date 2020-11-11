// Copyright (c) Bernie White.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Badger.Models;
using Badger.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Badger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class BadgeController : ControllerBase
    {
        private readonly IBadgeService _BadgeService;

        public BadgeController(IBadgeService badgeService)
        {
            _BadgeService = badgeService;
        }

        // GET api/<BadgeController>/5
        [HttpGet("{service}/{**id}")]
        [Produces("image/svg+xml;charset=utf-8")]
        public async Task<ActionResult> GetAsync(string service, string id)
        {
            try
            {
                var stream = await _BadgeService.GetAsync(service, id);
                if (stream == null)
                    return NotFound();
                
                return File(stream, "image/svg+xml;charset=utf-8");
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // PUT api/<BadgeController>/5
        [HttpPut("{service}/{**id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task PutAsync(string service, string id, [FromBody] BadgeCreate badge)
        {
            await _BadgeService.CreateAsync(service, id, badge.Status, badge.Label);
        }
    }
}
