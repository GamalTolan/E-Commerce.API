using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using Shared.IdentityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class AuthenticationController(IServiceManager serviceManager) : ApiController
    {


        [HttpPost("Register")]
        public async Task<ActionResult<UserResultDto>> Register(RegisterDto registerDto)
        {
            var result = await serviceManager.AuthenticationService.RegisterAsync(registerDto);
            return Ok(result);
        }
        [HttpPost("Login")]
        public async Task<ActionResult<UserResultDto>> Login(LoginDto loginDto)
        {
            var result = await serviceManager.AuthenticationService.LoginAsync(loginDto);
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserResultDto>> GetCurrentUser()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var result = await serviceManager.AuthenticationService.GetUserByEmailAsync(email);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<bool>> IsEmailExist(string email)
        {
            var result = await serviceManager.AuthenticationService.IsEmailExist(email);
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var result = await serviceManager.AuthenticationService.GetUserAddressAsync(email);
            return Ok(result);

        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto addressDto)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var result = await serviceManager.AuthenticationService.UpdateUserAddressAsync(email, addressDto);
            return Ok(result);
        }
    }
}
