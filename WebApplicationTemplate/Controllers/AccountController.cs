using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplicationTemplate.Security;

namespace WebApplicationTemplate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccessManager _accessManager;
        public AccountController(AccessManager accessManager)
        {
            _accessManager = accessManager;
        }

        [HttpPost]
        public IActionResult Login([FromBody]User usuario)
        {
           

            if (_accessManager.ValidateCredentials(usuario))
                return Ok(_accessManager.GenerateToken(usuario));
            else
                return Ok(new
                {
                    Authenticated = false,
                    Message = "Falha ao autenticar"
                });
        }
    }
}