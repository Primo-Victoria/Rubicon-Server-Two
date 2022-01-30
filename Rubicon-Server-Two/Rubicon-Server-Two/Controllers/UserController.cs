using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rubicon_Server_Two.Core.Models;
using Rubicone_Server_Two.BusinessLogic.Core.Interfaces;
using Rubicone_Server_Two.BusinessLogic.Core.Models;
using Rubicone_Server_Two.DataAccess.Core.Interfaces.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rubicon_Server_Two.Controllers
{
    [Route("/api/[controller]")]
    [Controller]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;


        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("registration")]
        public async Task<ActionResult<UserInformationDto>> Register(UserIdentityDto userIdentityDto)
        {
            UserInformationBlo userInformationBlo = await _userService.RegisterWithPhone(userIdentityDto.NumberPrefix, userIdentityDto.Number, userIdentityDto.Password);

            return await ConvertToUserInformationAsync(userInformationBlo);
        }

        [HttpPost("authwithphone")]
        public async Task<ActionResult> AuthWithPhone(UserIdentityDto userIdentityDto)
        {
            try
            {
                await _userService.AuthWithPhone(userIdentityDto.Number, userIdentityDto.NumberPrefix, userIdentityDto.Password);
                return Ok();
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPost("authwithemail")]
        public async Task<ActionResult> AuthWithEmail(UserIdentityDto userIdentityDto)
        {
            try
            {
                await _userService.AuthWithEmail(userIdentityDto.Email, userIdentityDto.Password);
                return Ok();
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPost("authwithlogin")]
        public async Task<ActionResult> AuthWithLogin(UserIdentityDto userIdentityDto)
        {
            try
            {
                await _userService.AuthWithLogin(userIdentityDto.Login, userIdentityDto.Password);
                return Ok();
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpGet("Get/{userId}")]
        public async Task<ActionResult> Get(int userId)
        {
            try
            {
                await _userService.Get(userId);
                return Ok();
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPatch("update")]
        public async Task<ActionResult> Update(UserUpdateDto userUpdateDto)
        {
            try
            {
                UserUpdateBlo userUpdateBlo = _mapper.Map<UserUpdateBlo>(userUpdateDto);
                await _userService.Update(userUpdateBlo);
                return Ok();
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPatch("DoesExist")]
        public async Task<ActionResult> DoesExist(UserIdentityDto userIdentityDto)
        {
            try
            {
                await _userService.DoesExist(userIdentityDto.Number, userIdentityDto.NumberPrefix);
                return Ok();
            }
            catch
            {
                return NotFound();
            }
        }

        private async Task<UserInformationDto> ConvertToUserInformationAsync(UserInformationBlo userInformationBlo)
        {
            if (userInformationBlo == null) throw new ArgumentNullException(nameof(userInformationBlo));

            UserInformationDto userInformationDto = _mapper.Map<UserInformationDto>(userInformationBlo);

            return userInformationDto;

        }
    }
}
