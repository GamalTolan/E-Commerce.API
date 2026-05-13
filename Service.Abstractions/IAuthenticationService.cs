using Shared.IdentityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstractions
{
    public interface IAuthenticationService
    {
        public Task<UserResultDto> RegisterAsync(RegisterDto registerDto);
        public Task<UserResultDto> LoginAsync(LoginDto loginDto); 
        public Task<UserResultDto>GetUserByEmailAsync(string email);
        public Task<bool> IsEmailExist(string email);
        public Task<AddressDto>GetUserAddressAsync(string email);
        public Task<AddressDto>UpdateUserAddressAsync(string email ,AddressDto addressDto);


    }
}
