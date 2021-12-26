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

        private async Task<UserInformationDto> ConvertToUserInformationAsync(UserInformationBlo userInformationBlo)
        {
            if (userInformationBlo == null) throw new ArgumentNullException(nameof(userInformationBlo));

            UserInformationDto userInformationDto = _mapper.Map<UserInformationDto>(userInformationBlo);

            return userInformationDto;

        }
    }
}
