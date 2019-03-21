using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ProfileController : Controller
    {
        // GET: api/profile
        [HttpGet, Authorize]
        public string Get()
        {
            return "profile";
        }
    }
}
